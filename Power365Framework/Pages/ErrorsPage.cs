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

        private By _usersTab = By.XPath("//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'Users')]");
        private By _tenantsTab = By.XPath("//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'Tenants')]");
        private By _groupsTab = By.XPath("//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'Groups')]");

        public ErrorsPage(IWebDriver webDriver) : base(_locator,webDriver) { }

        public bool UsersGrpoupsTenantsAreDisplayed()
        {
            bool result;
            result= validateElement(_usersTab);
            result = validateElement(_tenantsTab);
            result = validateElement(_groupsTab);
            return result;
        }

        public bool UsersTenantsAreDisplayed()
        {
            bool result;
            result = validateElement(_usersTab);
            result = validateElement(_tenantsTab);            
            return result;
        }

        private bool validateElement(By element)
        {
            bool result;
            ClickElementBy(element);
            result=IsElementExists(element);
            result=IsElementVisible(element);
            return result;
        }
    }
}
