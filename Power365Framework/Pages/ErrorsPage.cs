using BinaryTree.Power365.AutomationFramework.Elements;
using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ErrorsPage: ActionPageBase
    {
        public Element GroupsTab
        {
            get
            {
                var tab = By.XPath(string.Format(tabFormat, "Groups"));
                return new Element(tab, WebDriver);
            }
        }

        public Element UsersTab
        {
            get
            {
                var tab = By.XPath(string.Format(tabFormat, "Users"));
                return new Element(tab, WebDriver);
            }
        }

        public Element TenantsTab
        {
            get
            {
                var tab = By.XPath(string.Format(tabFormat, "Tenants"));
                return new Element(tab, WebDriver);
            }
        }

        public ButtonElement ActionDropdown
        {
            get
            {
               return new ButtonElement(actionsDropdown, WebDriver);
            }
        }

        public Element DismissAction
        {
            get
            {
                var tab = By.XPath(string.Format(actionsFormat, "Dismiss"));
                return new Element(tab, WebDriver);
            }
        }

        public Element ExportAction
        {
            get
            {
                var tab = By.XPath(string.Format(actionsFormat, "Export"));
                return new Element(tab, WebDriver);
            }
        }

        private static readonly By _locator = By.XPath("//div[@id='users']//input[contains(@placeholder,'error')]");
        private readonly string tabFormat = "//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'{0}')]";
        private readonly By actionsDropdown = By.XPath("//div[@class='tab-pane active']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]");
        private readonly string actionsFormat = "//div[@id='users']//*[contains(text(),'{0}')]";

        public ErrorsPage(IWebDriver webDriver) 
            : base(LogManager.GetLogger(typeof(ErrorsPage)), _locator,webDriver) { }

        public bool CheckDismissAndExportAreDisplayed()
        {
            ActionDropdown.Click(); 
            return DismissAction.IsVisible() && ExportAction.IsVisible();
        }

        public bool IsExportDisplayed()
        {
            ActionDropdown.Click();           
            return ExportAction.IsVisible();
        }
    }
}
