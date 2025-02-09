using back_end.Models;
using back_end.ViewModels.AttendanceRequest;

namespace back_end.Services.Attendance
{
    public interface IAttendanceRequestService
    {
        List<AttendanceRequest> GetPendingRequests();
        bool SubmitRequest(AttendanceRequestCreateViewModel requestViewModel);
        bool ApproveRequest(int requestId);
        bool DeclineRequest(int requestId);
    }
}
