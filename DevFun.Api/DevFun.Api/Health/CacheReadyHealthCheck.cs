using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DevFun.Api.Health
{
    public class CacheReadyHealthCheck : IHealthCheck
    {
        public static DateTimeOffset cacheReadyDelay;
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            // some demo startup mechanism to throttle startup
            if (cacheReadyDelay == default(DateTimeOffset))
            {
                cacheReadyDelay = DateTimeOffset.UtcNow;
            }

            if (cacheReadyDelay.AddSeconds(10) >= DateTimeOffset.UtcNow)
            {
                return Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy, "Sample cache ready health check"));
            }

            return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy, "Sample cache ready health check"));
        }
    }
}
