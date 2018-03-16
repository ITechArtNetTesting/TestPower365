using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;
using Product.Framework.Enums;
using Product.Framework.Forms.NewProjectWizardForms;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileContentToMigrateForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'What type of mailbox content would you like to migrate')]");

        public ProfileContentToMigrateForm() : base(TitleLocator, "Content to migrate form")
        {
        }

        public void SelectType(ContentType type)
        {
            Log.Info("Selecting: "+type.GetValue());
            Label typeLabel = new Label(By.XPath($"//label[contains(@for, '{type.GetValue()}')]"), "Type label");
            RadioButton typeRadioButton = new RadioButton(By.Id($"{type.GetValue()}"), "Type radiobutton");
            typeLabel.Click();
            try
            {
                typeRadioButton.WaitForSelected(5000);
            }
            catch (Exception)
            {
                Log.Info("Radiobutton is not ready");
                typeLabel.Click();
            }
        }
    }
}
