using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Common
{
    public interface IBrowserAutomation : ISearchContext
    {
        public Task<string> GetUrl();
        public Task<bool> SupportsJavaScriptExecution();
        public Task<IJavaScriptExecutor> GetJavaScriptExecutor();
        public Task Quit();
    }
}
