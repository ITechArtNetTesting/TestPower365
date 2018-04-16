using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms
{
	public class CmtLoginForm : BaseForm
	{
		private static readonly By TitleLocator = By.Id("ctl00_PageTitleLabel");

		private readonly TextBox loginTextBox ;

		private readonly TextBox passwordTextBox ;

		private readonly Button signInButton ;
        

        public CmtLoginForm(Guid driverId) : base(TitleLocator, "CMT login form",driverId)
		{
            this.driverId = driverId;
            loginTextBox = new TextBox(By.Id("ctl00_ContentPlaceHolder1_UsernameTextBox"),"Login textbox",driverId);
            passwordTextBox = new TextBox(By.Id("ctl00_ContentPlaceHolder1_PasswordTextBox"),"Password textbox",driverId);
            signInButton = new Button(By.Id("ctl00_ContentPlaceHolder1_SubmitButton"), "Sign in button",driverId);
        }

		public void SetLogin(string login)
		{
			Log.Info($"Setting {login} login");
			loginTextBox.ClearSetText(login);
		}

		public void SetPassword(string password)
		{
			Log.Info($"Setting {password} password");
			passwordTextBox.ClearSetText(password);
		}

		public void SignIn()
		{
			Log.Info("Clicking SignIn button");
			signInButton.Click();
		}
	}
}