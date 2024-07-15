using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Common
{
    public class WebPageObjectBase : PageObjectBase
    {
        protected WebPageObjectBase(ISearchContext searchContext) : base(searchContext)
        {
        }

        protected WebPageObjectBase(PageObjectBase parent) : base(parent)
        {

        }

        protected WebPageObjectBase(PageObjectBase parent, ISearchContext searchContext) : base(parent, searchContext)
        {
        }

        public Task<string> GetCurrenUrl(){
            return RootPageObject?.Browser.GetUrl();
        }

    
        protected BrowserPageObject RootPageObject => GetBrowserPageObject(this);
        protected virtual IBrowserAutomation Browser => RootPageObject.Browser;
        protected Task<IJavaScriptExecutor> GetJsExecutor()
        {
            return Browser.GetJavaScriptExecutor();
        }

        #region public methods
        public override Task<TControl> FindById<TControl>(string id, bool doWait = true)
        {
            return this.SearchContext.FindById<TControl>(id, doWait);
        }

        public Task<TControl> FindByName<TControl>(string name, bool doWait = true) where TControl : class, IWebElement
        {
            return this.SearchContext.FindByName<TControl>(name, doWait);
        }

        public Task<IEnumerable<TControl>> FindManyByName<TControl>(string name) where TControl : class, IWebElement
        {
            return this.SearchContext.FindManyByName<TControl>(name);
        }

        public Task<TControl> FindByCssSelector<TControl>(string cssSelector, bool doWait = true) where TControl : class, IWebElement
        {
            return this.SearchContext.FindByCssSelector<TControl>(cssSelector, doWait);
        }
        

        public Task<IEnumerable<TControl>> FindManyByCssSelector<TControl>(string cssSelector) where TControl : class, IWebElement
        {
            return this.SearchContext.FindManyByCssSelector<TControl>(cssSelector);
        }

        public Task<TControl> FindByXPath<TControl>(string xPath, bool doWait = true) where TControl : class, IWebElement
        {
            return this.SearchContext.FindByXPath<TControl>(xPath, doWait);
        }

        public Task<IEnumerable<TControl>> FindManyByXPath<TControl>(string xPath) where TControl : class, IWebElement
        {
            return this.SearchContext.FindManyByXPath<TControl>(xPath);
        }
        
        #endregion

        #region private methods

        private BrowserPageObject GetBrowserPageObject(PageObjectBase pageObject)
        {
            if (pageObject == null)
            {
                return null;
            }

            var browserPageObject = pageObject as BrowserPageObject;
            if (browserPageObject != null)
            {
                return browserPageObject;
            }
            else
            {
                return GetBrowserPageObject(pageObject.Parent);
            }
        }

        #endregion

    }
}
