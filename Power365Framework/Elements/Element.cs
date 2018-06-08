using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class Element : ElementBase
    {
        public Element(By locator, IWebDriver webDriver) 
            : base(LogManager.GetLogger(typeof(Element)), locator, webDriver) { }

        protected Element(ILog logger, By locator, IWebDriver webDriver)
            : base(logger, locator, webDriver) { }

        public bool IsVisible(int timeoutInSec = 5, int pollIntervalInSec = 0)
        {
            return IsElementVisible(Locator, timeoutInSec, pollIntervalInSec);
        }
    }
}
