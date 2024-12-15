using AutoMapper;
using back_end.Repositories;
using back_end.Services.Attendance;
using back_end.Services.ZKEM_Machine;

namespace back_end.Mediators.Attendance
{
    public class AttendanceMediator : IAttendanceMediator
    {
        private IAttendanceService _attendanceService;
        private IRepository<Models.AttArchive> _attArchiveRepository;
        private IMachineService _machineService;
        private IMapper _mapper;

        public AttendanceMediator(IAttendanceService attendanceService, IRepository<Models.AttArchive> attArchiveRepository, IMachineService machineService, IMapper mapper)
        {
            _attendanceService = attendanceService;
            _attArchiveRepository = attArchiveRepository;
            _machineService = machineService;
            _mapper = mapper;
        }

        public void SyncDailyLog()
        {
            // To Be Continued
            _machineService.setDeviceNetwork("192.168.1.30", 4370);
            if (_machineService.isConnected())
            {
                var MachineLog = _machineService.GetDailyAttendanceRecords();
                var DbLog = _attendanceService.GetDailyLog();
                var UniqueLog = MachineLog.Except(DbLog);
                _attendanceService.AddLog(UniqueLog);
                _attendanceService.SaveChanges();
            }
        }

        public void DeleteDailyLog()
        {
            _attendanceService.DeleteLog(_attendanceService.GetDailyLog());
            _attendanceService.SaveChanges();
        }

        public void ArchiveWeekLogs()
        {
            var LogsToArchive = _attendanceService.GetLastWeekLog();
            var archiveLogs = _mapper.Map<IEnumerable<Models.AttArchive>>(LogsToArchive);
            _attendanceService.HardDeleteLog(LogsToArchive);
            _attArchiveRepository.AddRange(archiveLogs);
            _attendanceService.SaveChanges();
            _attArchiveRepository.SaveChanges();
        }
    }
}
