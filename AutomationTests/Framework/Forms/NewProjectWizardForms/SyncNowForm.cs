using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
    public class SyncNowForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'sync')]");

        private Button back = new Button(By.XPath("//span[@data-translation='Back']"), "Back button");

        public SyncNowForm() : base(TitleLocator, "Sync now form")
        {

        }
             

    }
}