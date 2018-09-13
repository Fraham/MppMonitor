using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MppMonitor.Hubs
{
    public class ArQueueSizeHub : Hub
    {
        public async Task UpdateArQueueSize(string size, string time)
        {
            await Clients.All.SendAsync("UpdateArQueueSize", size, time);
        }
    }
}
