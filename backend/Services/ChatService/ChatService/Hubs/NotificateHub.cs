using Microsoft.AspNetCore.SignalR;
using Tools.Controller;

namespace ChatService.Hubs
{
    public class NotificateHub : Hub
    {
        public async Task Send(string message)
        {
            var user = GetObject.GetUser(GetObject.GetClaimsFromToken(Context.GetHttpContext().Request.Query["access_token"]));
        }

        public override async Task OnConnectedAsync()
        { 
            await base.OnConnectedAsync();
        }
    }
}
