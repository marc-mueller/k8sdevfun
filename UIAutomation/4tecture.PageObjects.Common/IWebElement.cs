using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Common
{
    public interface IWebElement : ISearchContext
    {
        Task<string> GetTagName();
        Task<string> GetText();
        Task<bool> IsEnabled();
        Task<bool> IsSelected();
        Task<Point> GetLocation();
        Task<Size> GetSize();
        Task<bool> IsDisplayed();
        Task Clear();
        Task SendKeys(string text);
        Task Submit();
        Task Click();
        Task<string> GetAttribute(string attributeName);
        Task<string> GetDomAttribute(string attributeName);
        Task<string> GetDomProperty(string propertyName);
        Task<string> GetCssValue(string propertyName);
        Task<ISearchContext> GetShadowRoot();
    }
}
