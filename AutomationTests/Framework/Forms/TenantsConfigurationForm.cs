using System;
using System.Threading;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class TenantsConfigurationForm : BaseForm
	{
		private static readonly By TitleLocator = By.XPath("//a[contains(@href, 'tenants')]");

		private readonly Button discoveryTabButton ;
		private readonly Button addressBooksTabButton ;
		private readonly Button expandedAddressBooksButton ;
		private readonly Button directorySyncButton ;
		private readonly Button expandedDirectorySyncButton ;
		private readonly Button expandedCalendarAvailabilityButton ;
		private readonly Button calenarAvailabilityButton ;
		private readonly Button expandedDiscoveryTabButton ;

		private readonly Button goToDashboardButton ;
	    protected Label descriptionLabel ;
        

        public TenantsConfigurationForm(Guid driverId) : base(TitleLocator, "Tenants configuration form",driverId)
		{
            this.driverId = driverId;
            discoveryTabButton = new Button(By.XPath("//ul[contains(@class, 'nav')]//a[contains(@href, 'discovery')]"),"Discovery tab button",driverId);
            addressBooksTabButton = new Button(By.XPath("//div[@id='tenantsManagementContainer']//a[contains(@href, 'addressBookSync')]"), "Address tab button",driverId);
            expandedAddressBooksButton = new Button(By.XPath("//a[contains(@href, 'addressBookSync')][contains(@aria-expanded, 'true')]"), "Expanded address books button",driverId);
            directorySyncButton = new Button(By.XPath("//div[@id='tenantsManagementContainer']//a[contains(@href, 'directorySync')]"), "Directory sync tab button",driverId);
            expandedDirectorySyncButton = new Button(By.XPath("//a[contains(@href, 'directorySync')][contains(@aria-expanded, 'true')]"), "Expanded directory sync button",driverId);
            expandedCalendarAvailabilityButton = new Button(By.XPath("//a[contains(@href, 'calendarAvailability')][contains(@aria-expanded, 'true')]"), "Expanded calendar availability button",driverId);
            calenarAvailabilityButton = new Button(By.XPath("//div[@id='tenantsManagementContainer']//a[contains(@href, 'calendarAvailability')]"), "Calendar availability button",driverId);
            expandedDiscoveryTabButton =new Button(By.XPath("//a[contains(@href, 'discovery')][contains(@aria-expanded, 'true')]"), "Expanded discovery tab",driverId);
            goToDashboardButton = new Button(By.XPath("//button[contains(@data-bind, 'goToDashboard')]"),"Go to dashboard button",driverId);
            descriptionLabel = new Label(By.XPath("//*[contains(@data-bind, 'projectDescription')]"), "Description Label",driverId);
            closeButton =new Button(By.XPath("//div[contains(@class, 'modal fade in')]//button[contains(text(), 'Close')]"),"Close popup button",driverId);
            discoverySwitherButton =new Button(By.XPath("//div[contains(@class, 'modal fade in')]//input[contains(@id, 'discoveryEnabled')]/.."),"Discovery switcher",driverId);
            runDiscoveryButton =new Button(By.XPath("//div[contains(@class, 'modal fade in')]//button[contains(@data-bind, 'startDiscovery')]"),"Run discovery button",driverId);
            descriptionLabel.WaitForElementPresent();
		}

		public void OpenAddressBooksTab()
		{
			Log.Info("Opening address books tab");
			addressBooksTabButton.Click();
			try
			{
				expandedAddressBooksButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Tab is not ready");
				addressBooksTabButton.Click();
			}
		}

		public void OpenDirectorySync()
		{
			Log.Info("Opening directory sync tab");
			directorySyncButton.Click();
			try
			{
				expandedDirectorySyncButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Tab is not ready");
				directorySyncButton.Click();
			}
		}

		public void OpenCalendarAvailabitily()
		{
			Log.Info("Opening Calendar availability");
			calenarAvailabilityButton.Click();
			try
			{
				expandedCalendarAvailabilityButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Tab is not ready");
				calenarAvailabilityButton.Click();
			}
		}

		public void OpenDiscoveryTab()
		{
			Log.Info("Opening discovery");
			discoveryTabButton.Click();
			try
			{
				expandedDiscoveryTabButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				discoveryTabButton.Click();
			}
		}

		public void GoToDashboard()
		{
			Log.Info("Going to dashboard");
			goToDashboardButton.Click();
		}
		
		public void RunDiscovery(string tenant)
		{
			Log.Info("Running discovery");
            //BaseElement.WaitForElementPresent(By.XPath($"//div[@id='discovery']//*[contains(text(), '{tenant}')]"), "");
            var tenantLabelButton = new Button(By.XPath($"//div[@id='discovery']//*[contains(text(), '{tenant}')]"),
                tenant + " label",driverId);
            new Element(By.XPath($"//div[@id='discovery']//*[contains(text(), '{tenant}')]"), "",driverId).WaitForElementPresent();            
            tenantLabelButton.Move();
            //BaseElement.WaitForElementPresent(By.XPath($"//*[contains(text(), '{tenant}')]/ancestor::tr//a[contains(@data-bind, 'startDiscovery')]"), "");
            new Element(By.XPath($"//*[contains(text(), '{tenant}')]/ancestor::tr//a[contains(@data-bind, 'startDiscovery')]"), "", driverId).WaitForElementPresent();
            //BaseElement.WaitForElementIsClickable(By.XPath($"//*[contains(text(), '{tenant}')]/ancestor::tr//a[contains(@data-bind, 'startDiscovery')]"));
            new Element(By.XPath($"//*[contains(text(), '{tenant}')]/ancestor::tr//a[contains(@data-bind, 'startDiscovery')]"), "", driverId).WaitForElementIsClickable();
			var runDiscoveryButton =
				new Button(By.XPath($"//*[contains(text(), '{tenant}')]/ancestor::tr//a[contains(@data-bind, 'startDiscovery')]"),
					tenant + " settings button",driverId);
			try
			{
				runDiscoveryButton.Click();
			}
			catch (Exception)
			{
				Log.Info("Run discovery button does not appear");
				discoveryTabButton.Move();
				Thread.Sleep(1000);	
				tenantLabelButton.Move();
			}
			try
			{
				Log.Info("Waiting till progress start");
				Label progressSpinerLabel = new Label(By.XPath($"//*[contains(text(), '{tenant}')]/ancestor::tr//i[contains(@class, 'fa-spinner')] | //*[contains(text(), '{tenant}')]/ancestor::tr//i//div[contains(@class, 'sk-spinner')]"), "Progress spinner for: "+tenant,driverId);
				progressSpinerLabel.WaitForElementPresent(60000);
			}
			catch (Exception)
			{
				runDiscoveryButton.Click();
			}
		}

		public void DownloadLogs(string tenant)
		{
			Log.Info("Downloading logs for: " + tenant);
			var tenantLogsButton =
				new Button(
					By.XPath($"//*[contains(text(), '{tenant}')]/ancestor::tr//a[contains(@data-bind, 'exportTenantLogs')]"),
					tenant + " logs button",driverId);
			var tenantLabelButton = new Button(By.XPath($"//div[@id='discovery']//*[contains(text(), '{tenant}')]"),
				tenant + " label",driverId);
			tenantLabelButton.Move();
			try
			{
				tenantLogsButton.WaitForElementIsVisible();
			}
			catch (Exception)
			{
				discoveryTabButton.Move();
				Thread.Sleep(1000);
				tenantLabelButton.Move();
			}
			tenantLogsButton.Click();
		}
		//Obsolete?
		#region [Settings popup]

		private readonly Button closeButton ;

		private readonly Button discoverySwitherButton ;

		private readonly Button runDiscoveryButton ;
		public void SwitchDiscovery()
		{
			Log.Info("Switching discovery");
			discoverySwitherButton.Click();
		}

		public void CloseSettings()
		{
			Log.Info("Close settings popup");
			closeButton.Click();
		}

		#endregion
	}
}