using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace back_end.Hubs
{
    [Authorize(Roles = "Admin")]
    public class AttendanceHub : Hub
    {
        
    }
}
