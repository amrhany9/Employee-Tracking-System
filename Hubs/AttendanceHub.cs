using back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace back_end.Hubs
{
    public class AttendanceHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
            await base.OnConnectedAsync();
        }

        public void NotifyNewAttendance(Attendance attendance)
        {
             Clients.Group("Admins").SendAsync("ProcessNewAttendance", attendance);
        }
    }
}
