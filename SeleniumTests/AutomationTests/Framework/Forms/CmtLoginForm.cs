using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class CmtLoginForm : BaseForm
	{
		private static readonly By TitleLocator = By.Id("ctl00_PageTitleLabel");

		private readonly TextBox loginTextBox = new TextBox(By.Id("ctl00_ContentPlaceHolder1_UsernameTextBox"),
			"Login textbox");

		private readonly TextBox passwordTextBox = new TextBox(By.Id("ctl00_ContentPlaceHolder1_PasswordTextBox"),
			"Password textbox");

		private readonly Button signInButton = new Button(By.Id("ctl00_ContentPlaceHolder1_SubmitButton"), "Sign in button");

		public CmtLoginForm() : base(TitleLocator, "CMT login form")
		{
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