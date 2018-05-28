using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class GoodToGoForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'good to go')]");

        private readonly Label NoWavesDefinedLabel = new Label(By.XPath("//span[contains(@data-translation,'NoWavesDefined')]"), "Span which indicates that no waves defined");

		public GoodToGoForm() : base(TitleLocator, "Good to go form")
		{
		}

        public void CheckNoWavesDefined()
        {
            Assert.IsTrue(NoWavesDefinedLabel.IsElementVisible(),"No Waves Defined is not displayed on the summary page");
        }
	}
}