using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	/// <summary>
	///     Class BaseForm.
	/// </summary>
	/// <seealso cref="BaseEntity" />
	public class BaseForm : BaseEntity
	{        
       
        //NOTE: CHECK 1.10 version
		private readonly Button allProjectsButton ;

		private readonly Button favoritesButton ;

		private readonly Button homeButton ;

	    private readonly Button backToDashboardButton;

        /// <summary>
        ///     The locator
        /// </summary>
        private readonly By locator;

		private readonly Button menuButton ;

		/// <summary>
		///     The name
		/// </summary>
		private readonly string name;

		private readonly Button tenantRestruncuringButton ;

		/// <summary>
		///     Initializes a new instance of the <see cref="BaseForm" /> class.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <param name="name">The name.</param>
		protected BaseForm(By locator, string name, Guid driverId)
		{
            this.driverId = driverId;
			this.locator = locator;
			this.name = name;
            allProjectsButton = new Button(By.XPath("//*[@id='actionbar']//a[contains(@href, 'Project/List')]"),"All projects button",driverId);
            favoritesButton =new Button(By.XPath("//*[@id='projectToolbar']//i[contains(@class, 'fa-star-o')]/.."), "Favorites button",driverId);
            homeButton = new Button(By.XPath(".//*[@id='breadcrumbsContainer']//a[text()='Home']"),"Home button",driverId);
            backToDashboardButton = new Button(By.XPath("//button[contains(@data-bind, 'goToDashboard')]"), "Back to dashboard button",driverId);
            menuButton = new Button(By.Id("hamburger"), "Main menu button",driverId);
            tenantRestruncuringButton =new Button(By.XPath("//div[@class='side-menu']//a[contains(@href, 'TenantRestructuring')]"),"Tenant restructuring button",driverId);
            addLinkButton =new Button(By.XPath("//div[contains(@class, 'popover-content')]//i[contains(@class, 'fa-plus')]/.."),"Add link button",driverId);
            favoriteNameTextBox =new TextBox(By.XPath("//div[contains(@class, 'popover-content')]//input[contains(@data-bind, 'description')]"),"Favorite link name",driverId);
            okButton =new Button(By.XPath("//div[contains(@class, 'popover-content')]//div[contains(@class, 'panel-footer')]//a[contains(text(), 'OK')]"),"OK button",driverId);
            editLinksButton =new Button(By.XPath("//div[contains(@class, 'popover-content')]//a[contains(@data-bind, 'openEditLinks')]"),"Edit links button",driverId);
            closeEditLinksDialogButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(text(), 'Close')]"),"Close edit links dialog",driverId);
            AssertIsPresent();
		}

		/// <summary>
		///     Asserts the is present.
		/// </summary>
		private void AssertIsPresent()
		{
            //BaseElement.WaitForElementPresent(locator, "Form " + name);
            new Element(locator, "Form " + name, driverId).WaitForElementPresent();
			Log.Info($"Form '{name}' is ready:");
		}

	    public void GoToClientRegistration()
	    {
            Log.Info("Navigating to client registration");
            Driver.GetDriver(driverId).Navigate().GoToUrl(RunConfigurator.GetValue("baseurl")+ "Account/Register");
	    }

	    /// <summary>
		///     Checks the text on form.
		/// </summary>
		/// <param name="text">The text.</param>
		public void CheckTextOnForm(string text)
		{
            //BaseElement.WaitForElementPresent(By.XPath("//*[contains(.,'" + text + "')]"), text);
            new Element(By.XPath("//*[contains(.,'" + text + "')]"), text, driverId).WaitForElementPresent();
			Log.Info($"Text '{text}' is shown on the page");
		}

		public void ScrollToTheBottom()
		{
			Log.Info("Scrolling to the bottom of the page");
			((IJavaScriptExecutor) Driver.GetDriver(driverId)).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 10)");
		}

		public void ScrollToTop()
		{
			Log.Info("Scrolling to the bottom of the page");
			((IJavaScriptExecutor) Driver.GetDriver(driverId)).ExecuteScript("window.scrollTo(0, 0)");
		}

		/// <summary>
		///     Elements the is present.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="name">The name.</param>
		private void ElementIsPresent(By element, string name)
		{
            //BaseElement.WaitForElementPresent(element, "Form " + name);
            new Element(element, "Form " + name, driverId).WaitForElementPresent();
			Log.Info($"Form '{name}' has appeared");
		}

		/// <summary>
		///     Gets the count.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>System.String.</returns>
		public void GoToMainPage()
		{
			Driver.GetDriver(driverId).Navigate().GoToUrl(RunConfigurator.GetValue("baseurl"));
		}

		/// <summary>
		///     Asserts the text is present.
		/// </summary>
		/// <param name="text">The text.</param>
		public void AssertTextIsPresent(string text)
		{
            //BaseElement.WaitForElementPresent(By.XPath("//*[contains(text(),'" + text + "')]"), "");
            new Element(By.XPath("//*[contains(text(),'" + text + "')]"), "", driverId).WaitForElementPresent();
			var elements = Driver.GetDriver(driverId).FindElements(By.XPath("//*[contains(text(),'" + text + "')]"));
			Assert.IsTrue(elements.Count > 0);
		}

	    public void GoToProjectOverview()
	    {
            Log.Info("Going to project overview");
            backToDashboardButton.Click();
	    }

	    public void SwitchToMainWindow()
		{
			Driver.GetDriver(driverId).SwitchTo().Window(Store.MainHandle);
		}

		public void OpenMainMenu()
		{
			Log.Info("Opening main menu");
            WaitForDOM();
            WaitForAjaxLoad();          
            menuButton.Click();
         
		}

		public void OpenTenantRestructuring()
		{
			Log.Info("Opening Tenant restructuring");
			ScrollToTop();
			OpenMainMenu();
			allProjectsButton.Click();
		}



		public void OpenFavorites()
		{
			Log.Info("Opening Favorites");
			try
			{
				favoritesButton.Click();
				addLinkButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Favorites dialog is not ready");
				favoritesButton.Click();
			}

			//Thread.Sleep(2000);
		}

		/// <summary>
		///     Ats the main menu.
		/// </summary>
		/// <returns>MainMenu.</returns>
		public MainMenu AtMainMenu(Guid driverId)
		{
			return new MainMenu(driverId);
		}

		#region[Menu]

		public class MainMenu
		{
            private Guid driverId;

            public MainMenu(Guid driverId)
            {
                this.driverId = driverId;
                allProjectsButton = new Button(By.XPath("//ul[@id='actionbar']//a[contains(text(), 'All Projects')]"), "All projects button", driverId);
                licensingButton = new Button(By.XPath("//a[contains(@href, 'Licensing')]"), "Licensing button", driverId);
                languageListButton = new Button(By.Id("language-list"), "Language list button",driverId);
                expandedDropdownLabel = new Label(By.XPath("//button[contains(@class, 'dropdown-toggle') and contains(@aria-expanded, 'true')]"), "Expanded dropdown",driverId);
                clientDropdownButton = new Button(By.XPath("//li[.//input[contains(@id, 'set-client-url')]]//button[contains(@class, 'dropdown')]"), "Client dropdown button",driverId);
                profilesButton = new Button(By.XPath("//div[@id='actionbar' and not(contains(@class, 'hidden'))]//*[contains(@class, 'menulink')]//*[contains(text(), 'Migration Profiles')]"), "Profiles button",driverId);
            }

			private readonly Button allProjectsButton ;
			private readonly Button licensingButton ;
		    private readonly Button languageListButton ;
		    private readonly Label expandedDropdownLabel ;
		    private const string LanguageLocator = "//div[contains(@id, 'language-list')]//a[contains(text(), '{0}')]";
		    private const string ClientLocator = "//div[contains(@class, 'open')]//a[contains(text(), '{0}')]";
            private const string CurrentLanguageLocator = "//*[contains(@data-bind, 'selectedLanguage') and contains(text(), '{0}')]";
            private readonly Button clientDropdownButton ;
            private Button profilesButton ;
            public void OpenLicensing()
			{
				Log.Info("Opening licensing");
				licensingButton.Click();
			}

		    public void OpenProfiles()
		    {
                Log.Info("Opening profiles");
                profilesButton.Click();
		    }

		    public void SelectRole(string role)
			{
				Log.Info("Selecting role: " + role);
			    clientDropdownButton.Click();
			    try
			     {
                   expandedDropdownLabel.WaitForElementPresent(7000);
			     }
			    catch (Exception)
			     {
			       clientDropdownButton.Click();
                 }
			     new Button(By.XPath(String.Format(ClientLocator, role)), role + " button",driverId).Click();
			}

		    public void SelectLanguage(string language)
		    {
		        if (!new Label(By.XPath(String.Format(CurrentLanguageLocator, language)), "Current language locator",driverId).IsPresent())
		        {
		            languageListButton.Click();
		            try
		            {
		                expandedDropdownLabel.WaitForElementPresent(7000);
		            }
		            catch (Exception)
		            {
		                languageListButton.Click();
		            }
		            new Button(By.XPath(String.Format(LanguageLocator, language)), language + " language",driverId).Click();
                }
		    }

		    public void GoToAllProjects()
			{
				Log.Info("Going to all projects");
				allProjectsButton.Click();
			}
		}

        #endregion

        public void WaitForAjaxLoad()
        {
            if (jQueryExists())
            {
                var wait = new WebDriverWait(Driver.GetDriver(driverId), TimeSpan.FromMinutes(1));
                wait.PollingInterval = TimeSpan.FromMilliseconds(100);
                wait.Until(wd => (bool)(Driver.GetDriver(driverId) as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
            }
        }
        public bool jQueryExists()
        {
            try
            {
                (Driver.GetDriver(driverId) as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void WaitForDOM()
        {
            var wait = new WebDriverWait(Driver.GetDriver(driverId),
                TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
            try
            {
                wait.Until(waiting =>
                {
                    try
                    {
                        return
                            ((IJavaScriptExecutor)Driver.GetDriver(driverId)).ExecuteScript("return document.readyState")
                                .Equals("complete");
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            }
            catch (TimeoutException)
            {
            }
        }

        #region [Favorites]

        private readonly Button addLinkButton ;

		private readonly TextBox favoriteNameTextBox ;

		private readonly Button okButton ;

		private readonly Button editLinksButton ;

		public void OpenAddLinkDialog()
		{
			Log.Info("Opening add link dialog");
			addLinkButton.Click();
		}

		public void OpenLink(string linkName)
		{
			Log.Info("Opening link: " + linkName);
			var linkButton =
				new Button(By.XPath($"//div[contains(@class, 'popover-content')]//a[contains(text(), '{linkName}')]"),
					"Link " + linkName,driverId);
			var je = (IJavaScriptExecutor) Driver.GetDriver(driverId);
			je.ExecuteScript("arguments[0].scrollIntoView(true);", linkButton.GetElement());
			linkButton.Click();
		}

		public void OpenEditLinksDialog()
		{
			Log.Info("Opening Edit links dialog");
			editLinksButton.Click();
		}

		public void SetLinkName(string name)
		{
			Log.Info("Setting link name to: " + name);
			favoriteNameTextBox.ClearSetText(name);
			Store.LinkName = name;
		}

		public void SaveLinkChanges()
		{
			Log.Info("Saving changes");
			okButton.Click();
		}

		public void AssertLinkIsPresent(string linkName)
		{
			Log.Info("Asserting link is present: " + linkName);
			var linkButton =
				new Button(By.XPath($"//div[contains(@class, 'popover-content')]//a[contains(text(), '{linkName}')]"),
					"Link " + linkName,driverId);
			linkButton.WaitForElementPresent();
		}

		public void AssertLinkIsAbsent(string linkName)
		{
			Log.Info("Asserting link is absent: " + linkName);
			var linkButton =
				new Button(By.XPath($"//div[contains(@class, 'popover-content')]//a[contains(text(), '{linkName}')]"),
					"Link " + linkName,driverId);
			linkButton.WaitForElementDisappear();
		}

		#endregion

		#region [Edit Links dialog]

		private readonly Button closeEditLinksDialogButton;

		public void CloseEditLinksDialog()
		{
			Log.Info("Closing edit links dialog");
			closeEditLinksDialogButton.Click();
			addLinkButton.WaitForElementDisappear();
		}

		public void RemoveLink(string linkName)
		{
			Log.Info("Removing link: " + linkName);
			var links =
				Driver.GetDriver(driverId)
					.FindElements(
						By.XPath(
							"//div[contains(@class, 'modal-dialog')]//div[contains(@class, 'slimScrollDiv')]//li"));
			foreach (var link in links)
			{
				if (
					link.FindElement(By.XPath(".//input[contains(@data-bind, 'description')]")).GetAttribute("value").Trim().ToLower() ==
					linkName.Trim().ToLower())
				{
					var je = (IJavaScriptExecutor) Driver.GetDriver(driverId);
					je.ExecuteScript("arguments[0].scrollIntoView(true);", link.FindElement(By.XPath(".//button")));
					link.FindElement(By.XPath(".//button")).Click();
				}
			}
		}

		public void ReorderLink(string name, string url)
		{
			Log.Info($"Reordering link {name} to {url} url");
			var links =
				Driver.GetDriver(driverId)
					.FindElements(
						By.XPath(
							"//div[contains(@class, 'modal-dialog')]//div[contains(@class, 'slimScrollDiv')]//li"));
			foreach (var link in links)
			{
				if (
					link.FindElement(By.XPath(".//input[contains(@data-bind, 'description')]")).GetAttribute("value").Trim().ToLower() ==
					name.Trim().ToLower())
				{
					var je = (IJavaScriptExecutor) Driver.GetDriver(driverId);
					je.ExecuteScript("arguments[0].scrollIntoView(true);",
						link.FindElement(By.XPath(".//input[contains(@data-bind, 'url')]")));
					link.FindElement(By.XPath(".//input[contains(@data-bind, 'url')]")).Clear();
					link.FindElement(By.XPath(".//input[contains(@data-bind, 'url')]")).SendKeys(url);
					Store.LinkUrl = url;
				}
			}
		}

		public void RenameLink(string oldName, string newName)
		{
			Log.Info($"Renaming link {oldName} to {newName}");
			var links =
				Driver.GetDriver(driverId)
					.FindElements(
						By.XPath(
							"//div[contains(@class, 'modal-dialog')]//div[contains(@class, 'slimScrollDiv')]//li"));
			foreach (var link in links)
			{
				if (
					link.FindElement(By.XPath(".//input[contains(@data-bind, 'description')]")).GetAttribute("value").Trim().ToLower() ==
					oldName.Trim().ToLower())
				{
					var je = (IJavaScriptExecutor) Driver.GetDriver(driverId);
					je.ExecuteScript("arguments[0].scrollIntoView(true);",
						link.FindElement(By.XPath(".//input[contains(@data-bind, 'description')]")));
					link.FindElement(By.XPath(".//input[contains(@data-bind, 'description')]")).Clear();
					link.FindElement(By.XPath(".//input[contains(@data-bind, 'description')]")).SendKeys(newName);
					Store.LinkName = newName;
				}
			}
		}

        		public void AssertLinkIsReordered(string name, string url)
		{
			Log.Info($"Asserting link {name} is reorderd to {url}");
			var links =
				Driver.GetDriver(driverId)
					.FindElements(
						By.XPath(
							"//div[contains(@class, 'modal-dialog')]//div[contains(@class, 'slimScrollDiv')]//li"));
			foreach (var link in links)
			{
				if (
					link.FindElement(By.XPath(".//input[contains(@data-bind, 'description')]")).GetAttribute("value").Trim().ToLower() ==
					name.Trim().ToLower())
				{
					var value = link.FindElement(By.XPath(".//input[contains(@data-bind, 'url')]")).GetAttribute("value");
					Assert.IsTrue(value.Trim().ToLower().Contains(url.Trim().ToLower()), "Url is not reordered");
				}
			}
		}

		#endregion
	}
}