using System;
using System.Reflection;
using DevFun.Common.Model.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReaFx.ApiServices.Common.Controllers;

namespace DevFun.Api.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StatusController : FrameworkControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public StatusController(IWebHostEnvironment environment, ILogger<StatusController> logger)
            : base(logger)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<StatusResponseDto> GetStatus()
        {
            StatusResponseDto status = new()
            {
                AssemblyInfoVersion = GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
                AssemblyVersion = GetType().Assembly.GetName().Version.ToString(),
                AssemblyFileVersion = GetType().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,

                MachineName = Environment.MachineName,
                EnvironmentName = environment?.EnvironmentName ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            };

            return Ok(status);
        }
    }
}