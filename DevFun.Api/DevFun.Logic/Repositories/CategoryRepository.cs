using System;
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
    public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
    {
        public CategoryRepository(IStorage storage, IMapperFactory mapperFactory, ISecurityContext securityContext, ILogger<CategoryRepository> logger)
            : base(storage, mapperFactory, securityContext, logger)
        {
        }

        protected override Expression<Func<Category, int>> IdPropertyExpression => e => e.Id;

        protected override Expression<Func<Category, bool>> DefineEntitySecurityFilterExpression(RepositoryAction action)
        {
            return e => true;
        }
    }
}