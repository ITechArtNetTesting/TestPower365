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

        private string tabsXPath = "//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'{0}')]";

        public ErrorsPage(IWebDriver webDriver) : base(_locator,webDriver) { }

        public bool UsersGrpoupsTenantsAreDisplayed()
        {
            By _usersTab = By.XPath(string.Format(tabsXPath, "Users"));
            By _tenantsTab = By.XPath(string.Format(tabsXPath, "Tenants"));
            By _groupsTab = By.XPath(string.Format(tabsXPath, "Groups"));
            bool usersTabResult= validateElement(_usersTab);
            bool tenantsTabResult = validateElement(_tenantsTab);
            bool groupsTabResult = validateElement(_groupsTab);
            return usersTabResult&&tenantsTabResult&&groupsTabResult;
        }

        public bool UsersTenantsAreDisplayed()
        {
            By _usersTab = By.XPath(string.Format(tabsXPath, "Users"));
            By _tenantsTab = By.XPath(string.Format(tabsXPath, "Tenants"));
            bool usersTabResult = validateElement(_usersTab);
            bool tenantsTabResult = validateElement(_tenantsTab);            
            return usersTabResult&&tenantsTabResult;
        }

        private bool validateElement(By element)
        {
            bool result;
            ClickElementBy(element);            
            result=IsElementVisible(element);
            return result;
        }
    }
}
