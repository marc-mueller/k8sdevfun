using _4tecture.PageObjects.Common;

namespace _4tecture.PageObjects.Selenium;

public class JavaScriptExecutorAdapter : IJavaScriptExecutor
{
    private readonly OpenQA.Selenium.IJavaScriptExecutor _seleniumJavaScriptExecutor;

    public JavaScriptExecutorAdapter(OpenQA.Selenium.IJavaScriptExecutor seleniumJavaScriptExecutor)
    {
        _seleniumJavaScriptExecutor = seleniumJavaScriptExecutor;
    }
    public Task<object> ExecuteScript(string script, params object[] args)
    {
        return Task.FromResult(_seleniumJavaScriptExecutor.ExecuteScript(script, args));
    }

    public Task<object> ExecuteAsyncScript(string script, params object[] args)
    {
        return Task.FromResult(_seleniumJavaScriptExecutor.ExecuteAsyncScript(script, args));
    }
}