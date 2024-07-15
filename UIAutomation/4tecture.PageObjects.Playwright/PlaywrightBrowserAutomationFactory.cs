using _4tecture.PageObjects.Common;
using Microsoft.Playwright;

namespace _4tecture.PageObjects.Playwright
{
    public class PlaywrightBrowserAutomationFactory : IBrowserAutomationFactory
    {
        public static TControl Create<TControl>(PlaywrightWebElementAdapter parent, IElementHandle elementHandle) where TControl : class, IWebElement
        {
            if (typeof(TControl) == typeof(IWebElement))
            {
                return new PlaywrightWebElementAdapter(parent, elementHandle) as TControl;
            }

            throw new NotSupportedException();
        }

        public async Task<IBrowserAutomation> LaunchBrowserForTesting(string url, TargetBrowser targetBrowser = TargetBrowser.InternetExplorer,
        bool remoteDriver = false, Uri remoteDriverUri = null, int width = 1024, int height = 768, string testName = null)
        {
            IPlaywright playwright = await Microsoft.Playwright.Playwright.CreateAsync().ConfigureAwait(false);
            IBrowser browser = null;

            if (remoteDriver)
            {
                switch (targetBrowser)
                {
                    case TargetBrowser.Chrome:
                        browser = await playwright.Chromium.ConnectAsync(remoteDriverUri.ToString(), new BrowserTypeConnectOptions()).ConfigureAwait(false);
                        break;
                    case TargetBrowser.Firefox:
                        browser = await playwright.Firefox.ConnectAsync(remoteDriverUri.ToString(), new BrowserTypeConnectOptions()).ConfigureAwait(false);
                        break;
                    case TargetBrowser.Edge:
                        browser = await playwright.Webkit.ConnectAsync(remoteDriverUri.ToString(), new BrowserTypeConnectOptions()).ConfigureAwait(false);
                        break;
                    default:
                        throw new ArgumentException($"Unknown target browser type {targetBrowser}");
                }
            }
            else
            {
                switch (targetBrowser)
                {
                    case TargetBrowser.Chrome:
                        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }).ConfigureAwait(false);
                        break;
                    case TargetBrowser.Firefox:
                        browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }).ConfigureAwait(false);
                        break;
                    case TargetBrowser.Edge:
                        browser = await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }).ConfigureAwait(false);
                        break;
                    default:
                        throw new ArgumentException($"Unknown target browser type {targetBrowser}");
                }
            }

            IBrowserContext context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = width, Height = height },
                IgnoreHTTPSErrors = true,
                RecordVideoDir = "/videos-tmp",
                RecordVideoSize = new RecordVideoSize() { Width = width, Height = height }
            }).ConfigureAwait(false);
            IPage page = await context.NewPageAsync().ConfigureAwait(false);
            await page.GotoAsync(url).ConfigureAwait(false);

            return new PlaywrightBrowserAutomation(page);
        }

        public async Task<TBrowserPageObject> LaunchBrowserForTesting<TBrowserPageObject>(string url, TargetBrowser targetBrowser = TargetBrowser.InternetExplorer,
            bool remoteDriver = false, Uri remoteDriverUri = null, int width = 1024, int height = 768, string testName = null) where TBrowserPageObject : BrowserPageObject
        {
            IBrowserAutomation driver = await LaunchBrowserForTesting(url, targetBrowser, remoteDriver, remoteDriverUri, width, height).ConfigureAwait(false);
            TBrowserPageObject? browserPageObject = Activator.CreateInstance(typeof(TBrowserPageObject), driver) as TBrowserPageObject;

            if (browserPageObject == null)
            {
                throw new InvalidOperationException($"Browser page object cannot be casted to {typeof(TBrowserPageObject).Name}");
            }

            return browserPageObject;
        }
    }
}
