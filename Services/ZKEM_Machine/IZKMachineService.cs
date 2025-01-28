using back_end.Models;

namespace back_end.Services.ZKEM_Machine
{
    public interface IZKMachineService
    {
        void setDeviceNetwork(Machine machine);
        bool isConnected();
        List<Models.Attendance> GetDailyAttendanceRecords();
        int GetLastError();
        void Dispose();
    }
}
