using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class SetProjectNameForm : BaseWizardStepForm
	{
        Store store;
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'What should we call this project?')]");
	        private readonly TextBox nameTextBox ;

        public SetProjectNameForm(Guid driverId,Store store) : base(TitleLocator, "Set project name form",driverId)
		{
            this.store = store;
            this.driverId = driverId;
            nameTextBox = new TextBox(By.Id("projectName"), "Name textbox",driverId);
        }

		public void SetName(string name)
		{
			Log.Info("Setting name: " + name);
			nameTextBox.ClearSetText(name);
			store.ProjectName = name;
		}
	}
}