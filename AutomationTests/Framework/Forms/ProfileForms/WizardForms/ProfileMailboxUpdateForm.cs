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
    public class ProfileMailboxUpdateForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'When users switch to their target mailbox, how would you like their source mailbox to be updated')]");

        public ProfileMailboxUpdateForm() : base(TitleLocator, "Update mailbox form")
        {
        }
        private Label hideLabel = new Label(By.XPath("//label[contains(@for, 'hiddenSource')]"), "Yes label");
        private RadioButton hideRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'hiddenSource')]"), "Yes radiobutton");
        private Label leaveLabel = new Label(By.XPath("//label[contains(@for, 'leaveSource')]"), "No label");
        private RadioButton leaveRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'leaveSource')]"), "No radiobutton");

        public void SelectHide()
        {
            Log.Info("Selecting yes");
            hideLabel.Click();
            try
            {
                hideRadioButton.WaitForSelected(5000);
            }
            catch (Exception)
            {
                Log.Info("Radiobutton is not ready");
                hideLabel.Click();
            }
        }

        public void SelectLeave()
        {
            Log.Info("Selecting no");
            leaveLabel.Click();
            try
            {
                Thread.Sleep(3000);
                leaveRadioButton.WaitForSelected(5000);
            }
            catch (Exception)
            {
                Log.Info("Radiobutton is not ready");
                leaveLabel.Click();
            }
        }
    }
}
