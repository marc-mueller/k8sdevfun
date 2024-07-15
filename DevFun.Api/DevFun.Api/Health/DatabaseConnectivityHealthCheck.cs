using DevFun.Common.Repositories;
using DevFun.Common.Storages;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ReaFx.DataAccess.Common.Storages;

namespace DevFun.Api.Health
{
    public class DatabaseConnectivityHealthCheck : IHealthCheck
    {
        private const string description = "Sample database ready health check";
        private readonly IStorageFactory<IDevFunStorage> storageFactory;

        public DatabaseConnectivityHealthCheck(IStorageFactory<IDevFunStorage> storageFactory)
        {
            this.storageFactory = storageFactory ?? throw new ArgumentNullException(nameof(storageFactory));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            using IStorageSession session = storageFactory.CreateStorageSession();
            try
            {
                IDevJokeRepository repo = session.ResolveRepository<IDevJokeRepository>();
                ReaFx.DataAccess.Common.Repositories.IPagedEnumerable<Common.Entities.DevJoke> result = await repo.GetAll(take: 3).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, description, ex);
            }

            return new HealthCheckResult(HealthStatus.Healthy, description);
        }
    }
}
