using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class ReviewTenantPairsForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'review your tenant pairs')]");
		public ReviewTenantPairsForm(Guid driverId) : base(TitleLocator, "Review tenant pairs form")
		{
            this.driverId = driverId;
		}
	}
}