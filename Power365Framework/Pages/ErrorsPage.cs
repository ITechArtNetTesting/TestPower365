using BinaryTree.Power365.AutomationFramework.Elements;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ErrorsPage: PageBase
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
                var tab = By.XPath(string.Format(dropDownFormat, "Select action"));
                return new ButtonElement(tab, WebDriver);
            }
        }


        private Element DismissAction
        {
            get
            {
                var tab = By.XPath(string.Format(dropDownFormat, "Dismiss"));
                return new Element(tab, WebDriver);
            }
        }

        private Element ExportAction
        {
            get
            {
                var tab = By.XPath(string.Format(dropDownFormat, "Export"));
                return new Element(tab, WebDriver);
            }
        }

        private readonly string tabFormat = "//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'{0}')]";
        private readonly string dropDownFormat = "//div[@id='users']//*[contains(text(),'{0}')]";

        public ErrorsPage(IWebDriver webDriver) : base(_locator,webDriver) { }

        public bool CheckDismissAndExportAreDisplayed()
        {
            SelectActionButton.Click(); 
            return DismissAction.IsVisible() && ExportAction.IsVisible();
        }


    }
}
