using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using MppMonitor.ExternalDependencies.Interfaces;
using MppMonitor.Hubs;
using MppMonitor.Models.RestApi;

namespace MppMonitor.Tickers
{
    public class ArQueueSizeTicker : IHostedService, IDisposable
    {
        private readonly object _updateLock = new object();

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(2500);

        private Timer _timer;
        private volatile bool _updatingData = false;

        public ArQueueSizeTicker(IHubContext<ArQueueSizeHub> clients, IRestApi restApi)
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

        private IHubContext<ArQueueSizeHub> Clients { get; set; }

        private IRestApi RestApi { get; set; }

        private void UpdateStockPrices(object state)
        {
            lock (_updateLock)
            {
                if (!_updatingData)
                {
                    _updatingData = true;

                    BroadcastUpdate(GetData().Result.First());

                    _updatingData = false;
                }
            }
        }

        private async Task<List<ArQueueSize>> GetData()
        {
            return await RestApi.GetArQueueSizeDataAsync();
        }

        private void BroadcastUpdate(ArQueueSize stock)
        {
            Clients.Clients.All.SendAsync("UpdateArQueueSize", stock.Size, DateTime.Parse(stock.Time).ToString("HH:mm:ss"));
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
