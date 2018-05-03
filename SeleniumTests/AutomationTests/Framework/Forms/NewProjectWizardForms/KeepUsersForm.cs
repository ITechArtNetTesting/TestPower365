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

		public KeepUsersForm() : base(TitleLocator, "Keep users list form")
		{
		}

		private readonly Label keepExistingLabel = new Label(By.XPath("//label[contains(@for, 'keepExisting')]"), "Keep existing label");
		private readonly RadioButton keepExistingRadioButton = new RadioButton(By.Id("keepExisting"), "Keep existing radiobutton");
		private readonly Label uploadNewUsersLabel = new Label(By.XPath("//label[contains(@for, 'manual')]"), "Upload new users label");
		private readonly RadioButton uploadNewUsersRadioButton = new RadioButton(By.Id("manual"), "Upload new users radiobutton");

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
