using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class InputElement : Element
    {
        public InputElement(By locator, IWebDriver webDriver) 
            : base(locator, webDriver) { }

        public void SendKeys(string text)
        {
            var input = FindVisibleElement(Locator);
            input.SendKeys(text);
        }
    }
}