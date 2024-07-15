using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Common
{
    public class PageObjectBase
    {
        protected PageObjectBase(ISearchContext searchContext)
        {
            this.SearchContext = searchContext;
        }

        protected PageObjectBase(PageObjectBase parent)
        {
            this.Parent = parent;
            this.SearchContext = parent.SearchContext;
        }

        protected PageObjectBase(PageObjectBase parent, ISearchContext searchContext)
        {
            this.Parent = parent;
            this.SearchContext = searchContext;
        }

        public PageObjectBase Parent { get; }
        public ISearchContext SearchContext { get; set; }

        public virtual Task<TControl> FindById<TControl>(string id, bool doWait = true) where TControl : class, IWebElement
        {
            return this.SearchContext.FindById<TControl>(id, doWait);
        }

        public Task<IScreenshot> TakeScreenshot()
        {
            return this.SearchContext.TakeScreenshot();
        }
    }
}
