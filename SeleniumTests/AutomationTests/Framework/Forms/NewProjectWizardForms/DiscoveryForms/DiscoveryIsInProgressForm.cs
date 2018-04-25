using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class DiscoveryIsInProgressForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//span[contains(text(), 'begin discovery')]");

		public DiscoveryIsInProgressForm() : base(TitleLocator, "Discovery is in progress form")
		{
		}
	}
}