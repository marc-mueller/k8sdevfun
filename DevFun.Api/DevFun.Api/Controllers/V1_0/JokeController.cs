using System.Threading.Tasks;
using Asp.Versioning;
using DevFun.Common.Entities;
using DevFun.Common.Model.Dtos.V1_0;
using DevFun.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReaFx.ApiServices.Common.Controllers;
using ReaFx.DataAccess.Common.Mapper;
using Swashbuckle.AspNetCore.Annotations;

namespace DevFun.Api.Controllers.V1_0
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class JokeController : EntityCrudControllerBase<DevJoke, DevJokeDto, int, IDevJokeService>
    {
        public JokeController(IDevJokeService service, IMapperFactory mapperFactory, IOptions<JsonOptions> options, ILogger<JokeController> logger)
            : base(mapperFactory, service, options, logger)
        {
        }

        // GET api/jokes/random
        [HttpGet("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(nameof(GetRandom))]
        public async Task<ActionResult<DevJokeDto>> GetRandom()
        {
            return await MapToDto(await Service.GetRandomJoke().ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}