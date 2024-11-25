using back_end.Models;

namespace back_end.Services.ZKEM_Machine
{
    public interface IMachineService
    {
        void setDeviceNetwork(string deviceIp, int devicePort);
        bool isConnected();
        IEnumerable<Attendance> GetDailyAttendanceRecords();
    }
}
