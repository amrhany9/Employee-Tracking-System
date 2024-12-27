using back_end.Models;
using back_end.Repositories;

namespace back_end.Services.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private IRepository<Models.Attendance> _attendanceRepository;

        public AttendanceService(IRepository<Models.Attendance> attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public void AddAttendance(Models.Attendance attendance)
        {
            _attendanceRepository.Add(attendance);
        }

        public IQueryable<Models.Attendance> GetLastWeekLog()
        {
            return _attendanceRepository.GetDeletedByFilter(x => x.CheckDate.Date <= DateTime.Now.AddDays(-7));
        }

        public IQueryable<Models.Attendance> GetDailyLog()
        {
            return _attendanceRepository.GetByFilter(x => x.CheckDate.Date == DateTime.Today);
        }

        public void AddLog(IEnumerable<Models.Attendance> Log)
        {
            _attendanceRepository.AddRange(Log);
        }

        public void DeleteLog(IEnumerable<Models.Attendance> Log)
        {
            _attendanceRepository.DeleteRange(Log);
        }

        public void HardDeleteLog(IEnumerable<Models.Attendance> Log)
        {
            _attendanceRepository.HardDeleteRange(Log);
        }

        public void SaveChanges()
        {
            _attendanceRepository.SaveChanges();
        }
    }
}
