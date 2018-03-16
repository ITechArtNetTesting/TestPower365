using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class SetProjectNameForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'What should we call this project?')]");
		private readonly TextBox nameTextBox = new TextBox(By.XPath("//div[@class='wizard-body']//input"), "Name textbox");

		public SetProjectNameForm() : base(TitleLocator, "Set project name form")
		{
		}

		public void SetName(string name)
		{
			Log.Info("Setting name: " + name);
			nameTextBox.ClearSetText(name);
			Store.ProjectName = name;
		}
	}
}