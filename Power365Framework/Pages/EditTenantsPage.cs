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
    }
}
