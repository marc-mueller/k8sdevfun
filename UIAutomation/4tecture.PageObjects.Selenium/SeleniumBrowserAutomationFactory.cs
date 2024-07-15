using System.Reflection;
using _4tecture.PageObjects.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Selenium
{
    public class SeleniumBrowserAutomationFactory : IBrowserAutomationFactory
    {
        public static TControl Create<TControl>(SeleniumWebElementAdapter parent, OpenQA.Selenium.IWebElement seleniumWebElement) where TControl : class, Common.IWebElement
        {
            if (typeof(TControl) == typeof(Common.IWebElement))
            {
                return new SeleniumWebElementAdapter(parent, seleniumWebElement) as TControl;
            }

            throw new NotSupportedException();
        }

        public async Task<IBrowserAutomation> LaunchBrowserForTesting(string url, TargetBrowser targetBrowser = TargetBrowser.InternetExplorer, bool remoteDriver = false, Uri remoteDriverUri = null, int width = 1024, int height = 768, string testName = null)
        {
            IWebDriver browser = null;

            if (remoteDriver)
            {
                switch (targetBrowser)
                {
                    case TargetBrowser.InternetExplorer:
                        var ieOptions = new InternetExplorerOptions
                        {
                            IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                            EnsureCleanSession = true,
                            AcceptInsecureCertificates = true
                        };
                        if (!string.IsNullOrEmpty(testName))
                        {
                            ieOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new RemoteWebDriver(remoteDriverUri, ieOptions);
                        break;

                    case TargetBrowser.Edge:
                        var edgeOptions = new EdgeOptions();
                        edgeOptions.AddArguments("no-sandbox", "disable-dev-shm-usage"); // workaround for current docker issues
                        edgeOptions.AcceptInsecureCertificates = true;
                        if (!string.IsNullOrEmpty(testName))
                        {
                            edgeOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new RemoteWebDriver(remoteDriverUri, edgeOptions);
                        break;

                    case TargetBrowser.Chrome:
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);
                        chromeOptions.AddArguments("no-sandbox", "disable-dev-shm-usage"); // workaround for current docker issues
                        chromeOptions.AcceptInsecureCertificates = true;
                        if (!string.IsNullOrEmpty(testName))
                        {
                            chromeOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new RemoteWebDriver(remoteDriverUri, chromeOptions);
                        break;

                    case TargetBrowser.Firefox:
                        var firefoxOptions = new FirefoxOptions();
                        firefoxOptions.AcceptInsecureCertificates = true;
                        if (!string.IsNullOrEmpty(testName))
                        {
                            firefoxOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new RemoteWebDriver(remoteDriverUri, firefoxOptions);
                        break;

                    case TargetBrowser.Safari:
                        var safariOptions = new SafariOptions();
                        safariOptions.AcceptInsecureCertificates = true;
                        if (!string.IsNullOrEmpty(testName))
                        {
                            safariOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new RemoteWebDriver(remoteDriverUri, safariOptions);
                        break;

                    default:
                        throw new ArgumentException($"Unknown target browser type {targetBrowser}");
                }
            }
            else
            {
                switch (targetBrowser)
                {
                    case TargetBrowser.InternetExplorer:
                        var ieOptions = new InternetExplorerOptions
                        {
                            IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                            EnsureCleanSession = true,
                            AcceptInsecureCertificates = true
                        };
                        if (!string.IsNullOrEmpty(testName))
                        {
                            ieOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new InternetExplorerDriver(ieOptions);
                        break;

                    case TargetBrowser.Edge:
                        var edgeOptions = new EdgeOptions
                        {
                            PageLoadStrategy = PageLoadStrategy.Eager,
                            AcceptInsecureCertificates = true
                        };
                        if (!string.IsNullOrEmpty(testName))
                        {
                            edgeOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new EdgeDriver(edgeOptions);
                        break;

                    case TargetBrowser.Chrome:
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AcceptInsecureCertificates = true;
                        if (!string.IsNullOrEmpty(testName))
                        {
                            chromeOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions);
                        break;

                    case TargetBrowser.Firefox:
                        var firefoxOptions = new FirefoxOptions();
                        firefoxOptions.AcceptInsecureCertificates = true;
                        if (!string.IsNullOrEmpty(testName))
                        {
                            firefoxOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new FirefoxDriver(firefoxOptions);
                        break;

                    case TargetBrowser.Safari:
                        var safariOptions = new SafariOptions();
                        safariOptions.AcceptInsecureCertificates = true;
                        if (!string.IsNullOrEmpty(testName))
                        {
                            safariOptions.AddAdditionalOption("se:name", testName);
                        }
                        browser = new SafariDriver(safariOptions);
                        break;

                    default:
                        throw new ArgumentException($"Unknown target browser type {targetBrowser}");
                }
            }

            browser.Manage().Window.Size = new System.Drawing.Size(width, height);
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            browser.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            browser.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);

            await browser.Navigate().GoToUrlAsync(new Uri(url)).ConfigureAwait(false);

            return new SeleniumBrowserAutomation(browser);
        }

        public async Task<TBrowserPageObject> LaunchBrowserForTesting<TBrowserPageObject>(string url,
            TargetBrowser targetBrowser = TargetBrowser.InternetExplorer, bool remoteDriver = false, Uri remoteDriverUri = null,
            int width = 1024, int height = 768, string testName = null) where TBrowserPageObject : BrowserPageObject
        {
            try
            {
                var browser = await LaunchBrowserForTesting(url, targetBrowser, remoteDriver, remoteDriverUri, width, height, testName).ConfigureAwait(false);

                var browserPageObject = Activator.CreateInstance(typeof(TBrowserPageObject), browser) as TBrowserPageObject;
                if (browserPageObject == null)
                {
                    throw new InvalidOperationException($"Browser page object cannot be casted to {typeof(TBrowserPageObject).Name}");
                }

                return browserPageObject;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not create instance of {typeof(TBrowserPageObject).Name}", ex);
            }
        }
    }
}
