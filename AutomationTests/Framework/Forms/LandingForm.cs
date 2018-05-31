using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
    public class LandingForm : BaseForm
    {
        private static By Locator = By.Id("landingContainer");
        private By _t2tButtonLocator = By.XPath("//div[contains(@data-bind, 'goToPower365')]");
        private By _dirSyncButtonLocator = By.XPath("//div[contains(@data-bind, 'goToCloudDirectorySync')]");

        public LandingForm()
            : base(Locator, "Landing Page") { }

        public void ClickT2T()
        {
            var t2tButton = new Button(_t2tButtonLocator, "T2T Button");
            t2tButton.WaitForElementIsClickable();
            t2tButton.Click();
        }

        public void ClickDirSync()
        {
            var dirSyncButton = new Button(_dirSyncButtonLocator, "DirSync Button");
            dirSyncButton.WaitForElementIsClickable();
            dirSyncButton.Click();
        }
    }
}
