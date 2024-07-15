using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using ReaFx.DataAccess.Common.Mapper;
using ReaFx.DataAccess.Common.Repositories;
using ReaFx.DataAccess.Common.Security;
using ReaFx.DataAccess.Common.Storages;
using ReaFx.Services.ViewDefinitionManagement.ServerModule.Storage.Entities;
using ReaFx.Services.ViewDefinitionManagement.ServerModule.Storage.Repositories;

namespace DevFun.Logic.Repositories
{
    public class CustomViewDefinitionRepository : ViewDefinitionRepository
    {
        public CustomViewDefinitionRepository(IStorage storage, IMapperFactory mapperFactory, ISecurityContext securityContext, ILogger<CustomViewDefinitionRepository> logger)
            : base(storage, mapperFactory, securityContext, logger)
        {
        }

        protected override Expression<Func<ViewDefinition, bool>> DefineEntitySecurityFilterExpression(RepositoryAction action)
        {
            return e => true;
        }
    }
}
