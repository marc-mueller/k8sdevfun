using _4tecture.PageObjects.Common;
using OpenQA.Selenium;
using IJavaScriptExecutor = _4tecture.PageObjects.Common.IJavaScriptExecutor;

namespace _4tecture.PageObjects.Selenium
{
    public class SeleniumBrowserAutomation : SeleniumWebElementAdapter, IBrowserAutomation
    {
        private readonly IWebDriver _webDriver;
        private readonly JavaScriptExecutorAdapter _javascriptExecutor;

        protected internal SeleniumBrowserAutomation(OpenQA.Selenium.IWebDriver webDriver) : base(webDriver)
        {
            _webDriver = webDriver;
            _javascriptExecutor = new JavaScriptExecutorAdapter((OpenQA.Selenium.IJavaScriptExecutor)webDriver);
        }


        public Task<string> GetUrl()
        {
            return Task.FromResult(_webDriver.Url);
        } 
        public Task<bool> SupportsJavaScriptExecution()
        {
            return Task.FromResult(true);
        }

        public Task<IJavaScriptExecutor> GetJavaScriptExecutor()
        {
            return Task.FromResult<Common.IJavaScriptExecutor>(_javascriptExecutor);
        }

        public Task Quit()
        {
            _webDriver.Quit();
            return Task.CompletedTask;
        }
    }
}
