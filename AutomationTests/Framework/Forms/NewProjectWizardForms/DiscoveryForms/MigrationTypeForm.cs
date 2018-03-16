using System;
using System.Threading;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class MigrationTypeForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How would you like to discover')]");

		private readonly Button groupsButton = new Button(By.XPath("//label[@for='groupsRadio']"), "Groups button");

		public MigrationTypeForm() : base(TitleLocator, "Migration type form")
		{
		}

		public void SelectGroupsOption()
		{
			Log.Info("Selecting groups option");
			groupsButton.Click();
            Thread.Sleep(3000);
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				groupsButton.Click();
			}
		}
	}
}