using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class SelectMigrationGroupForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Which Active Directory groups')]");

		private readonly TextBox groupTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'searchQuery')]"),
			"Group textbox");
		
		public SelectMigrationGroupForm() : base(TitleLocator, "Select migration group form")
		{
		}

		public SelectMigrationGroupForm(By _TitleLocator, string _name) : base(_TitleLocator, _name)
		{
		}
		public void SetGroup(string group)
		{
			Log.Info("Setting group: " + group);
			groupTextBox.ClearSetText(group);
		}

		public void SelectGroup(string group)
		{
			Log.Info("Selecting group: " + group);
			var groupButton = new Button(By.XPath($"//a[contains(@data-bind, 'selectGroup')][contains(text(), '{group}')]"),
				group + " button");
			groupButton.Click();
		}
	}
}