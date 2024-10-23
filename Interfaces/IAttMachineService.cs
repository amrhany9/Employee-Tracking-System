using back_end.Entities;

namespace back_end.Interfaces
{
    public interface IAttMachineService
    {
        public List<UserAttendanceRecord> GetDailyAttendanceRecords();
    }
}
