using _4tecture.PageObjects.Common;

namespace DevFun.Web.PageObjects
{
    public class DevFunAppPageObject : BrowserPageObject
    {
        protected Uri BaseUri { get; private set; }

        public DevFunAppPageObject(IBrowserAutomation parentSearchContext) : base(parentSearchContext)
        {
        }

        public Task<RandomJokePanelPageObject> NavigateToRandomJokes()
        {
            return Task.FromResult(new RandomJokePanelPageObject(this, this.SearchContext));
        }
        
    }
}