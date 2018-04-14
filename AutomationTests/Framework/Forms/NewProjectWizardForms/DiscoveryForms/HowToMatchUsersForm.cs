using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class HowToMatchUsersForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How would you like to match')]");

		public HowToMatchUsersForm(Guid driverId) : base(TitleLocator, "How to match users form")
		{
            this.driverId = driverId;
		}

		public HowToMatchUsersForm(By _TitleLocator, string name) : base(_TitleLocator, name)
		{
		}
	}
}