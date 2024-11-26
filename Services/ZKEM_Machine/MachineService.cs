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
        private string _deviceIp;
        private int _devicePort;

        private CZKEM _zkemKeeper;
        public bool _isConnected;
        public int _lastErrorCode;

        public MachineService()
        {
            _zkemKeeper = new CZKEM();
            _isConnected = false;
            _lastErrorCode = 0;
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

                Models.Attendance attendance = new Models.Attendance
                {
                    UserId = EnrollNumber,
                    VerifyMode = VerifyMethod,
                    CheckType = AttState,
                    CheckDate = checkDate,
                };

                //_attendanceRepository.Add(attendance);
                //_attendanceRepository.SaveChanges();
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
                            VerifyMode = verifyMode,
                            CheckType = inOutMode,
                            CheckDate = new DateTime(yearValue, monthValue, day, hour, minute, 0),
                        };

                        logs.Add(userAttendance);
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
            }
        }

        ~MachineService()
        {
            Disconnect();
        }
    }
}
