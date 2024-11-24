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
    }
}
