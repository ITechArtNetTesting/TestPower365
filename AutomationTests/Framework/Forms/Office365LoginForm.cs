using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class Office365LoginForm : BaseForm
	{
        private static readonly By TitleLocator = By.Name("f1");
       
        private readonly TextBox loginTextBox = new TextBox(By.Name("loginfmt"), "Login textbox");
        private readonly TextBox passwordTextBox = new TextBox(By.Name("passwd"), "Password textbox");

        private readonly Button nextButton = new Button(By.ClassName("btn-primary"), "Next button");

        private readonly Button useAnotherAccountButton = new Button(By.Id("otherTile"), "Use another account button");

        private readonly Button dontShowAgain = new Button(By.Name("DontShowAgain"), "Don't show again button");

        /// <summary>
        ///     Initializes a new instance of the <see cref="Office365LoginForm" /> class.
        /// </summary>
        public Office365LoginForm() 
            : base(TitleLocator, "Office365 login form")
		{
		}
        
		public void SetLogin(string login)
		{
            Log.Info("Clicking user");
            loginTextBox.WaitForElementPresent();
            loginTextBox.Click();
            Log.Info($"Setting {login} login");
            loginTextBox.ClearSetText(login);
        }

		public void SetPassword(string password)
		{
            Log.Info("Clicking password");
            passwordTextBox.WaitForElementPresent();
            passwordTextBox.Click();
            Log.Info($"Setting {password} password");
            passwordTextBox.ClearSetText(password);
        }

        public void UseAnotherAccountClick()
        {
            Log.Info("Clicking use another account");
            useAnotherAccountButton.WaitForElementPresent();
            useAnotherAccountButton.Click();
        }

        public void NextClick()
        {
            Log.Info($"Clicking Next");
            nextButton.WaitForElementPresent();
            nextButton.Click();

            try
            {
                //Handle Don't Show Again page.
                dontShowAgain.WaitForElementPresent(1000);
                nextButton.Click();
            }
            catch (Exception)
            {
                //Ignore Timeout
            }
        }
	}
}