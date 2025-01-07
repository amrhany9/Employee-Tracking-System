using back_end.Data;
using back_end.Models;
using System.Linq;
using zkemkeeper;
using System.Runtime.InteropServices;
using back_end.Repositories;
using back_end.Services.Attendance;
using System.Timers;
using back_end.Constants.Enums;
using Microsoft.AspNetCore.SignalR;
using back_end.Hubs;

namespace back_end.Services.ZKEM_Machine
{
    public class MachineService : IMachineService
    {
        private IServiceScopeFactory _serviceScopeFactory;
        private IHubContext<AttendanceHub> _attendancehubContext;

        private string _deviceIp;
        private int _devicePort;

        private CZKEM _zkemKeeper;
        public bool _isConnected;
        public int _lastErrorCode;

        public int TimerInterval;
        public System.Timers.Timer zkTimer1;

        public MachineService(IServiceScopeFactory serviceScopeFactory, IHubContext<AttendanceHub> attendancehubContext)
        {
            _zkemKeeper = new CZKEM();
            _isConnected = false;
            _lastErrorCode = 0;

            _serviceScopeFactory = serviceScopeFactory;
            _attendancehubContext = attendancehubContext;

            TimerInterval = 20000;
            zkTimer1 = new System.Timers.Timer(TimerInterval);
        }

        public void setDeviceNetwork(string deviceIp, int devicePort)
        {
            _deviceIp = deviceIp;
            _devicePort = devicePort;
            Connect();
        }

        public bool isConnected()
        {
            return _isConnected;
        }

        private void Connect()
        {
            _isConnected = _zkemKeeper.Connect_Net(_deviceIp, _devicePort);

            RegisterEvents();
        }

        public void RegisterEvents()
        {
            if (!_isConnected) return;

            if (_zkemKeeper.RegEvent(1, 1))
            {
                _zkemKeeper.OnAttTransaction += new _IZKEMEvents_OnAttTransactionEventHandler(ProcessNewAttendance);

                zkTimer1.Elapsed += new ElapsedEventHandler(zkTimer1_Tick);
                zkTimer1.Enabled = true;
            }
        }

        public void UnregisterEvents()
        {
            zkTimer1.Enabled = false;
            zkTimer1.Elapsed -= new ElapsedEventHandler(zkTimer1_Tick);

            _zkemKeeper.OnAttTransaction -= new _IZKEMEvents_OnAttTransactionEventHandler(ProcessNewAttendance);
        }

        protected virtual void zkTimer1_Tick(object sender, ElapsedEventArgs e) 
        {
            try
            {
                zkTimer1.Enabled = false; 
                if (_isConnected)
                {
                    if (_zkemKeeper.ReadRTLog(1))
                    {
                        while (_isConnected && _zkemKeeper.GetRTLog(1))
                        {
                            ;
                        }
                    }
                    else
                    {
                        GetLastError();
                        if (_lastErrorCode == -7 || _lastErrorCode == -1) Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                zkTimer1.Enabled = _isConnected;
            }
        }

        private void ProcessNewAttendance(int EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second)
        {
            DateTime checkDate = new DateTime(Year, Month, Day, Hour, Minute, Second);

            Models.Attendance attendance = new Models.Attendance
            {
                UserId = EnrollNumber,
                VerifyMode = (Constants.Enums.VerifyMode)VerifyMethod,
                CheckType = (Constants.Enums.CheckType)AttState,
                CheckDate = checkDate,
            };

            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var attendanceService = scope.ServiceProvider.GetRequiredService<IAttendanceService>();
                var userRepository = scope.ServiceProvider.GetRequiredService<IRepository<User>>();
                //var attendanceHub = scope.ServiceProvider.GetRequiredService<IHubContext<AttendanceHub>>();
                    
                var user = userRepository.GetById(attendance.UserId).First();

                if (user != null)
                {
                    switch (attendance.CheckType)
                    {
                        case CheckType.CheckIn:
                            user.IsCheckedIn = true;
                            userRepository.Update(user);
                            break;

                        case CheckType.CheckOut:
                            user.IsCheckedIn = false;
                            userRepository.Update(user);    
                            break;
                    }
                }

                userRepository.SaveChanges();

                attendanceService.AddAttendance(attendance);
                attendanceService.SaveChanges();

                //attendanceHub.Clients.Group("Admins").SendAsync("NewAttendance", attendance);
                _attendancehubContext.Clients.Group("Admins").SendAsync("NewAttendance", attendance);
            }
        }

        public List<Models.Attendance> GetDailyAttendanceRecords()
        {
            List<Models.Attendance> logs = [];
            DateTime today = DateTime.Now;

            if (_zkemKeeper.ReadGeneralLogData(1))
            {
                int dwTMachineNumber = 0;
                int enrollNumber = 0;
                int dwEMachineNumber = 0;
                int verifyMode = 0;
                int inOutMode = 0;
                int yearValue = 0, monthValue = 0, day = 0, hour = 0, minute = 0;

                while (_zkemKeeper.GetGeneralLogData(1,
                    ref dwTMachineNumber,
                    ref enrollNumber,
                    ref dwEMachineNumber,
                    ref verifyMode,
                    ref inOutMode,
                    ref yearValue,
                    ref monthValue,
                    ref day,
                    ref hour,
                    ref minute))
                {
                    if (yearValue == today.Year && monthValue == today.Month && day == today.Day)
                    {
                        Models.Attendance userAttendance = new Models.Attendance
                        {
                            UserId = enrollNumber,
                            VerifyMode = (Constants.Enums.VerifyMode)verifyMode,
                            CheckType = (Constants.Enums.CheckType)inOutMode,
                            CheckDate = new DateTime(yearValue, monthValue, day, hour, minute, 0),
                        };

                        if (!logs.Any(log => log.CheckDate == userAttendance.CheckDate))
                        {
                            logs.Add(userAttendance);
                        }
                    }
                }

                return logs;
            }
            else
            {
                throw new Exception($"Error reading general log data from the fingerprint device. Error Code: {GetLastError()}");
            }
        }

        public int GetLastError()
        {
            _zkemKeeper.GetLastError(ref _lastErrorCode);
            return _lastErrorCode;
        }

        private void Disconnect()
        {
            if (_isConnected)
            {
                _zkemKeeper.Disconnect();
                _isConnected = false;

                UnregisterEvents();
            }
        }

        public void Dispose()
        {
            Disconnect();
            if (zkTimer1 != null)
            {
                zkTimer1.Dispose();
                zkTimer1 = null;
            }
        }

        ~MachineService()
        {
            Dispose();
        }
    }
}
