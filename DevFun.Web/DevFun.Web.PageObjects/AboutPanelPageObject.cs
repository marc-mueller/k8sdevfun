using _4tecture.PageObjects.Common;

namespace DevFun.Web.PageObjects
{
    public class AboutPanelPageObject : WebPageObjectBase
    {
        protected AboutPanelPageObject(PageObjectBase parent) : base(parent)
        {
        }

        public AboutPanelPageObject(PageObjectBase parent, ISearchContext searchContext)
            : base(parent, searchContext)
        {
        }

        private IWebElement homeLink;
        private IWebElement aboutLink;

        private IWebElement deploymentEnvironment;
        private IWebElement machineName;

        public async Task<IWebElement> GetElementHomeLink()
        {
            
                homeLink ??= await FindById<IWebElement>("homeLink").ConfigureAwait(false);
                return homeLink;
            
        }

        public async Task<IWebElement> GetElementAboutLink()
        {
            
                aboutLink ??= await FindById<IWebElement>("aboutLink").ConfigureAwait(false);
                return aboutLink;
            
        }



        public async Task<IWebElement> GetElementDeploymentEnvironment()
        {
            
                deploymentEnvironment ??= await FindById<IWebElement>("deploymentEnvironment").ConfigureAwait(false);
                return deploymentEnvironment;
            
        }

        public async Task<IWebElement> GetElementMachineName()
        {
            
                machineName ??= await FindById<IWebElement>("machineName").ConfigureAwait(false);
                return machineName;
            
        }

        public async Task<RandomJokePanelPageObject> GotoHome()
        {
            await (await GetElementHomeLink().ConfigureAwait(false)).Click().ConfigureAwait(false);
            await Task.Delay(100).ConfigureAwait(false);
            return new RandomJokePanelPageObject(Parent, Parent.SearchContext);
        }

        public async Task<AboutPanelPageObject> GotoAbout()
        {
            await (await GetElementAboutLink().ConfigureAwait(false)).Click().ConfigureAwait(false);
            await Task.Delay(100).ConfigureAwait(false);
            return new AboutPanelPageObject(Parent, SearchContext);
        }

        public async Task<string> GetAboutText()
        {
            string a1 = await (await GetElementDeploymentEnvironment().ConfigureAwait(false)).GetText().ConfigureAwait(false);
            string a2 = await (await GetElementMachineName().ConfigureAwait(false)).GetText().ConfigureAwait(false);
            return $"{a1} {a2}";
        }
    }
}