using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class SetProjectDescriptionForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Describe this project in just a few words')]");

	        private readonly TextBox descriptionTextBox = new TextBox(By.Id("projectDescription"),
            "Description textbox");

        public SetProjectDescriptionForm(Guid driverId) : base(TitleLocator, "Set project description form")
		{
            this.driverId = driverId;
		}

		public void SetDescription(string description)
		{
			Log.Info("Setting description: " + description);
			descriptionTextBox.ClearSetText(description);
		}
	}
}