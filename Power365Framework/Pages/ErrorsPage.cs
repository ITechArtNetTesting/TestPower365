using BinaryTree.Power365.AutomationFramework.Elements;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ErrorsPage : PageBase
    {
        private static readonly By _locator = By.XPath("//div[@id='users']//input[contains(@placeholder,'error')]");


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

        private ButtonElement SelectActionButton
        {
            get
            {
                return new ButtonElement(By.XPath("//div[@id='users']//button//span[@data-translation='SelectAction']//font[contains(text(),'Select action')]"), WebDriver);
            }
        }


        private Element DismissAction
        {
            get
            {
                return new Element(By.XPath("//div[contains(@class,'open')]//ul[@class='dropdown-menu scrollable-dropdown' and contains(@data-bind,'actions')]//font[contains(text(),'Dismiss')]"), WebDriver);
            }
        }

        private Element ExportAction
        {
            get
            {
                return new Element(By.XPath("//div[contains(@class,'open')]//ul[@class='dropdown-menu scrollable-dropdown' and contains(@data-bind,'actions')]//font[contains(text(),'Export')]"), WebDriver);
            }
        }


        private readonly string tabFormat = "//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'{0}')]";

        public ErrorsPage(IWebDriver webDriver) : base(_locator,webDriver) { }

        public bool CheckDismissAndExportAreDisplayed()
        {
            SelectActionButton.Click();
            return DismissAction.IsVisible() && ExportAction.IsVisible();
        }
       
    }
}
