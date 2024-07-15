using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReaFx.ApiClients.Common.Settings;
using ReaFx.ApiClients.Common.Tokens;

namespace DevFun.Web.Security
{
    public class CustomTokenHandler : TokenHandlerBase
    {
        public CustomTokenHandler(
            ILogger<CustomTokenHandler> logger,
            IServiceEndpointSettings serviceEndpointSettings)
            : base(logger, serviceEndpointSettings)
        {

        }

        public override Task<JwtSecurityToken> GetTokenForUser<TService>(Uri baseUri)
        {
            return Task.FromResult(new JwtSecurityToken());
        }

        public override Task<JwtSecurityToken> GetTokenForService<TService>(Uri baseUri)
        {
            return Task.FromResult(new JwtSecurityToken());
        }
    }
}
