using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using T365.Framework;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Waiters;

namespace TestFramework.PageObjects
{
    public class TenantManagementPage : BasePage, ITenantManagementPage
    {
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='Disable']")]
        IList<IWebElement> TenantsDisables { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='BackToDashboard']")]
        IWebElement BackToDashboardButton { get; set; }

        public void ClickBackToDashboardButton()
        {
            Perform.Click(BackToDashboardButton);
        }

        public void DisableAllTenants()
        {
            foreach (var disable in TenantsDisables)
            {
                Perform.HowerThenClick(disable);
            }
        }
    }
}       
