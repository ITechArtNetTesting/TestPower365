using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileTranslateSourceEmailForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='WouldYouLikeToTranslateSourceEmail']");
        

        public ProfileTranslateSourceEmailForm(Guid driverId) : base(TitleLocator, "Profile translate source email",driverId)
        {
            this.driverId = driverId;
        }

    }
}
