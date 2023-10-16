using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using static Other.Api.Controllers.WeatherForecastController;

namespace Other.Api.Hubs
{
    [Authorize]
    public class HelloHub : Hub
    {
        
        public async Task SendMessage(string message)
        {
            //var _user = Context.User; // Получаем пользователя из контекста хаба
            //var claims = _user?.Claims.Select(c => new ClaimModel { Type = c.Type, Value = c.Value }).FirstOrDefault(x => x.Type.ToLower() == "name");
            var messageObject = new
            {
                user = "qwe",
                text = message,
                Status = "Ok"
            };
            //Console.WriteLine(claims.Value);
            await Clients.All.SendAsync("ReceiveMessage", messageObject);
        }
    }
}
