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
    public class EditTenantsPage : PageBase
    {
        private static By _locator = By.Id("tenantsManagementContainer");

        private readonly By _discoveryTab = By.XPath("//a[@role='tab' and contains(@href,'discovery')]");

        private string tenantLogsXPath = "//*[contains(text(), '{0}')]/ancestor::tr//a[contains(@data-bind, 'exportTenantLogs')]";

        public EditTenantsPage(IWebDriver webDriver) : base(_locator, webDriver) { }

        public void ClickDiscoveryTab()
        {
            ClickElementBy(_discoveryTab);
        }

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
    }
}
