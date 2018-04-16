using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;
using System;
using System.Collections.Generic;
using System.Linq;
using Product.Framework.Elements;
using Product.Framework.Enums;
using Product.Framework.Forms.NewProjectWizardForms;


namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileTypeOfMailboxContentForm : BaseWizardStepForm
    {

        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='WhatTypeOfMailboxContentWouldYouLikeToMigrate']");
        

        public ProfileTypeOfMailboxContentForm(Guid driverId) : base(TitleLocator, "Profile translate type email to migrate",driverId)
        {
            this.driverId = driverId;
        }
        public void SelectType(ContentType type)
        {
            Log.Info("Selecting: " + type.GetValue());
            Label typeLabel = new Label(By.XPath($"//label[contains(@for, '{type.GetValue()}')]"), "Type label",driverId);
            RadioButton typeRadioButton = new RadioButton(By.Id($"{type.GetValue()}"), "Type radiobutton",driverId);
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
