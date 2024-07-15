using System;
using System.Linq;
using System.Threading.Tasks;
using DevFun.Common.Entities;
using DevFun.Common.Repositories;
using DevFun.Common.Services;
using DevFun.Common.Storages;
using Microsoft.Extensions.Logging;
using ReaFx.DataAccess.Common.Repositories;
using ReaFx.DataAccess.Common.Services;
using ReaFx.DataAccess.Common.Storages;

namespace DevFun.Logic.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("AsyncUsage", "AsyncFixer01:Unnecessary async/await usage", Justification = "because of the using and Task<>, the object is disposed too early when not using async/await in combination with using")]
    public class DevJokeService : EntityServiceBase<DevJoke, int, IDevJokeRepository, IDevFunStorage>,IDevJokeService
    {
        public DevJokeService(IStorageFactory<IDevFunStorage> storageFactory, ILogger<DevJokeService> logger)
            : base(storageFactory, logger)
        {
        }

        protected override IIncludePath<DevJoke>[] GetServiceSpecificIncludes()
        {
            return new[] {this.CreateIncludePath(j => j.Category)};
        }

        public async Task<DevJoke> GetRandomJoke()
        {
            using var session = StorageFactory.CreateStorageSession();
            var repo = session.ResolveRepository<IDevJokeRepository>();
            var result = (await repo.GetAll().ConfigureAwait(false)).OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            return result;
        }
    }
}