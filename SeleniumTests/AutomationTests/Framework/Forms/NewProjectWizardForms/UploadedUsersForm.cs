using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class UploadedUsersForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Upload was a success')]");
		public UploadedUsersForm() : base(TitleLocator, "Uploaded users form")
		{
		}

		public UploadedUsersForm(By _TitleLocator, string name) : base(_TitleLocator, name)
		{
		}
		private new readonly Button backButton = new Button(By.XPath("//button[contains(@data-bind, 'goReupload')]"), "Go back button");
		public new void GoBack()
		{
			Log.Info("Going back");
			backButton.Click();
		}
	}
}