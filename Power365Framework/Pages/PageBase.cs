using BinaryTree.Power365.AutomationFramework.Components;
using BinaryTree.Power365.AutomationFramework.Elements;
using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public abstract class PageBase: ElementBase
    {
        public MenuComponent Menu
        {
            get
            {
                return new MenuComponent(WebDriver);
            }
        }

        protected PageBase(ILog logger, By locator, IWebDriver webDriver)
            :base (logger, locator, webDriver) { }
        
        public void Refresh()
        {
            WebDriver.Navigate().Refresh();
        }
    }
}
