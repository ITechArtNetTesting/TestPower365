using BinaryTree.Power365.AutomationFramework.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;


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

        private string tenantDisableFormat = "//*[contains(text(), '{0}')]//ancestor::tr//span[contains(@data-translation,'Disable')]";

        private string tenantEnableFormat = "//*[contains(text(), '{0}')]//ancestor::tr//a[contains(@data-bind,'enable')]";
        private readonly string _navigationTabFormat = ("//*[contains(@class, 'nav nav-tabs')]/li/a/*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]]");
        private readonly string _tenantStatus = "//div[@id='discovery']//tr[child::*//strong[contains(text(),'{0}')]]//i[contains(@class,'check')]";

        public EditTenantsPage(IWebDriver webDriver) : base(_locator, webDriver) { }

        public void ClickDiscoveryTab()
        {
            ClickElementBy(_discoveryTab);
        }


        //delete
        public bool CheckDiscoveryFileIsDownloaded(string downloadPath, int timeout)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(downloadPath);            
            DefaultWait<DirectoryInfo> wait = new DefaultWait<DirectoryInfo>(directoryInfo);
            wait.Timeout = TimeSpan.FromSeconds(timeout);
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            Func<DirectoryInfo, bool>  fileIsDownloaded = new Func<DirectoryInfo, bool> ((DirectoryInfo info) =>
            {    
                var test= info.GetFiles("Tenant*.csv").Count() >= 1;
                return test;
            });
            try
            {
                return wait.Until(fileIsDownloaded);               
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        

        public bool CheckTenantCanBeEnabledOrDisabled(string tenant, bool isCheckEnabled=true)
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
            ClickElementBy(tenantOutputState);
        }


        public void DownloadLogs(string tenant)
        {
            By tenantLogs = By.XPath(string.Format(tenantLogsXPath, tenant));
            HoverElement(tenantLogs);
            ClickElementBy(tenantLogs);
        }

        public void DeleteTenantLogs(string downloadPath)
        {
            FileInfo[] downloadedFiles = new DirectoryInfo(downloadPath).GetFiles("Tenant*.csv");
            foreach (var file in downloadedFiles)
            {
                file.Delete();
            }
        }


        public bool IsTenantDirSyncStatusSuccesfull(string tenantName)
        {
            var OkStatus = By.XPath(string.Format(_tenantStatus, tenantName));
            return IsElementVisible(OkStatus, 10, 1);
        }
    }
}
