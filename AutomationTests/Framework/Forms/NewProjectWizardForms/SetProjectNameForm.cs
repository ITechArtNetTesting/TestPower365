using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class SetProjectNameForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'What should we call this project?')]");
	        private readonly TextBox nameTextBox = new TextBox(By.Id("projectName"), "Name textbox");

        public SetProjectNameForm(Guid driverId) : base(TitleLocator, "Set project name form")
		{
            this.driverId = driverId;
		}

		public void SetName(string name)
		{
			Log.Info("Setting name: " + name);
			nameTextBox.ClearSetText(name);
			Store.ProjectName = name;
		}
	}
}