using AutoMapper;
using back_end.Constants.Enums;
using back_end.Data;
using back_end.Hubs;
using back_end.Migrations;
using back_end.Models;
using back_end.Repositories;
using back_end.ViewModels.AttendanceRequest;
using Microsoft.AspNetCore.SignalR;

namespace back_end.Services.Attendance
{
    public class AttendanceRequestService : IAttendanceRequestService
    {
        private ApplicationDbContext _context;
        private IHubContext<AttendanceHub> _attendanceHubContext;
        private IAttendanceService _attendanceService;
        private IRepository<AttendanceRequest> _attendanceRequestsRepository;
        private IRepository<Employee> _employeeRepository;
        private IMapper _mapper;

        public AttendanceRequestService(ApplicationDbContext context, IHubContext<AttendanceHub> attendanceHubContext, IAttendanceService attendanceService, IRepository<AttendanceRequest> attendanceRequestsRepository, IRepository<Employee> employeeRepository, IMapper mapper)
        {
            _context = context;
            _attendanceHubContext = attendanceHubContext;
            _attendanceService = attendanceService;
            _attendanceRequestsRepository = attendanceRequestsRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public List<AttendanceRequestViewModel> GetPendingRequests()
        {
            var pendingRequests = _attendanceRequestsRepository
                .GetByFilter(x => x.status == RequestStatus.Pending)
                .ToList();

            return _mapper.Map<List<AttendanceRequestViewModel>>(pendingRequests);
        }

        public bool SubmitRequest(AttendanceRequestViewModel requestViewModel)
        {
            var attendanceRequest = _mapper.Map<AttendanceRequest>(requestViewModel);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _attendanceRequestsRepository.Add(attendanceRequest);
                _attendanceRequestsRepository.SaveChanges();

                _attendanceHubContext.Clients.Group("Admins").SendAsync("NewAttendanceRequest", attendanceRequest);

                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return false;
            }
        }

        public bool ApproveRequest(int requestId)
        {
            var request = _attendanceRequestsRepository.GetByFilter(x => x.requestId == requestId).Single();

            if (request == null)
            {
                return false;
            }

            var employee = _employeeRepository.GetByFilter(x => x.employeeId == request.employeeId).Single();

            if (employee == null)
            {
                return false;
            }

            var attendance = _mapper.Map<Models.Attendance>(request);

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _attendanceService.AddAttendance(attendance);
                _attendanceService.SaveChanges();

                request.status = RequestStatus.Approved;
                request.employee.isCheckedIn = true;

                _attendanceRequestsRepository.Update(request);
                _attendanceRequestsRepository.SaveChanges();
                transaction.Commit();

                _attendanceHubContext.Clients.Group("Admins").SendAsync("ApprovedAttendanceRequest", requestId);

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return false;
            }
        }

        public bool DeclineRequest(int requestId)
        {
            var request = _attendanceRequestsRepository.GetByFilter(x => x.requestId == requestId).Single();

            if (request == null)
            {
                return false;
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                request.status = RequestStatus.Declined;

                _attendanceRequestsRepository.Update(request);
                _attendanceRequestsRepository.SaveChanges();

                transaction.Commit();

                _attendanceHubContext.Clients.Group("Admins").SendAsync("DeclinedAttendanceRequest", requestId);

                return true;
            }
            catch(Exception ex)
            {
                transaction.Rollback();

                return false;
            }
        }
    }
}
