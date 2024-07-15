namespace _4tecture.PageObjects.Common
{
    public interface ISearchContext
    {
        Task<TControl> FindById<TControl>(string id, bool doWait) where TControl : class, IWebElement;
        Task<TControl> FindByName<TControl>(string name, bool doWait) where TControl : class, IWebElement;

        Task<IScreenshot> TakeScreenshot();

        Task<IEnumerable<TControl>> FindManyByName<TControl>(string name) where TControl : class, IWebElement;
        Task<TControl> FindByCssSelector<TControl>(string cssSelector, bool doWait) where TControl : class, IWebElement;
        Task<IEnumerable<TControl>> FindManyByCssSelector<TControl>(string cssSelector) where TControl : class, IWebElement;
        Task<TControl> FindByXPath<TControl>(string xPath, bool doWait) where TControl : class, IWebElement;
        Task<IEnumerable<TControl>> FindManyByXPath<TControl>(string xPath) where TControl : class, IWebElement;
    }
}
