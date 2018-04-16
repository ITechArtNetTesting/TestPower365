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

		public EnterPasswordForm(Guid driverId) : base(TitleLocator, "Enter password form",driverId)
		{
            this.driverId = driverId;
            passwordTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'textInput: password')]"), "Password textbox",driverId);
            confirmPasswordTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'textInput: confirmPassword')]"), "Confirm password textbox",driverId);
        }

		private readonly TextBox passwordTextBox ;
		private readonly TextBox confirmPasswordTextBox ;

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
