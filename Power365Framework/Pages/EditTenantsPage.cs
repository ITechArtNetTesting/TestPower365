using OpenQA.Selenium;
using System;
using System.Collections.Generic;
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

        public void ClickDiscoveryTab()
        {
            ClickElementBy(_discoveryTab);
        }

        public bool CheckDiscoveryFileIsDownloaded()
        {
            throw new NotImplementedException();
        }

        public void DownloadLogs()
        {

        }
    }
}
