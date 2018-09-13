using Microsoft.AspNetCore.SignalR;
using MppMonitor.Models;
using System.Threading.Tasks;

namespace MppMonitor.Hubs
{
    public class PaymentAuthRateHub : Hub
    {
        public async Task UpdatePaymentAuthRate(string paymentProvider, PaymentSuccessRate authRate)
        {
            await Clients.All.SendAsync($"UpdatePaymentAuthRate-{paymentProvider}", authRate.AuthRate);
        }
    }
}
