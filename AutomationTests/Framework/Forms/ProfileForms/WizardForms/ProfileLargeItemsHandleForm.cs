using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileLargeItemsHandleForm : BaseWizardStepForm
    {
      
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleLargeItems']");
        

        public ProfileLargeItemsHandleForm(Guid driverId) : base(TitleLocator, "Profile large items handle form",driverId)
        {
            this.driverId = driverId;
        }

    }
}
