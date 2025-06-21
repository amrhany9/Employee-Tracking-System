using back_end.Models;
using zkemkeeper;
using back_end.Repositories;
using back_end.Services.Attendance;
using System.Timers;
using back_end.Constants.Enums;
using Microsoft.AspNetCore.SignalR;
using back_end.Hubs;

namespace back_end.Services.ZKEM_Machine
{
    public class ZKMachineService : IZKMachineService
    {
        private IServiceScopeFactory _serviceScopeFactory;
        private IHubContext<AttendanceHub> _attendancehubContext;

        private Machine currentMachine;

        private CZKEM _zkemKeeper;
        public bool _isConnected;
        public int _lastErrorCode;

        public int TimerInterval;
        public System.Timers.Timer zkTimer1;

        public ZKMachineService(IServiceScopeFactory serviceScopeFactory, IHubContext<AttendanceHub> attendancehubContext)
        {
            _zkemKeeper = new CZKEM();
            _isConnected = false;
            _lastErrorCode = 0;

            _serviceScopeFactory = serviceScopeFactory;
            _attendancehubContext = attendancehubContext;

            TimerInterval = 2000;
            zkTimer1 = new System.Timers.Timer(TimerInterval);
        }

        public void setDeviceNetwork(Machine machine)
        {
            currentMachine = machine;
            Connect();
        }

        public bool isConnected()
        {
            return _isConnected;
        }

        private void Connect()
        {
            _isConnected = _zkemKeeper.Connect_Net(currentMachine.Ip, currentMachine.Port);

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
                MachineCode = currentMachine.Code,
                EmployeeId = EnrollNumber,
                VerifyMode = (VerifyMode)VerifyMethod,
                CheckType = (CheckType)AttState,
                CheckDate = checkDate,
                Latitude = 0,
                Longitude = 0
            };

            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var attendanceService = scope.ServiceProvider.GetRequiredService<IAttendanceService>();
                var userRepository = scope.ServiceProvider.GetRequiredService<IRepository<Employee>>();
                    
                var user = userRepository.GetByFilter(x => x.Id == attendance.EmployeeId).FirstOrDefault();

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

                _attendancehubContext.Clients.Group("Admins").SendAsync("NewAttendance", attendance);

                userRepository.SaveChanges();

                attendanceService.AddAttendance(attendance);
                attendanceService.SaveChanges();
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
                            EmployeeId = enrollNumber,
                            VerifyMode = (VerifyMode)verifyMode,
                            CheckType = (CheckType)inOutMode,
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

        ~ZKMachineService()
        {
            Dispose();
        }
    }
}
