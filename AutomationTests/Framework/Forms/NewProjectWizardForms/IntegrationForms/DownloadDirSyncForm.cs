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
            "//*/span[@data-translation='LetsDownloadPower365DirectorySyncLiteApplication']");


        public DownloadDirSyncForm(Guid driverId) : base(TitleLocator, "Download dirsync app form",driverId)
        {
            this.driverId = driverId;
        }
    }
}
