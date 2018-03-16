using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;
using Product.Framework.Forms.NewProjectWizardForms;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileSyncDistributionGroupsForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How would you like to sync distribution groups')]");

        public ProfileSyncDistributionGroupsForm() : base(TitleLocator, "Sync distribution grops form")
        {
        }

        private Label manuallyLabel = new Label(By.XPath("//label[contains(@for, 'manual')]"), "Yes label");
        private RadioButton manuallyRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'manual')]"), "Yes radiobutton");
        private Label continuouslyLabel = new Label(By.XPath("//label[contains(@for, 'continuous')]"), "No label");
        private RadioButton continuouslyRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'continuous')]"), "No radiobutton");

        public void SelectYes()
        {
            Log.Info("Selecting yes");
            manuallyLabel.Click();
            try
            {
                manuallyRadioButton.WaitForSelected(5000);
            }
            catch (Exception)
            {
                Log.Info("Radiobutton is not ready");
                manuallyLabel.Click();
            }
        }

        public void SelectNo()
        {
            Log.Info("Selecting no");
            continuouslyLabel.Click();
            try
            {
                Thread.Sleep(3000);
                continuouslyRadioButton.WaitForSelected(5000);
            }
            catch (Exception)
            {
                Log.Info("Radiobutton is not ready");
                continuouslyLabel.Click();
            }
        }
    }
}
