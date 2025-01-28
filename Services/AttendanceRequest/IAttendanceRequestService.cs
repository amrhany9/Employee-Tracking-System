using back_end.Models;
using back_end.ViewModels.AttendanceRequest;

namespace back_end.Services.Attendance
{
    public interface IAttendanceRequestService
    {
        List<AttendanceRequestViewModel> GetPendingRequests();
        bool SubmitRequest(AttendanceRequestViewModel requestViewModel);
        bool ApproveRequest(int requestId);
        bool DeclineRequest(int requestId);
    }
}
