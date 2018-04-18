using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class EnterPasswordForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Enter a password')]");

		public EnterPasswordForm() : base(TitleLocator, "Enter password form")
		{
		}

		private readonly TextBox passwordTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'textInput: password')]"), "Password textbox");
		private readonly TextBox confirmPasswordTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'textInput: confirmPassword')]"), "Confirm password textbox");

		public void SetPassword(string password)
		{
			Log.Info("Setting password: "+password);
			passwordTextBox.ClearSetText(password);
		}

		public void SetConfirmPassword(string password)
		{
			Log.Info("Setting confirm password");
			confirmPasswordTextBox.ClearSetText(password);
		}
	}
}
