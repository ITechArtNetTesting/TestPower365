using OpenQA.Selenium;
using Product.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms
{
    public class PublicFoldersOverviewForm:BaseForm
    {
        public PublicFoldersOverviewForm() : base(TitleLocator,"Public Folders Overview Form")
        {
            integrationLabel.WaitForElementPresent();
        }

        protected Label integrationLabel => new Label(TitleLocator, "Integration");
        private static readonly By TitleLocator = By.XPath("//*[text()='Integration']");

        private readonly Element FirstPublicFolder = new Element(By.XPath("//*/div[@id='publicFolderContainer']//*/tbody"), "FirstPublicFolder");
        private readonly Element ActionsDropDown = new Element(By.XPath("//*/button[@class='btn button-dark dropdown-toggle']"), "ActionsDropDown");
        private readonly Button ArchiveActionButton = new Button(By.XPath("//*/span[text()='Archive']"), "ArchiveActionButton");
        private readonly Button ApplyActionButton = new Button(By.XPath("//*/span[text()='Apply action']"), "ApplyActionButton");
        private readonly Button BackToDashboardButton = new Button(By.XPath("//*/span[@data-translation='BackToDashboard']"), "BackToDashboardButton");
        private readonly Button YesSureArchive = new Button(By.XPath("//*/button[@data-bind='click: confirm']"), "YesSureArchive");           

        public void SelectFirstPublicFolder()
        {
            FirstPublicFolder.Click();
        }
        public void PerformArchiveAction()
        {
            ActionsDropDown.Click();
            ArchiveActionButton.Click();
        }
        public void ApplyChangies()
        {
            ApplyActionButton.Click();
            YesSureArchive.Click();
        }
        public void BackToDashboard()
        {
            BackToDashboardButton.Click();
        }
    }
}
