using back_end.Models;

namespace back_end.Interfaces
{
    public interface IAttMachineService
    {
        public List<Attendance> GetDailyAttendanceRecords();
    }
}
