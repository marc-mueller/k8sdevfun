using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace UiTestAutomationBase
{
    public abstract class BrowserPageObject : WebPageObjectBase
    {
        protected BrowserPageObject(ISearchContext parentSearchContext) : base(parentSearchContext)
        {
        }

        protected override IWebDriver Browser => this.SearchContext as IWebDriver;

        public void Close()
        {
            Browser.Quit();
        }

        protected static IWebDriver LaunchWebDriver(string url, TargetBrowser targetBrowser = TargetBrowser.InternetExplorer, bool remoteDriver = false, Uri remoteDriverUri = null, int width = 1024, int height = 768)
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
                            EnsureCleanSession = true
                        };
                        browser = new RemoteWebDriver(remoteDriverUri, ieOptions);
                        break;

                    case TargetBrowser.Edge:
                        var edgeOptions = new EdgeOptions();
                        edgeOptions.AddArguments("no-sandbox", "disable-dev-shm-usage"); // workaround for current docker issues
                        browser = new RemoteWebDriver(remoteDriverUri, edgeOptions);
                        break;

                    case TargetBrowser.Chrome:
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);
                        chromeOptions.AddArguments("no-sandbox", "disable-dev-shm-usage"); // workaround for current docker issues
                        browser = new RemoteWebDriver(remoteDriverUri, chromeOptions);
                        break;

                    case TargetBrowser.Firefox:
                        var firefoxOptions = new FirefoxOptions();
                        firefoxOptions.AcceptInsecureCertificates = true;
                        browser = new RemoteWebDriver(remoteDriverUri, firefoxOptions);
                        break;

                    case TargetBrowser.Safari:
                        var safariOptions= new SafariOptions();
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
                            EnsureCleanSession = true
                        };
                        browser = new InternetExplorerDriver(ieOptions);
                        break;

                    case TargetBrowser.Edge:
                        var edgeOptions = new EdgeOptions
                        {
                            PageLoadStrategy = PageLoadStrategy.Eager
                        };
                        browser = new EdgeDriver(edgeOptions);
                        break;

                    case TargetBrowser.Chrome:
                        browser = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                        break;

                    case TargetBrowser.Firefox:
                        browser = new FirefoxDriver();
                        break;

                    case TargetBrowser.Safari:
                        browser = new SafariDriver();
                        break;
                        
                    default:
                        throw new ArgumentException($"Unknown target browser type {targetBrowser}");
                }
            }

            browser.Manage().Window.Size = new Size(width, height);
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            browser.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            browser.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);

            browser.Navigate().GoToUrl(url);

            return browser;
        }
    }
}