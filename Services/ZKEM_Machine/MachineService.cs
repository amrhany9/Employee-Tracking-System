using back_end.Data;
using back_end.Models;
using System.Linq;
using zkemkeeper;
using System.Runtime.InteropServices;
using back_end.Repositories;

namespace back_end.Services.ZKEM_Machine
{
    public class MachineService : IMachineService
    {
        private string deviceIp;
        private int devicePort;

        private CZKEM _zkemKeeper;
        private bool _isConnected;
        private int _lastErrorCode;

        private IRepository<Attendance> _attendanceRepository;

        public MachineService(IRepository<Attendance> attendanceRepository)
        {
            _zkemKeeper = new CZKEM();
            _isConnected = false;
            _lastErrorCode = 0;

            _attendanceRepository = attendanceRepository;
        }

        public void setDeviceNetwork(string deviceIp, int devicePort)
        {
            this.deviceIp = deviceIp;
            this.devicePort = devicePort;
        }

        public bool isConnected()
        {
            return _isConnected;
        }

        public void Connect()
        {
            _isConnected = _zkemKeeper.Connect_Net(deviceIp, devicePort);

            if (!_isConnected)
            {
                GetLastError();
                throw new Exception($"Error connecting to the fingerprint device. Error Code: {_lastErrorCode}");
            }
        }

        private void RegisterEvents()
        {
            if (!_isConnected) return;

            _zkemKeeper.OnAttTransaction += ProcessNewAttendance;
        }

        private void ProcessNewAttendance(int EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second)
        {
            if (IsInValid == 0)
            {
                DateTime checkDate = new DateTime(Year, Month, Day, Hour, Minute, Second);

                Attendance attendance = new Attendance
                {
                    UserId = EnrollNumber,
                    VerifyMode = VerifyMethod,
                    CheckType = AttState,
                    CheckDate = checkDate,
                };

                _attendanceRepository.Add(attendance);
                _attendanceRepository.SaveChanges();
            }
        }

        public IEnumerable<Attendance> GetDailyAttendanceRecords()
        {
            IEnumerable<Attendance> logs = new List<Attendance>();
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
                        Attendance userAttendance = new Attendance
                        {
                            UserId = enrollNumber,
                            VerifyMode = verifyMode,
                            CheckType = inOutMode,
                            CheckDate = new DateTime(yearValue, monthValue, day, hour, minute, 0),
                        };

                        logs.Append(userAttendance);
                    }
                }

                return logs;
            }
            else
            {
                GetLastError();
                throw new Exception($"Error reading general log data from the fingerprint device. Error Code: {_lastErrorCode}");
            }
        }

        public void GetLastError()
        {
            _zkemKeeper.GetLastError(ref _lastErrorCode);
        }

        public void Disconnect()
        {
            if (_isConnected)
            {
                _zkemKeeper.Disconnect();
                _isConnected = false;
            }
        }
    }
}
