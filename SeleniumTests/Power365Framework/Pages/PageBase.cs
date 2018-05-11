using BinaryTree.Power365.AutomationFramework.Elements;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public abstract class PageBase: ElementBase
    {
        public MenuElement Menu
        {
            get
            {
                return new MenuElement(WebDriver);
            }
        }

        protected PageBase(By locator, IWebDriver webDriver)
            :base (locator, webDriver) { }

      
        public void Refresh()
        {
            WebDriver.Navigate().Refresh();
        }
    }
}
