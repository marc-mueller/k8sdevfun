using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DevFun.Api.Health
{
    public class StartupHealthCheck : IHealthCheck
    {
        public static DateTimeOffset startupTimestamp;
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            // some demo startup mechanism to throttle startup
            if (startupTimestamp == default(DateTimeOffset))
            {
                startupTimestamp = DateTimeOffset.UtcNow;
            }

            if (startupTimestamp.AddSeconds(10) >= DateTimeOffset.UtcNow)
            {
                return Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy, "Sample startup health check"));
            }

            return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy, "Sample startup health check"));
        }
    }
}
