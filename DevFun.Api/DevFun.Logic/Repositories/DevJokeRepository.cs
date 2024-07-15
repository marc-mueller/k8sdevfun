using System;
using System.Linq;
using System.Linq.Expressions;
using DevFun.Common.Entities;
using DevFun.Common.Repositories;
using Microsoft.Extensions.Logging;
using ReaFx.DataAccess.Common.Mapper;
using ReaFx.DataAccess.Common.Repositories;
using ReaFx.DataAccess.Common.Security;
using ReaFx.DataAccess.Common.Storages;

namespace DevFun.Logic.Repositories
{
    public class DevJokeRepository : RepositoryBase<DevJoke, int>, IDevJokeRepository
    {
        public DevJokeRepository(IStorage storage, IMapperFactory mapperFactory, ISecurityContext securityContext, ILogger<DevJokeRepository> logger)
            : base(storage, mapperFactory, securityContext, logger)
        {
        }

        protected override Expression<Func<DevJoke, int>> IdPropertyExpression => e => e.Id;

        protected override IQueryable<DevJoke> ApplyDefaultIncludes(IQueryable<DevJoke> query)
        {
            return query.Include(i => i.Category, this);
        }

        protected override Expression<Func<DevJoke, bool>> DefineEntitySecurityFilterExpression(RepositoryAction action)
        {
            return e => true;
        }
    }
}