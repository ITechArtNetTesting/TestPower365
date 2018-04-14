using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class AlmostDoneForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'almost done')]");

		public AlmostDoneForm(Guid driverId) : base(TitleLocator, "Almost done form")
		{
            this.driverId = driverId;
		}
		public void VerifySubmitIsEnabled()
		{
			Log.Info("Verifying Submit is enabled");
			nextButton.IsPresent();
		}
	}
}