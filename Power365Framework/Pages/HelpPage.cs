using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class HelpPage : PageBase
    {
        private static readonly By _locator = By.XPath("//h1[text()=' Help Center']");

        public HelpPage(IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(EditWavePage)), _locator, webDriver) { }

        public string GetUrl()
        {
            return WebDriver.Url;
        }
    }
}