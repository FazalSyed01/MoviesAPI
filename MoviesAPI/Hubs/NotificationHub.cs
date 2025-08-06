using Microsoft.AspNetCore.SignalR;
using MoviesAPI.Entities;
using KaamKaaj.Application.Interfaces;

namespace MoviesAPI.Hubs
{
    public class NotificationHub : Hub
    {

        private readonly IJobsService _jobsService;

        public NotificationHub(IJobsService jobsService)
        {
            _jobsService = jobsService;
        }
        public async Task JobAdded(Jobs job)
        {
            await _jobsService.AddJobsAsync(job);
            await Clients.All.SendAsync("ReceiveJobNotification", job);
        }
        public async Task BroadcastText(string message)
        {
            await Clients.All.SendAsync("ReceiveTextNotification", message);

        }
    }
}
