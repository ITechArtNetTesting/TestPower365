using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class TenantRestructuringForm : BaseForm
	{
		private static readonly By TitleLocator = By.XPath("//*[contains(@id, 'projectsContainer')] | //video");

		private readonly Button addProjectButton ;
        private readonly Button addProjectBottomButton ;

		private readonly Button clientRoleButton ;

		private readonly TextBox clientRoleTextBox ;

		private readonly Label fakeLabel ;

		private readonly Button mailFromFileButton ;

		private readonly Button mailWithDiscovery ;

		private readonly Button moreButton ;

		private readonly Button profileButton ;

		private readonly Button roleDropdownButton ;

		private readonly Button signOutButton ;
        private readonly Label projectsContainerLabel ;
        

        private Button logoButton ;
		public TenantRestructuringForm(Guid driverId) : base(TitleLocator, "Tenant Restructuring Form",driverId)
		{
            this.driverId = driverId;
            addProjectButton =new Button(By.XPath("//div[contains(@class, 'topnav')]//a[contains(@href, 'Project/Create')]"),"Add project button",driverId);
            addProjectBottomButton = new Button(By.XPath("//a[contains(@href, 'Project/Create')]"), "Bottom add project button",driverId);
            clientRoleButton = new Button(By.XPath("//a[@role='button']//i[contains(@class, 'fa-users')]"),"Client role button",driverId);
            clientRoleTextBox = new TextBox(By.XPath("//div[contains(@class, 'dropdown inline m-r')]"),"Client role textbox",driverId);
            fakeLabel = new Label(By.XPath("//span[@class='m-r'][text()='1']"), "Fake 1 label",driverId);
            mailFromFileButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'addMailOnlyProject')]"),"Mail only from file button",driverId);
            mailWithDiscovery =new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'addMailWithDiscoveryProject')]"),"Mail with discovery button",driverId);
            moreButton =new Button(By.XPath("//*[@id='recentClientsPane']//div[contains(@role, 'tooltip')]//*[@id='moreClientsLink']"),"More button",driverId);
            profileButton = new Button(By.XPath("//i[contains(@class, 'glyphicon-user')]/.."),"Profile button",driverId);
            roleDropdownButton =new Button(By.XPath("//div[contains(@class, 'dropdown inline m-r')]"), "Role dropdown",driverId);
            signOutButton =new Button(By.XPath("//div[contains(@class, 'popover-content')]//a[contains(@href, 'SignOut')]"), "Sign out button",driverId);
            projectsContainerLabel = new Label(By.XPath("//*[contains(@id, 'projectsContainer')] | //video"), "Projects container",driverId);
            logoButton = new Button(By.XPath("//ul[contains(@class, 'topnav-menu')]"), "Logo button",driverId);
        }

		public void AddProjectClick()
		{
            WaitForDOM();
            Log.Info("Clicking Add Project button");

		    if (addProjectButton.IsPresent())
		    {
		        addProjectButton.Click();
            }
		    else
		    {
		        addProjectBottomButton.Click();
		    }
		}

	    public void GoToProjects()
	    {
            Log.Info("Going to projects");
         
	        try
	        {
	            logoButton.Click();
            }
	        catch (Exception)
	        {
	            logoButton = new Button(By.XPath("//ul[contains(@class, 'topnav-menu')]"), "Logo button",driverId);
	            logoButton.Click();
            }
           
	    }

	    public void OpenProfile()
		{
			Log.Info("Opening profile");
			profileButton.Click();
			try
			{
				signOutButton.WaitForElementPresent(7000);
			}
			catch (Exception)
			{
				Log.Info("Profile not ready");
				profileButton.Click();
			}
		}

	    public void WaitForProjectsContainer()
	    {
            Log.Info("Waitong till projects container");
            projectsContainerLabel.WaitForElementPresent();
	    }

	    public void SignOut()
		{
			Log.Info("Signing out");
			signOutButton.Click();
		}

		public void SelectProject(string projectName)
		{
			Log.Info("Selecting project: " + projectName);
			var projectButton = new Button(By.XPath($"//span[contains(text(), '{projectName}')][contains(@class, 'notranslate')]"), projectName + " button",driverId);
			projectButton.Click();
		}

		public void SelectMailFromFile()
		{
			Log.Info("Selecting Mail only from file option");
			mailFromFileButton.Click();
		}

		public void SelectMailWithDiscovery()
		{
			Log.Info("Selecting Mail with discovery");
			mailWithDiscovery.Click();
		}

		public void OpenProject(string name)
		{
			Log.Info($"Opening {name} project");
			var projectButton = new Button(By.XPath($"//a[contains(text(), '{name}')]"), $"{name} project button",driverId);
			projectButton.Click();
			try
			{
				projectButton.WaitForElementDisappear(10000);
			}
			catch (Exception)
			{
				Log.Info("Project button is not ready");
				projectButton.Click();
			}
		}

		public void OpenClientsConteiner()
		{
			Log.Info("Opening clients container");
			clientRoleButton.Click();
		}

		public void OpenMoreInput()
		{
			Log.Info("Opening More input");
			moreButton.Click();
		}

		public void SetRoleByTyping(string role)
		{
			Log.Info($"Setting {role} role");
			clientRoleTextBox.WaitForElementIsVisible();
			fakeLabel.WaitForElementDisappear();
			if (!CheckRole(role))
			{
				clientRoleTextBox.ClearSetText(role);
				clientRoleTextBox.PressEnter();
				clientRoleButton.WaitForElementPresent();
				clientRoleButton.WaitForElementIsVisible();
			}
		}

		public void ScrollToElement(IWebElement element)
		{
            //((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView();", element);
            ((IJavaScriptExecutor)Driver.GetDriver(driverId)).ExecuteScript("arguments[0].scrollIntoView();", element);
        }

		public void SetRoleByDropDown(string role)
		{
			Log.Info($"Setting {role} role");
			clientRoleTextBox.WaitForElementIsVisible();
			fakeLabel.WaitForElementDisappear();
			roleDropdownButton.Click();
			Thread.Sleep(3000);
			var dropDownSelectButton =
				new Button(
					By.XPath($"//button[contains(@aria-expanded, 'true')]/..//a[contains(text(), '{role}')]"),
					$"{role} dropdown option",driverId);
			try
			{
				ScrollToElement(dropDownSelectButton.GetElement());
				dropDownSelectButton.WaitForElementIsClickable();
			}
			catch (Exception)
			{
				Log.Info("DROP DOWN IS NOT EXPANDED");
				roleDropdownButton.Click();
				dropDownSelectButton.WaitForElementIsClickable();
			}
			ScrollToElement(dropDownSelectButton.GetElement());
			dropDownSelectButton.Click();
			clientRoleButton.WaitForElementPresent();
			clientRoleButton.WaitForElementIsVisible();
		}

		private bool CheckRole(string role)
		{
			var roleLabel = new Label(By.XPath($"//span[@class='m-r'][text()='{role}']"), "Current role label",driverId);
			return roleLabel.IsPresent();
		}

		//Used for Assertion on New Client Registration
		public void AssertNoDataForNewClient()
		{
			var NoDataElement = new Element(By.XPath("//span[contains(text(), 'No projects')]"), "No Data Element",driverId);
			NoDataElement.WaitForElementPresent();
			Assert.IsTrue(NoDataElement.IsPresent());
		}
	}
}