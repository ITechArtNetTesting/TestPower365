using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
    public class DownloadDirSyncForm : BaseWizardStepForm
    {
        private readonly static By TitleLocator = By.XPath(
            "//div[contains(@class, 'wizard-body')]//*[contains(text(), 'download the Power365 Directory Sync Pro')]");

        public DownloadDirSyncForm() : base(TitleLocator, "Download dirsync app form")
        {
        }
    }
}
