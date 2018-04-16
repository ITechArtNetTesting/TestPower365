using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class Office365LoginForm : BaseForm
	{
        private static readonly By TitleLocator = By.XPath("//*/title");

        private readonly TextBox loginTextBox ;

        private readonly TextBox passwordTextBox ;

        private readonly Button nextButton ;

        private readonly Button useAnotherAccountButton ;

        private readonly Button dontShowAgain ;
        


        /// <summary>
        ///     Initializes a new instance of the <see cref="Office365LoginForm" /> class.
        /// </summary>
        public Office365LoginForm(Guid driverId) 
            : base(TitleLocator, "Office365 login form",driverId)
		{
            this.driverId = driverId;
            loginTextBox = new TextBox(By.Id("i0116"), "Login textbox",driverId);
            passwordTextBox = new TextBox(By.Name("passwd"), "Password textbox",driverId);
            nextButton = new Button(By.Id("idSIButton9"), "Next button",driverId);
            useAnotherAccountButton = new Button(By.Id("otherTile"), "Use another account button",driverId);
            dontShowAgain = new Button(By.Name("DontShowAgain"), "Don't show again button",driverId);
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
            AccertClick();
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

        public void AccertClick()
        {
           Log.Info($"Clicking Next");
           nextButton.WaitForElementPresent();
           nextButton.Click();
                     
        }
    }
}