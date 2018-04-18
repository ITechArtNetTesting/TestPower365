using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class AlmostDoneForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'almost done')]");

		public AlmostDoneForm() : base(TitleLocator, "Almost done form")
		{
		}
		public void VerifySubmitIsEnabled()
		{
			Log.Info("Verifying Submit is enabled");
			nextButton.IsPresent();
		}
	}
}