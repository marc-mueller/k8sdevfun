using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DevFun.Web.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ReaFx.ApiClients.Common.Factory;

namespace DevFun.Web.Health
{
    public class BackendConnectivityHealthCheck : IHealthCheck
    {
        private readonly DevFunOptions apiOptions;
        private readonly IServiceClientFactory clientFactory;
        private const string description = "Sample backend connectivity health check";

        public BackendConnectivityHealthCheck(IServiceClientFactory clientFactory, DevFunOptions apiOptions)
        {
            this.clientFactory = clientFactory;
            this.apiOptions = apiOptions;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {

            var data = new Dictionary<string, object>();
            var success = false;
            HttpResponseMessage responseApiUrl = null;
            try
            {
                using (var client = await clientFactory.GetHttpClient().ConfigureAwait(false))
                {
                    client.BaseAddress = new Uri(apiOptions.ApiUrl);
                    data["TestUrl"] =  client.BaseAddress;
                    responseApiUrl = await client.GetAsync("/health", cancellationToken).ConfigureAwait(false);
                    if (responseApiUrl != null && responseApiUrl.IsSuccessStatusCode)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception) { }

            if(!success)
            {
                try
                {
                    using (var client = await clientFactory.GetHttpClient().ConfigureAwait(false))
                    {
                        data["MainApiUrlFailed"] =  true;

                        client.BaseAddress = new Uri(apiOptions.AlternateTestingUrl);
                        data["TestUrl"] = client.BaseAddress;
                        var responseAlternateApiUrl = await client.GetAsync("/health", cancellationToken).ConfigureAwait(false);
                        if (responseAlternateApiUrl != null && responseAlternateApiUrl.IsSuccessStatusCode)
                        {
                            success = true;
                        }
                    }
                }
                catch (Exception) { }
            }

            if (success)
            {
                return new HealthCheckResult(HealthStatus.Healthy, description, data: new ReadOnlyDictionary<string, object>(data));
            }
            else
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, description, data: new ReadOnlyDictionary<string, object>(data));
            }

        }
    }
}
