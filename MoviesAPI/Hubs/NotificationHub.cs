using Microsoft.AspNetCore.SignalR;

namespace MoviesAPI.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task BroadcastText(string message)
        {
            await Clients.All.SendAsync("ReceiveTextNotification", message);
        }
    }
}
