using Microsoft.Extensions.Configuration;
using MppMonitor.ExternalDependencies.Interfaces;
using MppMonitor.Models.RestApi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace MppMonitor.ExternalDependencies
{
    public class RestApi : IRestApi
    {
        private static readonly HttpClient client = new HttpClient();

        public IConfiguration Configuration { get; }

        public RestApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<List<ArQueueSize>> GetArQueueSizeDataAsync()
        {
            var serializer = new DataContractJsonSerializer(typeof(List<ArQueueSize>));

            var streamTask = GetData("ARPendingNow");
            var repositories = serializer.ReadObject(await streamTask) as List<ArQueueSize>;

            return repositories;
        }

        public async Task<List<PaymentAuthRate>> GetPaymentAuthRateDataAsync(DateTime startDate, DateTime endDate)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<PaymentAuthRate>));

            var streamTask = GetData("CreditDebitCardAdvancedAuthorisations_PSP", startDate, endDate);
            var repositories = serializer.ReadObject(await streamTask) as List<PaymentAuthRate>;

            return repositories;
        }

        private Task<System.IO.Stream> GetData(string method, DateTime startDate, DateTime endDate)
        {
            return GetData($"CreditDebitCardAdvancedAuthorisations_PSP?startDate={startDate}&endDate={endDate}");
        }

        private Task<System.IO.Stream> GetData(string method)
        {
            lock (client)
            {
                var url = Configuration["URL"];
                var username = Configuration["API-Username"];
                var password = Configuration["API-Password"];

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-AffiliateId", "451");
                client.DefaultRequestHeaders.Add("X-Username", username);
                client.DefaultRequestHeaders.Add("X-Password", password);                

                return client.GetStreamAsync($"{url}/api/analytics/{method}");
            }
        }
    }
}
