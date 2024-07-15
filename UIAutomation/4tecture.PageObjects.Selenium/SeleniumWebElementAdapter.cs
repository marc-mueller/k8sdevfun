using System.Drawing;
using _4tecture.PageObjects.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace _4tecture.PageObjects.Selenium;

public class SeleniumWebElementAdapter : Common.IWebElement
{

    protected internal SeleniumWebElementAdapter(OpenQA.Selenium.ISearchContext searchContext)
    {
        this.SearchContext = searchContext;
    }

    protected internal SeleniumWebElementAdapter(OpenQA.Selenium.IWebDriver webDriver)
    {
        this.WebDriver = webDriver;
        this.SearchContext = webDriver;
    }

    protected internal SeleniumWebElementAdapter(SeleniumWebElementAdapter parent)
    {
        this.Parent = parent;
        this.SearchContext = parent.SearchContext;
    }
        
    protected internal SeleniumWebElementAdapter(SeleniumWebElementAdapter parent, OpenQA.Selenium.IWebElement webElement)
    {
        this.Parent = parent;
        this.SearchContext = webElement;
        this.WebElement = webElement;
    }

    public OpenQA.Selenium.IWebElement WebElement { get; set; }

    public OpenQA.Selenium.ISearchContext SearchContext { get; set; }
    public SeleniumWebElementAdapter Parent { get; set; }
    public OpenQA.Selenium.IWebDriver WebDriver { get; set; }

    public Task<TControl> FindById<TControl>(string id, bool doWait) where TControl : class, Common.IWebElement
    {
        var by = By.Id(id);
        if (doWait)
        {
            var wait = new WebDriverWait(GetRootElementAdapter(this).WebDriver, TimeSpan.FromMilliseconds(10000));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
        }
        var element = SearchContext.FindElement(by);
        return Task.FromResult(SeleniumBrowserAutomationFactory.Create<TControl>(this, element));
    }

    public Task<TControl> FindByName<TControl>(string name, bool doWait) where TControl : class, Common.IWebElement
    {
        var by = By.Name(name);
        if (doWait)
        {
            var wait = new WebDriverWait(GetRootElementAdapter(this).WebDriver, TimeSpan.FromMilliseconds(10000));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
        }
        var element =  this.SearchContext.FindElement(by);
        return Task.FromResult(SeleniumBrowserAutomationFactory.Create<TControl>(this, element));
    }
        
    public Task<IEnumerable<TControl>> FindManyByName<TControl>(string name) where TControl : class, Common.IWebElement
    {
        var elements = this.SearchContext.FindElements(By.Name(name));
        return Task.FromResult(elements.Select(e => SeleniumBrowserAutomationFactory.Create<TControl>(this, e)));
    }

    public Task<TControl> FindByCssSelector<TControl>(string cssSelector, bool doWait) where TControl : class, Common.IWebElement
    {
        var by = By.CssSelector(cssSelector);
        if (doWait)
        {
            var wait = new WebDriverWait(GetRootElementAdapter(this).WebDriver, TimeSpan.FromMilliseconds(10000));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
        }
        var element = this.SearchContext.FindElement(by);
        return Task.FromResult(SeleniumBrowserAutomationFactory.Create<TControl>(this, element));
    }

    public Task<IEnumerable<TControl>> FindManyByCssSelector<TControl>(string cssSelector) where TControl : class, Common.IWebElement
    {
        var elements = this.SearchContext.FindElements(By.CssSelector(cssSelector));
        return Task.FromResult(elements.Select(e => SeleniumBrowserAutomationFactory.Create<TControl>(this, e)));
    }

    public Task<TControl> FindByXPath<TControl>(string xPath, bool doWait) where TControl : class, Common.IWebElement
    {
        var by = By.XPath(xPath);
        if (doWait)
        {
            var wait = new WebDriverWait(GetRootElementAdapter(this).WebDriver, TimeSpan.FromMilliseconds(10000));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
        }
        var element = this.SearchContext.FindElement(by);
        return Task.FromResult(SeleniumBrowserAutomationFactory.Create<TControl>(this, element));
    }

    public Task<IEnumerable<TControl>> FindManyByXPath<TControl>(string xPath) where TControl : class, Common.IWebElement
    {
        var elements = this.SearchContext.FindElements(By.XPath(xPath));
        return Task.FromResult(elements.Select(e => SeleniumBrowserAutomationFactory.Create<TControl>(this, e)));
    }

    public Task<IScreenshot> TakeScreenshot()
    {
        var screenshot = ((ITakesScreenshot)GetRootElementAdapter(this).WebDriver).GetScreenshot();
        return Task.FromResult<IScreenshot>(new _4tecture.PageObjects.Selenium.Screenshot(screenshot.AsByteArray));
    }
        
    private static SeleniumWebElementAdapter GetRootElementAdapter(SeleniumWebElementAdapter adapter)
    {
        if (adapter == null)
        {
            return null;
        }

        if (adapter.Parent == null)
        {
            return adapter;
        }

        return GetRootElementAdapter(adapter.Parent);
    }
    
    public Task<string> GetTagName()
    {
        return Task.FromResult(WebElement.TagName);
    }

    public Task<string> GetText()
    {
        return Task.FromResult(WebElement.Text);
    }

    public Task<bool> IsEnabled()
    {
        return Task.FromResult(WebElement.Enabled);
    }

    public Task<bool> IsSelected()
    {
        return Task.FromResult(WebElement.Selected);
    }

    public Task<Point> GetLocation()
    {
        return Task.FromResult(WebElement.Location);
    }

    public Task<Size> GetSize()
    {
        return Task.FromResult(WebElement.Size);
    }

    public Task<bool> IsDisplayed()
    {
        return Task.FromResult(WebElement.Displayed);
    }

    public Task Clear()
    {
        WebElement.Clear();
        return Task.CompletedTask;
    }

    public Task SendKeys(string text)
    {
        WebElement.SendKeys(text);
        return Task.CompletedTask;
    }

    public Task Submit()
    {
        WebElement.Submit();
        return Task.CompletedTask;
    }

    public Task Click()
    {
        WebElement.Click();
        return Task.CompletedTask;
    }

    public Task<string> GetAttribute(string attributeName)
    {
        return Task.FromResult(WebElement.GetAttribute(attributeName));
    }

    public Task<string> GetDomAttribute(string attributeName)
    {
        return Task.FromResult(WebElement.GetDomAttribute(attributeName));
    }

    public Task<string> GetDomProperty(string propertyName)
    {
        return Task.FromResult(WebElement.GetDomProperty(propertyName));
    }

    public Task<string> GetCssValue(string propertyName)
    {
        return Task.FromResult(WebElement.GetCssValue(propertyName));
    }

    public Task<Common.ISearchContext> GetShadowRoot()
    {
        //return SeleniumAdapterFactory.Create<IUiElement>(this, WebElement.GetShadowRoot());
        throw new NotImplementedException();
    }
}