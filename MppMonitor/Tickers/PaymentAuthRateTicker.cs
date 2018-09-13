using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using MppMonitor.ExternalDependencies.Interfaces;
using MppMonitor.Hubs;
using MppMonitor.Models;
using MppMonitor.Models.RestApi;

namespace MppMonitor.Tickers
{
    public class PaymentAuthRateTicker : IHostedService, IDisposable
    {
        private readonly object _updateLock = new object();

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(30000);

        private Timer _timer;
        private volatile bool _updatingData = false;

        public PaymentAuthRateTicker(IHubContext<PaymentAuthRateHub> clients, IRestApi restApi)
        {
            Clients = clients;
            RestApi = restApi;

            _timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private IHubContext<PaymentAuthRateHub> Clients { get; set; }

        private IRestApi RestApi { get; set; }

        private void UpdateStockPrices(object state)
        {
            lock (_updateLock)
            {
                if (!_updatingData)
                {
                    _updatingData = true;

                    var data = GetData().Result;

                    const string realex = "Realex";
                    var realexRate = GetData(data, realex);
                    BroadcastUpdate(realex, new PaymentSuccessRate { AuthRate = realexRate });

                    const string secpay = "SecPayVpn";
                    var secpayRate = GetData(data, secpay);
                    BroadcastUpdate(secpay, new PaymentSuccessRate { AuthRate = secpayRate });

                    var overallRate = GetData(data);
                    BroadcastUpdate("Overall", new PaymentSuccessRate { AuthRate = overallRate });

                    _updatingData = false;
                }
            }
        }

        private static double GetData(List<PaymentAuthRate> data, string paymentProvider)
        {
            return GetData(data.Where(d => d.PaymentProvider == paymentProvider));
        }

        private static double GetData(IEnumerable<PaymentAuthRate> data)
        {
            var auth = data.Sum(d => d.Renew_CountOfAuthorisations + d.Retry_CountOfAuthorisations + d.First_CountOfAuthorisations);
            var failure = data.Sum(d => d.First_CountOfDeclines + d.First_CountOfErrors + d.Retry_CountOfDeclines + d.Retry_CountOfErrors + d.Renew_CountOfDeclines + d.Renew_CountOfErrors);
            return Math.Round(100 * ((double) auth / ((double) auth + (double) failure)), 2);
        }

        private async Task<List<PaymentAuthRate>> GetData()
        {
            return await RestApi.GetPaymentAuthRateDataAsync(DateTime.Now.AddHours(-1), DateTime.Now);
        }

        private void BroadcastUpdate(string paymentProvider, PaymentSuccessRate rate)
        {
            Clients.Clients.All.SendAsync($"UpdatePaymentAuthRate-{paymentProvider}", rate.AuthRate);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
