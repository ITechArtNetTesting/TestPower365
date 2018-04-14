using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class DiscoveryIsInProgressForm : BaseWizardStepForm
	{
        private Guid driverId;
        private static readonly By TitleLocator = By.XPath("//span[contains(text(), 'begin discovery')]");

		public DiscoveryIsInProgressForm(Guid driverId) : base(TitleLocator, "Discovery is in progress form")
		{
            this.driverId = driverId;
		}
	}
}