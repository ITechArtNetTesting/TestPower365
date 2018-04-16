using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileFilterTasksForm : BaseWizardStepForm
    {
      
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToFilterTaskItems']");
        

        public ProfileFilterTasksForm(Guid driverId) : base(TitleLocator, "Profile filter tasks form",driverId)
        {
            this.driverId = driverId;
        }
    }
}
