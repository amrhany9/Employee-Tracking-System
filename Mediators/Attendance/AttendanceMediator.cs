using back_end.Repositories;
using back_end.Services.ZKEM_Machine;

namespace back_end.Mediators.Attendance
{
    public class AttendanceMediator : IAttendanceMediator
    {
        private IRepository<Models.Attendance> _attendanceRepository;
        private IMachineService _machineService;

        public AttendanceMediator(IRepository<Models.Attendance> attendanceRepository, IMachineService machineService)
        {
            _attendanceRepository = attendanceRepository;
            _machineService = machineService;
        }

        public void SyncDailyLogToDatabase()
        {
            IEnumerable<Models.Attendance> Log = _machineService.GetDailyAttendanceRecords();
            _attendanceRepository.AddRange(Log);
            _attendanceRepository.SaveChanges();
        }

        private void HandleDuplicatesInDatabase()
        {

        }

        public void MarkDailyLogAsDeleted()
        {
            IEnumerable<Models.Attendance> Log = _attendanceRepository.GetByFilter(x => x.CheckDate == DateTime.Today).ToList();
            foreach (var attendance in Log)
            {
                _attendanceRepository.Delete(attendance);
            }
            _attendanceRepository.SaveChanges();
        }

        public void ArchiveWeekLogs()
        {

        }
    }
}
