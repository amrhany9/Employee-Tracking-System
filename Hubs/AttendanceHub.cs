using back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace back_end.Hubs
{
    public class AttendanceHub : Hub
    {
        [Authorize]
        public override async Task OnConnectedAsync()
        {
            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Admin")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
                Console.WriteLine("Admin added to group.");
            }
            else
            {
                Console.WriteLine("Non-admin connected.");
            }

            await base.OnConnectedAsync();
        }

        public void NotifyNewAttendance(Attendance attendance)
        {
             Clients.Group("Admins").SendAsync("ProcessNewAttendance", attendance);
        }
    }
}
