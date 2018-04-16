using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class KeepUsersForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator = By.XPath("//span[contains(text(), 'Which user list do you want to use')]");

		public KeepUsersForm(Guid driverId) : base(TitleLocator, "Keep users list form",driverId)
		{
            this.driverId = driverId;
            keepExistingLabel = new Label(By.XPath("//label[contains(@for, 'keepExisting')]"), "Keep existing label",driverId);
            keepExistingRadioButton = new RadioButton(By.Id("keepExisting"), "Keep existing radiobutton",driverId);
            uploadNewUsersLabel = new Label(By.XPath("//label[contains(@for, 'manual')]"), "Upload new users label",driverId);
            uploadNewUsersRadioButton = new RadioButton(By.Id("manual"), "Upload new users radiobutton",driverId);
        }

		private readonly Label keepExistingLabel ;
		private readonly RadioButton keepExistingRadioButton ;
		private readonly Label uploadNewUsersLabel ;
		private readonly RadioButton uploadNewUsersRadioButton ;

		public void SelectKeepExisting()
		{
			Log.Info("Selecting keep existing users");
			keepExistingLabel.Click();
			try
			{
				keepExistingRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				keepExistingLabel.Click();
			}
		}

		public void UploadNewUsers()
		{
			Log.Info("Uploading new users");
			uploadNewUsersLabel.Click();
			try
			{
				uploadNewUsersRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				uploadNewUsersLabel.Click();
			}
		}
	}
}
