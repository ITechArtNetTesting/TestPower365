﻿using System;
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
    public class ProfileOutlookConfigForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Will users need to update their Outlook profile to their new e-mail address')]");

        

        public ProfileOutlookConfigForm(Guid driverId) : base(TitleLocator, "Profile outlook configuration form",driverId)
        {
            this.driverId = driverId;
            yesLabel = new Label(By.XPath("//label[contains(@for, 'runAgentYes')]"), "Yes label",driverId);
            yesRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'runAgentYes')]"), "Yes radiobutton",driverId);
            noLabel = new Label(By.XPath("//label[contains(@for, 'runAgentNo')]"), "No label",driverId);
            noRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'runAgentNo')]"), "No radiobutton",driverId);
        }

        private Label yesLabel ;
        private RadioButton yesRadioButton ;
        private Label noLabel ;
        private RadioButton noRadioButton ;

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
