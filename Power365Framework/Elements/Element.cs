using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class Element : ElementBase
    {
        public Element(By locator, IWebDriver webDriver) 
            : base(locator, webDriver) { }

        public bool IsVisible(int timeoutInSec = 5, int pollIntervalInSec = 0)
        {
            return IsElementVisible(Locator, timeoutInSec, pollIntervalInSec);
        }
    }
}
