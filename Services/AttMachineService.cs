using back_end.Data;
using back_end.Models;
using back_end.Interfaces;
using System.Linq;
using zkemkeeper;

namespace back_end.Services
{
    public class AttMachineService : IAttMachineService
    {
        private string deviceIp;
        private int devicePort;

        private CZKEM _zkemKeeper;
        private bool _isConnected;
        private int _lastErrorCode;

        private ApplicationDbContext _context;

        public AttMachineService(ApplicationDbContext context)
        {
            this._zkemKeeper = new CZKEM();
            this._isConnected = false;
            this._lastErrorCode = 0;
            _context = context;
        }

        public void setDeviceNetwork(string deviceIp, int devicePort)
        {
            this.deviceIp = deviceIp;
            this.devicePort = devicePort;
        }

        public bool isConnected()
        {
            return this._isConnected;
        }

        public void Connect()
        {
            this._isConnected = this._zkemKeeper.Connect_Net(this.deviceIp, this.devicePort);

            if (!this._isConnected)
            {
                GetLastError();
                throw new Exception($"Error connecting to the fingerprint device. Error Code: {_lastErrorCode}");
            }
        }

        public List<Attendance> GetDailyAttendanceRecords()
        {
            List<Attendance> logs = new List<Attendance>();

            DateTime today = DateTime.Now;

            if (this._zkemKeeper.ReadGeneralLogData(1))
            {
                int dwTMachineNumber = 0;
                int enrollNumber = 0;
                int dwEMachineNumber = 0;
                int verifyMode = 0;
                int inOutMode = 0;
                int yearValue = 0, monthValue = 0, day = 0, hour = 0, minute = 0;

                while (this._zkemKeeper.GetGeneralLogData(1,
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
                        var userAccount = _context.Accounts.FirstOrDefault(x => x.UserId == enrollNumber);

                        if (userAccount != null)
                        {
                            Attendance userAttendance = new Attendance
                            {
                                UserId = enrollNumber,
                                VerifyMode = verifyMode,
                                CheckType = inOutMode,
                                CheckDate = new DateTime(yearValue, monthValue, day, hour, minute, 0),
                            };

                            logs.Add(userAttendance);
                        }
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
            this._zkemKeeper.GetLastError(ref _lastErrorCode);
        }

        public void Disconnect()
        {
            if (this._isConnected)
            {
                this._zkemKeeper.Disconnect();
                this._isConnected = false;
            }
        }
    }
}
