using _4tecture.PageObjects.Common;

namespace DevFun.Web.PageObjects
{
    public class RandomJokePanelPageObject : WebPageObjectBase
    {
        protected RandomJokePanelPageObject(PageObjectBase parent) : base(parent)
        {
        }

        public RandomJokePanelPageObject(PageObjectBase parent, ISearchContext searchContext)
            : base(parent, searchContext)
        {
        }

        private IWebElement homeLink;
        private IWebElement aboutLink;

        private IWebElement jokeText;

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

        public async Task<IWebElement> GetElementJokeText()
        {
            
                jokeText ??= await FindById<IWebElement>("jokeText").ConfigureAwait(false);
                return jokeText;
            
        }


        public async Task<RandomJokePanelPageObject> GotoHome()
        {
            await (await GetElementHomeLink().ConfigureAwait(false)).Click().ConfigureAwait(false);
            await Task.Delay(100).ConfigureAwait(false);
            return new RandomJokePanelPageObject(Parent, SearchContext);
        }

        public async Task<AboutPanelPageObject> GotoAbout()
        {
            await (await GetElementAboutLink().ConfigureAwait(false)).Click().ConfigureAwait(false);
            await Task.Delay(100).ConfigureAwait(false);
            return new AboutPanelPageObject(Parent, Parent.SearchContext);
        }

        public async Task<string> GetJokeText()
        {
            try
            {
                return await (await GetElementJokeText().ConfigureAwait(false)).GetText().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<RandomJokePanelPageObject> VerifyAlternateUrl()
        {
            if (string.IsNullOrWhiteSpace(await GetJokeText().ConfigureAwait(false)))
            {
                IWebElement activateLink = await FindById<IWebElement>("activatealternateurl").ConfigureAwait(false);
                activateLink?.Click();
            }
            return new RandomJokePanelPageObject(Parent, SearchContext);
        }

    }
}