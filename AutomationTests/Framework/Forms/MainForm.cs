using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class MainForm : BaseForm
	{
		private static readonly By TitleLocator = By.XPath("//a[contains(@href, 'SignIn')][contains(@class, 'btn')]");
        
        private readonly Button registerButton =
			new Button(By.XPath("//a[contains(@href, 'Register')][contains(@class, 'btn')]"), "Register button");

		private readonly Button signInButton = new Button(
			By.XPath("//a[contains(@href, 'SignIn')][contains(@class, 'btn')]"), "Sign In button");
        private readonly Button languageSelectorButton = new Button(By.Id("language-selector"), "Language selector button");
        private readonly Button expandedLanguageSelectorButton = new Button(By.XPath("//div[contains(@id, 'language-selector') and contains(@aria-expanded, 'true')]"), "Expanded language selector");
        private readonly Button englishLanguageListOptionButton = new Button(By.XPath("//div[contains(@class, 'open')]//a[contains(@data-bind, 'parent.languageSelected')]//*[contains(text(),'en')]"), "EN list option");
        private readonly Button currentlySelectedLanguageButton = new Button(By.XPath("//div[@id='language-selector']//span[contains(text(),'en')]"), "Currently selected english");
        
		public MainForm() : base(TitleLocator, "Main Form")
		{
		}

		internal void SetClientName(string v)
		{
			throw new NotImplementedException();
        }

	    public void OpenLanguageSelector()
	    {
            Log.Info("Opening language selector");
            languageSelectorButton.Click();
	        try
	        {
                expandedLanguageSelectorButton.WaitForElementPresent(5000);
	        }
	        catch (Exception)
	        {
	            languageSelectorButton.Click();
            }
	    }

	    public bool IsDefaultEnglish()
	    {
            Log.Info("Is default english");
	        return currentlySelectedLanguageButton.IsPresent();
	    }

	    public void SelectEnglishLanguage()
	    {
            Log.Info("Selecting english language");
            englishLanguageListOptionButton.Click();
	    }

	    public void ClickSignIn()
		{
            Log.Info("Clicking P365 Signin");
            signInButton.Click();
        }

		public void ClickRegister()
		{
			registerButton.Click();
		}
	}
}