using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class UploadedUsersForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Upload was a success')]");
		public UploadedUsersForm(Guid driverId) : base(TitleLocator, "Uploaded users form",driverId)
		{
            this.driverId = driverId;
            backButton = new Button(By.XPath("//button[contains(@data-bind, 'goReupload')]"), "Go back button",driverId);
        }

		public UploadedUsersForm(By _TitleLocator, string name,Guid driverId) : base(_TitleLocator, name,driverId)
		{
            this.driverId = driverId;
            backButton = new Button(By.XPath("//button[contains(@data-bind, 'goReupload')]"), "Go back button", driverId);
        }
		private new readonly Button backButton ;
		public new void GoBack()
		{
			Log.Info("Going back");
			backButton.Click();
		}
	}
}