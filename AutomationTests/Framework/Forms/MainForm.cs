using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class MainForm : BaseForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//a[contains(@href, 'SignIn')][contains(@class, 'btn')]");
        
        private readonly Button registerButton;

		private readonly Button signInButton ;
        private readonly Button languageSelectorButton ;
        private readonly Button expandedLanguageSelectorButton ;
        private readonly Button englishLanguageListOptionButton ;
        private readonly Button currentlySelectedLanguageButton;
        
		public MainForm(Guid driverId) : base(TitleLocator, "Main Form")
		{
            this.driverId = driverId;
            registerButton = new Button(By.XPath("//a[contains(@href, 'Register')][contains(@class, 'btn')]"), "Register button", driverId);
            signInButton = new Button(By.XPath("//a[contains(@href, 'SignIn')][contains(@class, 'btn')]"), "Sign In button",driverId);
            languageSelectorButton=new Button(By.Id("language-selector"), "Language selector button",driverId);
            expandedLanguageSelectorButton= new Button(By.XPath("//div[contains(@id, 'language-selector') and contains(@aria-expanded, 'true')]"), "Expanded language selector",driverId);
            englishLanguageListOptionButton= new Button(By.XPath("//div[contains(@class, 'open')]//a[contains(@data-bind, 'parent.languageSelected')]//*[contains(text(),'en')]"), "EN list option",driverId);
            currentlySelectedLanguageButton= new Button(By.XPath("//div[@id='language-selector']//span[contains(text(),'en')]"), "Currently selected english",driverId);
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