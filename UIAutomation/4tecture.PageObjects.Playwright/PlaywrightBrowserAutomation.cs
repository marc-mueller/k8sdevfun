using _4tecture.PageObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace _4tecture.PageObjects.Playwright
{
    public class PlaywrightBrowserAutomation : PlaywrightWebElementAdapter, IBrowserAutomation
    {
        private readonly IPage _page;

        public PlaywrightBrowserAutomation(IPage page) : base(page)
        {
            _page = page;
        }

        public Task<string> GetUrl()
        {
            return Task.FromResult(_page.Url);
        }

        public Task<bool> SupportsJavaScriptExecution()
        {
            return Task.FromResult(true);
        }

        public Task<IJavaScriptExecutor> GetJavaScriptExecutor()
        {
            return Task.FromResult<IJavaScriptExecutor>(new JavaScriptExecutorAdapter(_page));
        }

        public async Task Quit()
        {
            await _page.Context.CloseAsync().ConfigureAwait(false);
            if (_page.Context.Browser != null)
            {
                await _page.Context.Browser.CloseAsync().ConfigureAwait(false);
            }
        }
    }
}
