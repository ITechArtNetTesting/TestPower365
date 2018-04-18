using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class SetProjectDescriptionForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Describe this project in just a few words')]");

		private readonly TextBox descriptionTextBox = new TextBox(By.XPath("//div[@class='wizard-body']//input"),
			"Description textbox");

		public SetProjectDescriptionForm() : base(TitleLocator, "Set project description form")
		{
		}

		public void SetDescription(string description)
		{
			Log.Info("Setting description: " + description);
			descriptionTextBox.ClearSetText(description);
		}
	}
}