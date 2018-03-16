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
    public class ProfileCreateDistributionGroupsForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Do you want to create new distribution groups in the target tenant')]");

        public ProfileCreateDistributionGroupsForm() : base(TitleLocator, "Create distributon groups form")
        {
        }

        private Label yesLabel = new Label(By.XPath("//label[contains(@for, 'createGroups')]"), "Yes label");
        private RadioButton yesRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'createGroups')]"), "Yes radiobutton");
        private Label noLabel = new Label(By.XPath("//label[contains(@for, 'dontCreateGroups')]"), "No label");
        private RadioButton noRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'dontCreateGroups')]"), "No radiobutton");

        public void SelectYes()
        {
            Log.Info("Selecting yes");
            yesLabel.Click();
            try
            {
                yesRadioButton.WaitForSelected(5000);
            }
            catch (Exception)
            {
                Log.Info("Radiobutton is not ready");
                yesLabel.Click();
            }
        }

        public void SelectNo()
        {
            Log.Info("Selecting no");
            noLabel.Click();
            try
            {
                Thread.Sleep(3000);
                noRadioButton.WaitForSelected(5000);
            }
            catch (Exception)
            {
                Log.Info("Radiobutton is not ready");
                noLabel.Click();
            }
        }
    }
}
