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
    public class EditTenantsPage: PageBase
    {


        private static By _locator = By.Id("tenantsManagementContainer");

        public EditTenantsPage(IWebDriver webDriver) : base(_locator, webDriver) { }

        private readonly By _discoveryTab = By.XPath("//a[@role='tab' and contains(@href,'discovery')]");

        private string tenantLogsXPath = "//*[contains(text(), '{0}')]/ancestor::tr//a[contains(@data-bind, 'exportTenantLogs')]";

        private string tenantDisableXPath = "//*[contains(text(), '{0}')]//ancestor::tr//span[contains(@data-translation,'Disable')]";

        private string tenantEnableXPath = "//*[contains(text(), '{0}')]//ancestor::tr//a[contains(@data-bind,'enable')]";    

        public void ClickDiscoveryTab()
        {
            ClickElementBy(_discoveryTab);
        }

        public bool CheckDiscoveryFileIsDownloaded(string downloadPath)
        {            
            FileInfo[] downloadedFiles = new DirectoryInfo(downloadPath).GetFiles("Tenant*.csv");         
            DefaultWait<bool> wait = new DefaultWait<bool>(downloadedFiles.Count() >= 1);
            Func<bool, bool> fileIsDownloaded = new Func<bool, bool>((bool condition) =>
            {
                return condition;
            });
            foreach (var file in downloadedFiles)
            {
                file.Delete();
            }
            return wait.Until(fileIsDownloaded);
        }

        public void DownloadLogs(string tenant)
        {
            By tenantLogs = By.XPath(string.Format(tenantLogsXPath, tenant));
            HowerElement(tenantLogs);
            ClickElementBy(tenantLogs);
        }

        public void DisableTenant(string tenant)
        {
            By tenantEnable = By.XPath(string.Format(tenantEnableXPath, tenant));
            if (IsElementExists(tenantEnable))
            {
                HowerElement(tenantEnable);
                ClickElementBy(tenantEnable);
            }
            By tenantDisable = By.XPath(string.Format(tenantDisableXPath, tenant));
            HowerElement(tenantDisable);
            ClickElementBy(tenantDisable);            
        }

        public void EnableTenant(string tenant)
        {
            By tenantDisable = By.XPath(string.Format(tenantDisableXPath, tenant));
            if (IsElementExists(tenantDisable))
            {
                HowerElement(tenantDisable);
                ClickElementBy(tenantDisable);
            }
            By tenantEnable = By.XPath(string.Format(tenantEnableXPath, tenant));
            HowerElement(tenantEnable);
            ClickElementBy(tenantEnable);
        }

        public bool CheckTenantCanBeDisabled(string tenant)
        {
            By tenantEnable = By.XPath(string.Format(tenantEnableXPath, tenant));          
            return IsElementExists(tenantEnable, 300,5);
        }

        public bool CheckTenantCanBeEnabled(string tenant)
        {
            By tenantDisable = By.XPath(string.Format(tenantDisableXPath, tenant));
            return IsElementExists(tenantDisable, 300, 5);
        }
    }
}
