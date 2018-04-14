using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class ReviewGroupsForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'review the selected Active Directory groups')]");

		public ReviewGroupsForm(Guid driverId) : base(TitleLocator, "Review AD groups form")
		{
            this.driverId = driverId;
		}
	}
}