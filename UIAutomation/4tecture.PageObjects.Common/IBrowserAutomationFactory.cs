using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Common
{
    public interface IBrowserAutomationFactory
    {
        Task<IBrowserAutomation> LaunchBrowserForTesting(string url, TargetBrowser targetBrowser = TargetBrowser.InternetExplorer, bool remoteDriver = false, Uri remoteDriverUri = null, int width = 1024, int height = 768, string testName = null);
        Task<TBrowserPageObject> LaunchBrowserForTesting<TBrowserPageObject>(string url, TargetBrowser targetBrowser = TargetBrowser.InternetExplorer, bool remoteDriver = false, Uri remoteDriverUri = null, int width = 1024, int height = 768, string testName = null) where TBrowserPageObject : BrowserPageObject;
    }
}
