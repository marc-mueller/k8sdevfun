using ReaFx.DataAccess.Common.Storages;
using ReaFx.DataAccess.EntityFramework.Modularity;
using ReaFx.DataAccess.EntityFramework.Storages;
using ReaFx.DependencyInjection.Common;
using ReaFx.Modularity.Common;
using DevFun.Common.Storages;
using DevFun.Storage.EntityConfigurations;
using DevFun.Storage.Storages;
using ReaFx.DataAccess.Common.Extensions;
using ReaFx.DependencyInjection.Common.Extensions;

namespace DevFun.Storage.Modularity
{
    public class DevFunStorageModule : ModuleDataAccessEntityFramework
    {
        public override IServiceDefinitionCollection RegisterServices(IServiceDefinitionCollection serviceCollection)
        {
            base.RegisterServices(serviceCollection);
            serviceCollection.AddTransient<IRelationalEntityConfiguration, DevJokeConfiguration>();
            serviceCollection.AddTransient<IRelationalEntityConfiguration, CategoryConfiguration>();

            serviceCollection.RegisterStorage<DevFunStorage>();

            return serviceCollection;
        }
    }

    public static class ModuleCatalogExtensions
    {
        public static IModuleCatalogCollection AddDevFunStorageModule(this IModuleCatalogCollection moduleCatalog)
        {
            if (moduleCatalog is null)
            {
                throw new System.ArgumentNullException(nameof(moduleCatalog));
            }

            moduleCatalog.AddModule("DevFunStorageModule", new DevFunStorageModule());
            return moduleCatalog;
        }
    }
}