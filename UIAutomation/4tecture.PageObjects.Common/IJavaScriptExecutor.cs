using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Common
{
    public interface IJavaScriptExecutor
    {
        Task<object> ExecuteScript(string script, params object[] args);
        //object ExecuteScript(PinnedScript script, params object[] args);
        Task<object> ExecuteAsyncScript(string script, params object[] args);
    }
}
