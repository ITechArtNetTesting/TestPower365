using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
    public class DownloadDirSyncForm : BaseWizardStepForm
    {
       
        private readonly static By TitleLocator = By.XPath(
            "//*/span[@data-translation='LetsDownloadPower365DirectorySyncLiteApplication']");

        private readonly Button _downloadDirSyncButton = new Button(By.XPath("//a[contains(@data-bind,'download')]//span[contains(@data-translation,'Start')]"), "Button to download DirSync Lite");


        public DownloadDirSyncForm() : base(TitleLocator, "Download dirsync app form")
        {
        }

        public  bool SeeDownloadDirSync()
        {
            return _downloadDirSyncButton.IsElementVisible();
        }
    }
}
