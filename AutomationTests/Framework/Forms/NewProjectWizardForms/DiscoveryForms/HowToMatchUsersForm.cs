using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class HowToMatchUsersForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How would you like to match')]");

		public HowToMatchUsersForm() : base(TitleLocator, "How to match users form")
		{
		}

		public HowToMatchUsersForm(By _TitleLocator, string name) : base(_TitleLocator, name)
		{
		}
	}
}