using BinaryTree.Power365.AutomationFramework.Elements;
using log4net;
using OpenQA.Selenium;
using System;
using System.IO;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class EditTenantsPage : PageBase
    {
        public Element AddressBookTab
        {
            get
            {
                var tab = By.XPath(string.Format(_navigationTabFormat, "address book"));
                return new Element(tab, WebDriver);
            }
        }

        public Element DiscoveryTab
        {
            get
            {
                var tab = By.XPath(string.Format(_navigationTabFormat, "discovery"));
                return new Element(tab, WebDriver);
            }
        }

        public Element TenantsTab
        {
            get
            {
                var tab = By.XPath(string.Format(_navigationTabFormat, "tenants"));
                return new Element(tab, WebDriver);
            }
        }

        public Element DirectorySyncTab
        {
            get
            {
                var tab = By.XPath(string.Format(_navigationTabFormat, "directory sync"));
                return new Element(tab, WebDriver);
            }
        }

        private static By _locator = By.Id("tenantsManagementContainer");

        private readonly By _discoveryTab = By.XPath("//a[@role='tab' and contains(@href,'discovery')]");

        private string tenantLogsXPath = "//*[contains(text(), '{0}')]/ancestor::tr//a[contains(@data-bind, 'exportTenantLogs')]";
        private string tenantLogsXref = "//*[contains(text(), '{0}')]/ancestor:://tr//a[contains(@data-bind, 'exportTenantLogs')]/@href";
        private string tenantDisableFormat = "//*[contains(text(), '{0}')]//ancestor::tr//span[contains(@data-translation,'Disable')]";

        private string tenantEnableFormat = "//*[contains(text(), '{0}')]//ancestor::tr//a[contains(@data-bind,'enable')]";
        private readonly string _navigationTabFormat = ("//*[contains(@class, 'nav nav-tabs')]/li/a/*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]]");
        private readonly string _tenantStatus = "//div[@id='discovery']//tr[child::*//strong[contains(text(),'{0}')]]//i[contains(@class,'check')]";

        public EditTenantsPage(IWebDriver webDriver) 
            : base(LogManager.GetLogger(typeof(EditTenantsPage)), _locator, webDriver) { }

        public void ClickDiscoveryTab()
        {
            ClickElementBy(_discoveryTab);
        }          

        public bool CheckTenantCanBeEnabledOrDisabled(string tenant, bool isCheckEnabled = true)
        {
            string outputState;
            string inputState;
            if (isCheckEnabled)
            {
                inputState = string.Format(tenantDisableFormat, tenant);
                 outputState = string.Format(tenantEnableFormat, tenant);
            }
            else
            {
                outputState = string.Format(tenantDisableFormat, tenant);
                inputState = string.Format(tenantEnableFormat, tenant);
            }

           ChangeTenantsDiscoveryState(inputState,  outputState,  tenant);
           By tenantStateElement =By.XPath(inputState);
           return IsElementExists(tenantStateElement, 300, 5);
        }

        private void ChangeTenantsDiscoveryState(string inputState, string outputState, string tenant)
        {
            By initTenantState = By.XPath(string.Format(inputState, tenant));
            if (IsElementExists(initTenantState)) // check the initial state
            {
                HoverElement(initTenantState);
                ClickElementBy(initTenantState);
            }
            By tenantOutputState = By.XPath(string.Format(outputState, tenant));
            HoverElement(tenantOutputState);
            ClickElementBy(By.XPath(string.Format(outputState, tenant)));
        }
        
        public void DownloadLogs(string tenant)
        {
            By tenantLogs = By.XPath(string.Format(tenantLogsXPath, tenant));
            HoverElement(tenantLogs);
          
           ClickElementBy(By.XPath(string.Format(tenantLogsXPath, tenant)));
        }
        
        public bool IsTenantDirSyncStatusSuccesfull(string tenantName)
        {
            var OkStatus = By.XPath(string.Format(_tenantStatus, tenantName));
            return IsElementVisible(OkStatus, 10, 1);
        }
    }
}
