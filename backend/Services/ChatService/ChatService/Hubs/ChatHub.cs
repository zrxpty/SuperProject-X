using ChatService.Data;
using ChatService.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Tools.Controller;

namespace ChatService.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly AppDbContext _db;
        public ChatHub(AppDbContext db) 
        { 
            _db = db;
        }


        public async Task SendMessage(string message, string to, List<byte[]> files)
        {
            var user = await GetCurrentUser();

            var chatMessage = new Message
            {
                Body = message,
                Created = DateTime.UtcNow,
                To = to,
                From = user.Login,
            };

            await _db.Messages.AddAsync(chatMessage);
            await _db.SaveChangesAsync();

            await Clients.All.SendAsync(to, chatMessage);
        }

        public override async Task OnConnectedAsync()
        {
            var user = await GetCurrentUser();
            await Clients.All.SendAsync("UserConnected", user.Login);
            await base.OnConnectedAsync();
            
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await GetCurrentUser();
            await Clients.All.SendAsync("UserDisconnected", user.Login);
            await base.OnDisconnectedAsync(exception);
        }

        #region
        private async Task<User> GetCurrentUser()
        {
            var user = GetObject.GetUser(GetObject.GetClaimsFromToken(Context.GetHttpContext().Request.Query["access_token"]));
            return await _db.Users.FirstOrDefaultAsync(x => x.UserId == user.Id);
        }
        #endregion
    }
}
