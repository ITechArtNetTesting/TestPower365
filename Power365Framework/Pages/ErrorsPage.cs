using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
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
        private static readonly By _locator = By.Id("users");
      

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

        private readonly string elementContainsText = "//div[@id='users']//*[contains(text(),'{0}')]";

        private readonly string tabFormat = "//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'{0}')]";

        public ErrorsPage(IWebDriver webDriver) : base(_locator,webDriver) { }

        public bool CheckExportAndDismissDisplayed()
        {
            return IsActionVisible(ActionType.Export) && IsActionVisible(ActionType.Dismiss);
        }

    }
}
