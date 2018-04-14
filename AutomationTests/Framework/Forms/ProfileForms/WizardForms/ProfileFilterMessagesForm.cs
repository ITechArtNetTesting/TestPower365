using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileFilterMessagesForm : BaseWizardStepForm
    {
       
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToFilterEmailMessages']");

        private Guid driverId;

        public ProfileFilterMessagesForm(Guid driverId) : base(TitleLocator, "Profile filter messages form")
        {
            this.driverId = driverId;
        }
    }
}
