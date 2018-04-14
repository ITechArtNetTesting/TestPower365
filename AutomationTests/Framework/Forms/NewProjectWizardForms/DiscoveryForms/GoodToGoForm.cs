using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class GoodToGoForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'good to go')]");

		public GoodToGoForm(Guid driverId) : base(TitleLocator, "Good to go form")
		{
            this.driverId = driverId;
		}
	}
}