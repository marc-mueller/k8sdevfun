using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace UiTestAutomationBase
{
    public static class ElementHelpers
    {
        public static AppiumElement SetText(this AppiumElement element, string text)
        {
            //element.Clear(); // todo
            //element.SendKeys(text);
            return element;
        }

        public static string GetText(this AppiumElement element)
        {
            return element.Text;
        }
    }
}
