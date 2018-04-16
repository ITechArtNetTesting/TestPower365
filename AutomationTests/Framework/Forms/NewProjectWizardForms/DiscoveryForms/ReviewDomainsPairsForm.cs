using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class ReviewDomainsPairsForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Domains')]");

		public ReviewDomainsPairsForm(Guid driverId) : base(TitleLocator, "Review domain pairs form",driverId)
		{
            this.driverId = driverId;
            addAnotherPairButton = new Button(By.XPath("//button[contains(@data-bind, 'addDomain')]"), "Add another pair button",driverId);
        }

		private readonly Button addAnotherPairButton ;

		public void AddAnotherPair()
		{
			Log.Info("Adding another pair");
			addAnotherPairButton.Click();
		}
	}
}