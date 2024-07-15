using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Common
{
    public abstract class BrowserPageObject : WebPageObjectBase
    {
        protected BrowserPageObject(IBrowserAutomation parentSearchContext) : base(parentSearchContext)
        {
        }

        protected override IBrowserAutomation Browser => this.SearchContext as IBrowserAutomation;

        public Task Close()
        {
            return Browser.Quit();
        }

    }
}
