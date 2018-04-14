using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileFilterNoteForm : BaseWizardStepForm
    {
       
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToFilterNoteItems']");

        private Guid driverId;
        
        public ProfileFilterNoteForm(Guid driverId) : base(TitleLocator, "Profile Filter note form")
        {
            this.driverId = driverId;
        }
    }
}
