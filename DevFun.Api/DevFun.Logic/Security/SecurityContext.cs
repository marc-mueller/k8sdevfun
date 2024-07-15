using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReaFx.DataAccess.Common.Security;

namespace DevFun.Logic.Security
{
    public class SecurityContext : SecurityContextBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private string subject;

        public SecurityContext(ILogger<SecurityContext> logger, IHttpContextAccessor httpContextAccessor)
            : base(logger)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public override string UserId
        {
            get
            {
                if (subject == null)
                {
                    string[] subjectClaims = GetClaimValues("sub");
                    subject = subjectClaims?.SingleOrDefault() ?? Guid.Empty.ToString();
                }
                return subject;
            }
        }

        private string[] GetClaimValues(string claimType)
        {
            List<string> claimValues = new();

            IEnumerable<string> values = httpContextAccessor.HttpContext?.User?.Claims.Where(c => c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase)).Select(i => i.Value);
            if (values != null && values.Any())
            {
                claimValues.AddRange(values.Where(v => v != null));
            }

            return claimValues.ToArray();
        }
    }
}
