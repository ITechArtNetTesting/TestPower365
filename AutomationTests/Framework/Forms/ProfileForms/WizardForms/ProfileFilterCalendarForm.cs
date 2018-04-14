using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileFilterCalendarForm : BaseWizardStepForm
    {
       
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToFilterCalendarAppointments']");

        private Guid driverId;

        public ProfileFilterCalendarForm(Guid driverId) : base(TitleLocator, "Profile filter calendar form")
        {
            this.driverId = driverId;
        }
    }
}
