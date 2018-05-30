using BinaryTree.Power365.AutomationFramework.Elements;
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
    public class ErrorsPage: PageBase
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

        private ButtonElement selectActionButton
        {
            get
            {
                var tab = By.XPath(string.Format(elementContainsText, "Select action"));
                return new ButtonElement(tab, WebDriver);
            }
        }

        private ButtonElement exportAction
        {
            get
            {
                var tab = By.XPath(string.Format(elementContainsText, "Export"));
                return new ButtonElement(tab, WebDriver);
            }
        }

        private ButtonElement selectAllCheckBox
        {
            get
            {
                return new ButtonElement(By.XPath("//div[@id='users']//*[contains(@data-bind,'allSelect')]"), WebDriver);
            }
        }

        private ButtonElement applyButton
        {
            get
            {
                var tab = By.XPath(string.Format(elementContainsText, "Apply"));
                return new ButtonElement(tab, WebDriver);
            }
        }

        private ButtonElement yesButton
        {
            get
            {
                return new ButtonElement(By.XPath("//button[contains(@data-bind,'confirm')]"), WebDriver);
            }
        }

        private readonly string elementContainsText = "//div[@id='users']//*[contains(text(),'{0}')]";

        private readonly string tabFormat = "//ul[@class='nav nav-tabs m-t-lg']//span[contains(text(),'{0}')]";

        public ErrorsPage(IWebDriver webDriver) : base(_locator,webDriver) { }

        public bool CheckExportAreDisplayed()
        {
            bool result;
            selectActionButton.Click();
            result= exportAction.IsVisible();
            selectActionButton.Click();
            return result;
        }

        public void SelectAllUsers()
        {
            WebDriver.FindElement(By.XPath("//div[@id='users']//*[contains(@data-bind,'allSelect')]")).Click();
        }

        public void ExportSelected()
        {
            selectActionButton.Click();
            exportAction.Click();
            applyButton.Click();
        }

        public void ClickYesButton()
        {
            yesButton.Click();
        }

        public void DeleteUserMigrationsJobsLogs(string downloadPath)
        {
            FileInfo[] downloadedFiles = new DirectoryInfo(downloadPath).GetFiles("UserMigrationsJobsLogs*.csv");
            foreach (var file in downloadedFiles)
            {
                file.Delete();
            }
        }

        public bool CheckUserMigrationJobsLogs(string downloadPath, int timeout)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(downloadPath);
            DefaultWait<DirectoryInfo> wait = new DefaultWait<DirectoryInfo>(directoryInfo);
            wait.Timeout = TimeSpan.FromSeconds(timeout);
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            Func<DirectoryInfo, bool> fileIsDownloaded = new Func<DirectoryInfo, bool>((DirectoryInfo info) =>
            {
                var test = info.GetFiles("UserMigrationsJobsLogs*.csv").Count() >= 1;
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
    }
}
