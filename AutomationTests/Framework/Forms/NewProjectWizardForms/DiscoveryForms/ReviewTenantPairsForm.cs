using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class ReviewTenantPairsForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'review your tenant pairs')]");
		public ReviewTenantPairsForm() : base(TitleLocator, "Review tenant pairs form")
		{
		}
	}
}