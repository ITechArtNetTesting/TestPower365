using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileToLicenseMailboxesForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToLicenseYourTargetMailboxes']");

        private Guid driverId;

        public ProfileToLicenseMailboxesForm(Guid driverId) : base(TitleLocator, "Profile to license the target mailboxes")
        {
            this.driverId = driverId;
        }
    }
}
