using _4tecture.PageObjects.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using System.Xml.Linq;

namespace _4tecture.PageObjects.Playwright
{
    public class PlaywrightWebElementAdapter : IWebElement
    {
        protected readonly IElementHandle _elementHandle;
        protected readonly IPage _page;
        private readonly PlaywrightWebElementAdapter _parent;

        public PlaywrightWebElementAdapter(IElementHandle elementHandle)
        {
            _elementHandle = elementHandle;
        }

        public PlaywrightWebElementAdapter(PlaywrightWebElementAdapter parent, IElementHandle elementHandle)
        {
            _parent = parent;
            _elementHandle = elementHandle;
        }

        public PlaywrightWebElementAdapter(IPage page)
        {
            _page = page;
        }

        public Task<TControl> FindById<TControl>(string id, bool doWait) where TControl : class, IWebElement
        {
            return FindElement<TControl>($"#{id}", doWait);
        }

        public Task<TControl> FindByName<TControl>(string name, bool doWait) where TControl : class, IWebElement
        {
            return FindElement<TControl>($"[name='{name}']", doWait);
        }

        public async Task<IScreenshot> TakeScreenshot()
        {
            if (_elementHandle != null)
            {
                var screenshot = await _elementHandle.ScreenshotAsync().ConfigureAwait(false);
                return new Screenshot(screenshot);
            }
            else if (_page != null)
            {
                var screenshot = await _page.ScreenshotAsync().ConfigureAwait(false);
                return new Screenshot(screenshot);
            }
            throw new InvalidOperationException("No valid page or element handle to take a screenshot.");
        }

        public Task<IEnumerable<TControl>> FindManyByName<TControl>(string name) where TControl : class, IWebElement
        {
            return FindElements<TControl>($"[name='{name}']");
        }

        public Task<TControl> FindByCssSelector<TControl>(string cssSelector, bool doWait) where TControl : class, IWebElement
        {
            return FindElement<TControl>(cssSelector, doWait);
        }

        public Task<IEnumerable<TControl>> FindManyByCssSelector<TControl>(string cssSelector) where TControl : class, IWebElement
        {
            return FindElements<TControl>(cssSelector);
        }

        public Task<TControl> FindByXPath<TControl>(string xPath, bool doWait) where TControl : class, IWebElement
        {
            return FindElement<TControl>($"xpath={xPath}", doWait);
        }

        public Task<IEnumerable<TControl>> FindManyByXPath<TControl>(string xPath) where TControl : class, IWebElement
        {
            return FindElements<TControl>($"xpath={xPath}");
        }

        public async Task<string> GetTagName()
        {
            return await ExecuteOnElementHandleOrPage<string>("e => e.tagName").ConfigureAwait(false);
        }

        public async Task<string> GetText()
        {
            return await ExecuteOnElementHandleOrPage<string>("e => e.innerText").ConfigureAwait(false);
        }

        public async Task<bool> IsEnabled()
        {
            return await _elementHandle.IsEnabledAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsSelected()
        {
            return await _elementHandle.IsCheckedAsync().ConfigureAwait(false);
        }

        public async Task<Point> GetLocation()
        {
            var box = await _elementHandle.BoundingBoxAsync().ConfigureAwait(false);
            return new Point((int)box.X, (int)box.Y);
        }

        public async Task<Size> GetSize()
        {
            var box = await _elementHandle.BoundingBoxAsync().ConfigureAwait(false);
            return new Size((int)box.Width, (int)box.Height);
        }

        public async Task<bool> IsDisplayed()
        {
            return await _elementHandle.IsVisibleAsync().ConfigureAwait(false);
        }

        public Task Clear()
        {
            return _elementHandle.FillAsync(string.Empty);
        }

        public Task SendKeys(string text)
        {
            return _elementHandle.TypeAsync(text);
        }

        public Task Submit()
        {
            return _elementHandle.PressAsync("Enter");
        }

        public Task Click()
        {
            return _elementHandle.ClickAsync();
        }

        public async Task<string> GetAttribute(string attributeName)
        {
            return await _elementHandle.GetAttributeAsync(attributeName).ConfigureAwait(false);
        }

        public async Task<string> GetDomAttribute(string attributeName)
        {
            return await _elementHandle.GetAttributeAsync(attributeName).ConfigureAwait(false);
        }

        public async Task<string> GetDomProperty(string propertyName)
        {
            return await ExecuteOnElementHandleOrPage<string>($"e => e.{propertyName}").ConfigureAwait(false);
        }

        public async Task<string> GetCssValue(string propertyName)
        {
            return await ExecuteOnElementHandleOrPage<string>($"(e) => getComputedStyle(e).getPropertyValue('{propertyName}')").ConfigureAwait(false);
        }

        public async Task<ISearchContext> GetShadowRoot()
        {
            var shadowRoot = await _elementHandle.EvaluateHandleAsync("e => e.shadowRoot").ConfigureAwait(false);
            return new PlaywrightWebElementAdapter(shadowRoot.AsElement());
        }

        private async Task<TControl> FindElement<TControl>(string selector, bool doWait) where TControl : class, IWebElement
        {
            IElementHandle element = null;
            if (_elementHandle != null)
            {
                if (doWait)
                {
                    element = await _elementHandle.WaitForSelectorAsync(selector).ConfigureAwait(false);
                }
                else
                {
                    element = await _elementHandle.QuerySelectorAsync(selector).ConfigureAwait(false);
                }
            }
            else if (_page != null)
            {
                if (doWait)
                {
                    element = await _page.WaitForSelectorAsync(selector).ConfigureAwait(false);
                }
                else
                {
                    element = await _page.QuerySelectorAsync(selector).ConfigureAwait(false);
                }
            }

            return PlaywrightBrowserAutomationFactory.Create<TControl>(this, element);
        }

        private async Task<IEnumerable<TControl>> FindElements<TControl>(string selector) where TControl : class, IWebElement
        {
            IEnumerable<IElementHandle> elements = null;
            if (_elementHandle != null)
            {
                elements = await _elementHandle.QuerySelectorAllAsync(selector).ConfigureAwait(false);
            }
            else if (_page != null)
            {
                elements = await _page.QuerySelectorAllAsync(selector).ConfigureAwait(false);
            }

            return elements.Select(e => PlaywrightBrowserAutomationFactory.Create<TControl>(this, e));
        }

        private async Task<TResult> ExecuteOnElementHandleOrPage<TResult>(string script)
        {
            if (_elementHandle != null)
            {
                return await _elementHandle.EvaluateAsync<TResult>(script).ConfigureAwait(false);
            }
            else if (_page != null)
            {
                return await _page.EvaluateAsync<TResult>(script).ConfigureAwait(false);
            }
            throw new InvalidOperationException("No valid page or element handle to execute the script.");
        }
    }
}
