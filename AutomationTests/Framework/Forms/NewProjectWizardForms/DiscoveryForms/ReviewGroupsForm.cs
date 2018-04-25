using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class ReviewGroupsForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'review the selected Active Directory groups')]");

		public ReviewGroupsForm() : base(TitleLocator, "Review AD groups form")
		{
		}
	}
}