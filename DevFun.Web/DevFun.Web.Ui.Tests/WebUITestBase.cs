using System;
using _4tecture.PageObjects.Common;
using _4tecture.PageObjects.Playwright;
using _4tecture.PageObjects.Selenium;
using DevFun.Web.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DevFun.Web.Ui.Tests
{
    public class WebUITestBase
    {
        private IBrowserAutomationFactory _automationDriverFactory;

        public TestContext TestContext { get; set; }

        public string BaseUrl { get; private set; }

        public DevFunAppPageObject CurrentApplication { get; private set; }

        [TestCleanup]
        public Task TeardownTest()
        {
            return CloseCurrentApplication();
        }

        public async Task<DevFunAppPageObject> Launch(string url = null, TargetBrowser targetBrowser = TargetBrowser.Undefined, bool remoteDriver = false, Uri remoteDriverUri = null, int width = 0, int height = 0)
        {
            if (url == null)
            {
                url = TestContext?.Properties["baseUrl"]?.ToString();
                if (url == null)
                {
                    throw new ArgumentNullException("Invalid base url parameter detected in the test context");
                }
            }

            if (targetBrowser == TargetBrowser.Undefined)
            {
                if (!Enum.TryParse(TestContext?.Properties["targetBrowser"]?.ToString(), true, out targetBrowser))
                {
                    throw new ArgumentNullException("Invalid target browser parameter detected in the test context");
                }
            }

            if (!string.IsNullOrWhiteSpace(TestContext?.Properties["driverType"]?.ToString()))
            {
                if (TestContext.Properties["driverType"].ToString().Equals("Remote", StringComparison.OrdinalIgnoreCase))
                {
                    remoteDriver = true;
                }
                else
                {
                    remoteDriver = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(TestContext?.Properties["remoteDriverUrl"]?.ToString()))
            {
                remoteDriverUri = new Uri(TestContext?.Properties["remoteDriverUrl"]?.ToString());
            }

            if (width <= 0 || height <= 0)
            {
                if (!int.TryParse(TestContext?.Properties["resolutionWidth"].ToString(), out width))
                {
                    throw new ArgumentNullException("Invalid resolution width parameter detected in the test context");
                }

                if (!int.TryParse(TestContext?.Properties["resolutionHeight"].ToString(), out height))
                {
                    throw new ArgumentNullException("Invalid resolution height parameter detected in the test context");
                }
            }

            if (CurrentApplication != null)
            {
                await CloseCurrentApplication().ConfigureAwait(false);
            }

            this.BaseUrl = url;

            if (_automationDriverFactory == null)
            {
                // Determine the automation framework to use (Selenium or Playwright)
                var automationFramework = TestContext?.Properties["automationFramework"]?.ToString();
                if (automationFramework != null &&
                    automationFramework.Equals("playwright", StringComparison.InvariantCultureIgnoreCase))
                {
                    _automationDriverFactory = new PlaywrightBrowserAutomationFactory();
                }
                else if (automationFramework != null &&
                         automationFramework.Equals("selenium", StringComparison.InvariantCultureIgnoreCase))
                {
                    _automationDriverFactory = new SeleniumBrowserAutomationFactory();
                }
                else
                {
                    throw new ArgumentException($"Automation framework \"{automationFramework}\" is not supported. Currently only Selenium and Playwright are supported");
                }
            }

            // Pass the test name to the factory
            var testName = $"{TestContext.TestName}_{targetBrowser.ToString()}";

            this.CurrentApplication = await _automationDriverFactory.LaunchBrowserForTesting<DevFunAppPageObject>(
                url, targetBrowser, remoteDriver, remoteDriverUri, width, height, testName).ConfigureAwait(false);
            return CurrentApplication;
        }

        public Task SaveScreenshot(string name = null)
        {
            TestContext.AddResultFile(name);
            return Task.CompletedTask;
        }

        private async Task CloseCurrentApplication()
        {
            if (this.CurrentApplication != null)
            {
                await this.CurrentApplication.Close().ConfigureAwait(false);
            }
            this.CurrentApplication = null;
        }
    }
}
