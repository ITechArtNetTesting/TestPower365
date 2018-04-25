using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class ReviewDomainsPairsForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Domains')]");

		public ReviewDomainsPairsForm() : base(TitleLocator, "Review domain pairs form")
		{
		}

		private readonly Button addAnotherPairButton = new Button(By.XPath("//button[contains(@data-bind, 'addDomain')]"), "Add another pair button");

		public void AddAnotherPair()
		{
			Log.Info("Adding another pair");
			addAnotherPairButton.Click();
		}
	}
}