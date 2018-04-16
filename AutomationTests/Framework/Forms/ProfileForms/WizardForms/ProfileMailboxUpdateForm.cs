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
        

        public ProfileMailboxUpdateForm(Guid driverId) : base(TitleLocator, "Update mailbox form",driverId)
        {
            this.driverId = driverId;
            hideLabel = new Label(By.XPath("//label[contains(@for, 'hiddenSource')]"), "Yes label",driverId);
            hideRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'hiddenSource')]"), "Yes radiobutton",driverId);
            leaveLabel = new Label(By.XPath("//label[contains(@for, 'leaveSource')]"), "No label",driverId);
            leaveRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'leaveSource')]"), "No radiobutton",driverId);
        }
        private Label hideLabel ;
        private RadioButton hideRadioButton ;
        private Label leaveLabel ;
        private RadioButton leaveRadioButton ;

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
