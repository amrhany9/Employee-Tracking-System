using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back_end.Services.Attendance;
using back_end.ViewModels.AttendanceRequest;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AttendanceRequestController : ControllerBase
    {
        private IAttendanceRequestService _attendanceRequestService;

        public AttendanceRequestController(IAttendanceRequestService attendanceRequestService)
        {
            _attendanceRequestService = attendanceRequestService;
        }

        [HttpGet]
        public ActionResult GetPendingRequests()
        {
            var pendingRequests = _attendanceRequestService.GetPendingRequests();

            return Ok(pendingRequests);
        }

        [Authorize]
        [HttpPost("Submit")]
        public ActionResult SubmitRequest(AttendanceRequestViewModel requestViewModel)
        {
            var isSuccess = _attendanceRequestService.SubmitRequest(requestViewModel);

            if (isSuccess)
            {
                return Ok("Request sent successfully");
            }
            else
            {
                return BadRequest("Error in sending request");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Approve/{requestId}")]
        public ActionResult ApproveRequest(int requestId)
        {
            var isSuccess = _attendanceRequestService.ApproveRequest(requestId);

            if (isSuccess)
            {
                return Ok("Request approved successfully");
            }
            else
            {
                return BadRequest("Error in approving request");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Decline/{requestId}")]
        public ActionResult DeclineRequest(int requestId)
        {
            var isSuccess = _attendanceRequestService.ApproveRequest(requestId);

            if (isSuccess)
            {
                return Ok("Request declined successfully");
            }
            else
            {
                return BadRequest("Error in declining request");
            }
        }
    }
}
