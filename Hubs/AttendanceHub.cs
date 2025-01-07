using back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace back_end.Hubs
{
    [Authorize]
    public class AttendanceHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User == null) throw new HubException("Cannot get current user claim");

            var userRole = Context.User.FindFirst(ClaimTypes.Role)?.Value;

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

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User == null) throw new HubException("Cannot get current user claim");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
