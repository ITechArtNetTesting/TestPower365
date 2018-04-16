using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class SetProjectDescriptionForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Describe this project in just a few words')]");

	        private readonly TextBox descriptionTextBox ;

        public SetProjectDescriptionForm(Guid driverId) : base(TitleLocator, "Set project description form",driverId)
		{
            this.driverId = driverId;
            descriptionTextBox = new TextBox(By.Id("projectDescription"),"Description textbox",driverId);
        }

		public void SetDescription(string description)
		{
			Log.Info("Setting description: " + description);
			descriptionTextBox.ClearSetText(description);
		}
	}
}