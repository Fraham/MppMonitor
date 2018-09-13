using MppMonitor.Models.RestApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MppMonitor.ExternalDependencies.Interfaces
{
    public interface IRestApi
    {
        Task<List<ArQueueSize>> GetArQueueSizeDataAsync();

        Task<List<PaymentAuthRate>> GetPaymentAuthRateDataAsync(DateTime startDate, DateTime endDate);
    }
}
