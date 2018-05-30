using BinaryTree.Power365.AutomationFramework.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ErrorsPage: ActionPageBase
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

        private ButtonElement selectActionDropdown
        {
            get
            {
               return new ButtonElement(actionsDropdown, WebDriver);
            }
        }


        private Element DismissAction
        {
            get
            {
                var tab = By.XPath(string.Format(actionsFormat, "Dismiss"));
                return new Element(tab, WebDriver);
            }
        }

        private Element ExportAction
        {
            get
            {
                var tab = By.XPath(string.Format(actionsFormat, "Export"));
                return new Element(tab, WebDriver);
            }
        }

              
        private ButtonElement yesButton
        {
            get
            {
                return new ButtonElement(By.XPath("//button[contains(@data-bind,'confirm')]"), WebDriver);
            }
        }

        private readonly string elementContainsText = "//div[@id='users']//*[contains(text(),'{0}')]";
        private readonly string tabFormat = "//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'{0}')]";
        private readonly By actionsDropdown = By.XPath("//div[@class='tab-pane active']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]");
       private readonly string actionsFormat = "//div[@id='users']//*[contains(text(),'{0}')]";

        public ErrorsPage(IWebDriver webDriver) : base(_locator,webDriver) { }

        public bool CheckDismissAndExportAreDisplayed()
        {
            selectActionDropdown.Click(); 
            return DismissAction.IsVisible() && ExportAction.IsVisible();
        }

        public bool IsExportDisplayed()
        {
            selectActionDropdown.Click();           
            return ExportAction.IsVisible();
        }      

    }
}
