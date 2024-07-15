using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReaFx.Services.LabelManagement.Common.Services;

namespace DevFun.Logic.Services
{
    internal class LabelSubsetPathProvider : ILabelSubsetPathProvider
    {
        public Task<IEnumerable<string>> BuildSubsetPaths(string route)
        {
            var result = new[] { "tenant/123/", "subscription/456", "/clientapp/789" };

            return Task.FromResult<IEnumerable<string>>(result);
        }
    }
}
