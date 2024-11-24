using back_end.Models;

namespace back_end.Services.ZKEM_Machine
{
    public interface IMachineService
    {
        public IEnumerable<Attendance> GetDailyAttendanceRecords();
    }
}
