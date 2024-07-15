using Asp.Versioning;
using DevFun.Common.Entities;
using DevFun.Common.Model.Dtos.V1_0;
using DevFun.Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReaFx.ApiServices.Common.Controllers;
using ReaFx.DataAccess.Common.Mapper;

namespace DevFun.Api.Controllers.V1_0
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : EntityCrudControllerBase<Category, CategoryDto, int, ICategoryService>
    {
        public CategoryController(ICategoryService service, IMapperFactory mapperFactory, IOptions<JsonOptions> options, ILogger<CategoryController> logger)
            : base(mapperFactory, service, options, logger)
        {
        }
    }
}