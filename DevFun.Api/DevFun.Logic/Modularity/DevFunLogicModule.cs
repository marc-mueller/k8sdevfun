using DevFun.Common.Entities;
using DevFun.Common.Repositories;
using DevFun.Common.Services;
using DevFun.Logic.Repositories;
using DevFun.Logic.Services;
using ReaFx.DataAccess.Common.Extensions;
using ReaFx.DataAccess.Common.Mapper;
using ReaFx.DataAccess.Common.Repositories;
using ReaFx.DataAccess.Common.Services;
using ReaFx.DependencyInjection.Common;
using ReaFx.DependencyInjection.Common.Extensions;
using ReaFx.Mapper.GeneratedMappers;
using ReaFx.Modularity.Common;
using ReaFx.Services.LabelManagement.ServerModule.Modularity;
using ReaFx.Services.ViewDefinitionManagement.ServerModule.Modularity;

namespace DevFun.Logic.Modularity
{
    public class DevFunLogicModule : ModuleBase
    {
        public override IServiceDefinitionCollection RegisterServices(IServiceDefinitionCollection serviceCollection)
        {
            serviceCollection.RegisterEntity<DevJoke, IDevJokeRepository, DevJokeRepository>();
            serviceCollection.RegisterEntity<Category, ICategoryRepository, CategoryRepository>();

            serviceCollection.RegisterMappersDetachedEntity();
            serviceCollection.RegisterMappersDto();
            serviceCollection.AddTransient<IMapperFactory, MapperFactory>();

            serviceCollection.AddTransient<IEntitySecurityAndValidationProviderResolver, EntitySecurityAndValidationProviderResolver>();

            serviceCollection.AddTransient<IDevJokeService, DevJokeService>();
            serviceCollection.AddTransient<IEntityService<DevJoke, int>, DevJokeService>();
            serviceCollection.AddTransient<ICategoryService, CategoryService>();
            serviceCollection.AddTransient<IEntityService<Category, int>, CategoryService>();

            return serviceCollection;
        }
    }

    public static class ModuleCatalogExtensions
    {
        public static IModuleCatalogCollection AddDevFunLogicModule(this IModuleCatalogCollection moduleCatalog)
        {
            if (moduleCatalog is null)
            {
                throw new System.ArgumentNullException(nameof(moduleCatalog));
            }

            moduleCatalog.AddModule("DevFunLogicModule", new DevFunLogicModule());
            return moduleCatalog;
        }
        public static IModuleCatalogCollection AddCustomLabelServiceModule(this IModuleCatalogCollection moduleCatalog)
        {
            if (moduleCatalog is null)
            {
                throw new System.ArgumentNullException(nameof(moduleCatalog));
            }

            moduleCatalog.AddLabelServiceModule(options =>
            {
                options.SubsetPathProviderFactory = s => s.ResolveUnregistered<LabelSubsetPathProvider>();
                options.RepositoryFactory = s => s.ResolveUnregistered<CustomLabelRepository>();
            });
            return moduleCatalog;
        }

        public static IModuleCatalogCollection AddCustomViewDefinitionServiceModule(this IModuleCatalogCollection moduleCatalog)
        {
            if (moduleCatalog is null)
            {
                throw new System.ArgumentNullException(nameof(moduleCatalog));
            }

            moduleCatalog.AddViewDefinitionServiceModule(options =>
            {
                options.ViewDefinitionRepositoryFactory = s => s.ResolveUnregistered<CustomViewDefinitionRepository>();
            });

            return moduleCatalog;
        }
    }
}