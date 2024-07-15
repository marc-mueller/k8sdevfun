using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using ReaFx.DataAccess.Common.Mapper;
using ReaFx.DataAccess.Common.Repositories;
using ReaFx.DataAccess.Common.Security;
using ReaFx.DataAccess.Common.Storages;
using ReaFx.Services.LabelManagement.ServerModule.Settings;
using ReaFx.Services.LabelManagement.ServerModule.Storage.Entities;
using ReaFx.Services.LabelManagement.ServerModule.Storage.Repositories;

namespace DevFun.Logic.Repositories
{
    public class CustomLabelRepository : LabelRepository
    {
        public CustomLabelRepository(
            IStorage storage,
            IMapperFactory mapperFactory,
            LabelServiceSettings labelServiceSettings,
            ISecurityContext securityContext,
            ILogger<CustomLabelRepository> logger)
            : base(storage, mapperFactory, labelServiceSettings, securityContext, logger)
        {
        }

        protected override Expression<Func<Label, bool>> DefineEntitySecurityFilterExpression(RepositoryAction action)
        {
            return action switch
            {
                RepositoryAction.Create or RepositoryAction.Update => e => true,
                RepositoryAction.Delete => e => false,
                _ => e => true,
            };
        }
    }
}
