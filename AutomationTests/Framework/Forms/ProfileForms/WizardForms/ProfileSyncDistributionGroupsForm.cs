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
       
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToSyncDistributionGroups']");
        

        public ProfileSyncDistributionGroupsForm(Guid driverId) : base(TitleLocator, "Sync distribution grops form",driverId)
        {
            this.driverId = driverId;
            manuallyLabel = new Label(By.XPath("//label[contains(@for, 'manual')]"), "Yes label",driverId);
            manuallyRadioButton = new RadioButton(By.Id("manual"), "Yes radiobutton",driverId);
            continuouslyLabel = new Label(By.XPath("//label[contains(@for, 'continuous')]"), "No label",driverId);
            continuouslyRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'continuous')]"), "No radiobutton",driverId);
        }

        private Label manuallyLabel ;
        private RadioButton manuallyRadioButton ;
        private Label continuouslyLabel ;
        private RadioButton continuouslyRadioButton ;

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
