using back_end.Models;

namespace back_end.Services.ZKEM_Machine
{
    public interface IMachineService
    {
        void setDeviceNetwork(string deviceIp, int devicePort);
        bool isConnected();
        List<Models.Attendance> GetDailyAttendanceRecords();
        int GetLastError();
    }
}
