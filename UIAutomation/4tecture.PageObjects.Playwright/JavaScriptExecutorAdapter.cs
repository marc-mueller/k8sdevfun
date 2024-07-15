using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4tecture.PageObjects.Common;
using Microsoft.Playwright;

namespace _4tecture.PageObjects.Playwright
{
    internal class JavaScriptExecutorAdapter : IJavaScriptExecutor
    {
        private readonly IPage _page;

        public JavaScriptExecutorAdapter(IPage page)
        {
            _page = page;
        }

        public async Task<object> ExecuteScript(string script, params object[] args)
        {
            return await _page.EvaluateAsync(script, args).ConfigureAwait(false);
        }

        public async Task<object> ExecuteAsyncScript(string script, params object[] args)
        {
            return await _page.EvaluateAsync(script, args).ConfigureAwait(false);
        }
    }
}
