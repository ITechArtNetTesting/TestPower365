﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;
using Product.Framework.Forms.NewProjectWizardForms;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileNameForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'What should we call this migration profile')]");

        

        public ProfileNameForm(Guid driverId) : base(TitleLocator, "Profile name form",driverId)
        {
            this.driverId = driverId;
            nameTextBox = new TextBox(By.Id("profileName"), "Profile name textbox",driverId);
        }

        private TextBox nameTextBox ;

        public void SetName(string name)
        {
            Log.Info("Setting profile name");
            nameTextBox.ClearSetText(name);
        }
    }
}
