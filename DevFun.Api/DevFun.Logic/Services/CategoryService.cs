using DevFun.Common.Entities;
using DevFun.Common.Repositories;
using DevFun.Common.Services;
using DevFun.Common.Storages;
using Microsoft.Extensions.Logging;
using ReaFx.DataAccess.Common.Services;
using ReaFx.DataAccess.Common.Storages;

namespace DevFun.Logic.Services
{
    public class CategoryService : EntityServiceBase<Category, int, ICategoryRepository, IDevFunStorage>, ICategoryService
    {
        public CategoryService(IStorageFactory<IDevFunStorage> storageFactory, ILogger<CategoryService> logger)
            : base(storageFactory, logger)
        {
        }
    }
}