using System;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class ProjectOverviewForm : BaseForm
	{
		private static readonly By TitleLocator = By.XPath("//a[contains(@href, 'Edit?project')]");

		private readonly Button allUsersAmountButton =
			new Button(
				By.XPath("//div[contains(@class, 'ibox-content')]/a[contains(@data-bind, 'total,')]"),
				"All users amount button");

		private readonly Button allUsersCompletedButton =
			new Button(
				By.XPath(
					"//*[contains(text(), 'users')]/ancestor::div[contains(@data-bind, 'overallStatusViewModel')]//a[contains(@data-bind, 'completedNumber')]"),
				"All users completed button");

		private readonly Button archiveProjectButton = new Button(
			By.XPath("//button[contains(@data-bind, 'deleteProject')]"), "Archive project button");

		private readonly Label connectionStatusLabel =
			new Label(
				By.XPath(
                    "//div[@class='page-content']/div[@class='ibox'][2]/div[@class='ibox-content']/div[@class='row']/div[@class='col-md-3 col-sm-6 col-xs-6'][1]/h3[@class='blue-text']/font/font[text()='Connected']"),
                "Connection status label");

		private readonly Label discoveryCompleteStateLabel =
			new Label(By.XPath("//i[contains(@data-bind, 'discoveryState')][contains(@class, 'icon-success')][contains(@class, 'fa-check-circle')]"),
				"Discovery Complete label");

		private readonly Label discoveryCompleteWithErrorsLabel =
			new Label(By.XPath("//i[contains(@data-bind, 'discoveryState')]/ancestor::tr//*[text()='Complete With Errors']"),
				"Complete with errors label");

		private readonly Button editProjectButton = new Button(By.XPath("//a[contains(@href, 'Edit?project')]"),
			"Edit Project button");

		private readonly Button editTenantsButton = new Button(By.XPath("//div[contains(@class, 'ibox-title')]//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'tenant')]]//ancestor::div[contains(@class, 'ibox-title')]//a"),
			"Edit tenants button");

		private readonly Label finalizingValueLabel =
			new Label(
//				By.XPath("//div[contains(@class, 'ibox-content')]/a[contains(@data-bind, 'completedNumber')]"),
				By.XPath("//div[@class='ibox'][1]/div[@class='ibox-content']//a[contains(@data-bind, 'completedNumber')][text()[contains(.,'2')]]"),
                "Finalizing users value label");
		
		private readonly Label migrationGroupLabel = new Label(By.XPath("//div[contains(@class, 'ibox-title')]//*[contains(text(), 'Migration waves for users')]"),
			"Migration groups label");

		private readonly Label migrationReadyValueLabel =
			new Label(
				By.XPath(
					"//*[contains(text(), 'Migration')]/ancestor::div[contains(@class, 'ibox')]//*[contains(text(), 'All Users')]/ancestor::tr//a[contains(@data-bind, 'readyNumber')]"),
				"Migration block Ready value label");

		private readonly Label readyUsersValueLabel =
			new Label(
				By.XPath("//div[contains(@class, 'ibox-content')]/a[contains(@data-bind, 'readyLink')]"),
				"Ready users value label");
		
		private readonly Label tenantsLabel = new Label(By.XPath("//div[contains(@class, 'ibox-title')]//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'tenant')]]"), "Tenants label");

		private readonly Button yesButton =
			new Button(By.XPath("//div[contains(@class, 'modal in')][@id='confirmationDialog']//button[text()='Yes']"),
				"Yes button");
		private readonly Button openPublicFoldersMigrationViewButton = new Button(By.XPath("//*[contains(text(), 'Public folders')]/ancestor::div[contains(@class, 'ibox-title')]//a"), "Edit Public folders button");
		private readonly Button coexSettingsButton = new Button(By.XPath("//a[contains(@data-bind, 'configureProjectLink')]"), "COEX settings button");
		private readonly Button totalPublicFoldersMigrationsButton = new Button(By.XPath("//*[contains(text(), 'Public folders')]/ancestor::div[contains(@class, 'ibox')]//div[contains(@class, 'ibox-content')]//a[contains(@data-bind, 'totalLink')]"), "Total migrations button");
		private readonly Button migrationGroupsButton = new Button(By.XPath("//a[contains(@data-bind, 'distGroupsLink')]"), "Migration groups button");
        private readonly Button totalGroupsButton = new Button(By.XPath("//div[contains(@class, 'ibox-content')]//a[contains(@data-bind, 'allGroupsLink')]"), "Total groups button");
	    protected Label descriptionLabel => new Label(By.XPath("//*[contains(@data-bind, 'projectDescription')]"), "Description Label");
        public ProjectOverviewForm() : base(TitleLocator, "Project overview form")
		{
            descriptionLabel.WaitForElementPresent();
		}

		public void OpenCoexSettings()
		{
			Log.Info("Opening COEX settings");
			coexSettingsButton.Click();
		}

	    public void OpenTotalGroups()
	    {
            Log.Info("Opening total groups");
            totalGroupsButton.Click();
	    }

	    public void OpenMigrationGroups()
		{
			Log.Info("Opening migrations group");
			migrationGroupsButton.Click();
		}

		public void OpenPublicFolders()
		{
			Log.Info("Opening public folders");
			totalPublicFoldersMigrationsButton.Click();
		}

		public void OpenUsersList()
		{
			Log.Info("Opening users list");
			allUsersAmountButton.Click();
		}

		public void EditTenants()
		{
			Log.Info("Editting tenants");
			editTenantsButton.WaitForElementPresent();
			ScrollToElement(editTenantsButton.GetElement(), "false");
			editTenantsButton.Click();
		}

		public void OpenEditPublicFolders()
		{
			Log.Info("Editing public folders");
			openPublicFoldersMigrationViewButton.Click();
		}

		public void ScrollToElement(IWebElement element)
		{
			((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView();", element);
		}

		public void ScrollToElement(IWebElement element, string options)
		{
			((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript($"arguments[0].scrollIntoView({options});", element);
			((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("var element = arguments[0]; var bodyRect = document.body.getBoundingClientRect(), elemRect = element.getBoundingClientRect(), offset = elemRect.top - bodyRect.top; window.scrollTo(0, offset - 100);", element);
		}

		public new void ScrollToTheBottom()
		{
			Log.Info("Scrolling to the bottom of the page");
			((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 10)");
		}

		public void AssertAllContentBlocksArePresent()
		{
			Log.Info("Asserting Migration groups exist");
			migrationGroupLabel.WaitForElementPresent();
			Log.Info("Asserting Tenants exist");
			tenantsLabel.WaitForElementPresent();
		}

		public void AssertConnectionsStatusesExist(int count)
		{
			Log.Info("Asserting connection status labels are: " + count);
			connectionStatusLabel.WaitForSeveralElementsPresent(count);
		}

		public void WaitTillDiscoveryIsComplete()
		{
			Log.Info("Waiting till discovery is completed");
			var counter = 0;
			while (!discoveryCompleteStateLabel.IsPresent(2) && counter < 60)
			{
				Refresh();
				Assert.IsTrue(!discoveryCompleteWithErrorsLabel.IsPresent());
				counter++;
			}
			discoveryCompleteStateLabel.WaitForSeveralElementsPresent(2);
		}

		public void WaitTillDiscoveryIsComplete(int timeout)
		{
			Log.Info("Waiting till discovery is completed");
			var counter = 0;
			while (!discoveryCompleteStateLabel.IsPresent(2) && counter < timeout)
			{
				Thread.Sleep(60000);
				Refresh();
				Assert.IsTrue(!discoveryCompleteWithErrorsLabel.IsPresent());
				counter++;
			}
			discoveryCompleteStateLabel.WaitForSeveralElementsPresent(2);
		}
		public void WaitTillDiscoveryIsComplete(int timeout, string tenant)
		{
			Log.Info("Waiting till discovery is completed for: "+tenant);
			var counter = 0;
			Label discoveryCompletedLabel = new Label(By.XPath($"//*[contains(text(), '{tenant}')]/ancestor::tr//i[contains(@data-bind, 'discoveryState')][contains(@class, 'icon-success')][contains(@class, 'fa-check-circle')]"),tenant+ " discovery completed state");
			while (!discoveryCompletedLabel.IsPresent() && counter < timeout)
			{
				Thread.Sleep(60000);
				Refresh();
				counter++;
			}
			discoveryCompletedLabel.WaitForElementPresent();
		}

        public void AssertBothTenantsDiscoveryIsComplete()
		{
			Log.Info("Asserting both tenants discovery is complete");
			discoveryCompleteStateLabel.WaitForSeveralElementsPresent(2);
		}

		public void WaitForMinutes(int time)
		{
			Log.Info($"Waiting for {time} minutes");
			var counter = 0;
			while (counter < time)
			{
				Thread.Sleep(40000);
				Browser.GetDriver().Navigate().Refresh();
				Thread.Sleep(20000);
				counter++;
			}
		}

		private void Refresh()
		{
			Thread.Sleep(22000);
			Browser.GetDriver().Navigate().Refresh();
			Thread.Sleep(8000);
		}

		public void OpenAllCompletedUsers()
		{
			Log.Info("Open all completed users details");
			allUsersCompletedButton.Click();
		}

		public void ArchiveProject()
		{
			Log.Info("Archiving project");
			archiveProjectButton.Click();
		}

		public void ConfirmArchiving()
		{
			Log.Info("Confirming archiving");
			yesButton.Click();
		}

		public void GetUsersCount()
		{
			Log.Info("Storing users count");
			Store.AllUsersCount = allUsersAmountButton.GetText().Trim();
			Log.Info("All users count is: " + Store.AllUsersCount);
			Store.ReadyUsersCount = readyUsersValueLabel.GetText().Trim();
			Log.Info("Ready users count is: " + Store.ReadyUsersCount);
		}

		public void AssertNewUsersAreAdded()
		{
			Log.Info("Asserting new users are added");
			Assert.IsTrue(int.Parse(Store.AllUsersCount) < int.Parse(allUsersAmountButton.GetText().Trim()),
				"New users are not added");
			Assert.IsTrue(int.Parse(Store.ReadyUsersCount) < int.Parse(readyUsersValueLabel.GetText().Trim()));
		}

		public void AssertMigrationUserCount()
		{
			Log.Info("Asserting migration user count");
			Assert.IsTrue(migrationReadyValueLabel.GetText().Trim() !="0", "Migration Ready users count is invalid");
		}

		public void AssertDuplicateUsersAreNotAdded()
		{
			Log.Info("Asserting new users are added");
			Assert.IsTrue(int.Parse(Store.AllUsersCount) == int.Parse(allUsersAmountButton.GetText().Trim()),
				"New users are not added");
			Assert.IsTrue(int.Parse(Store.ReadyUsersCount) == int.Parse(readyUsersValueLabel.GetText().Trim()));
		}

		public void AssertNewUsersAreReplaced()
		{
			Log.Info("Asserting new users are replaced");
			Assert.IsTrue(int.Parse(Store.AllUsersCount) <= int.Parse(allUsersAmountButton.GetText().Trim()),
				"New users are not added");
			Assert.IsTrue(int.Parse(Store.ReadyUsersCount) <= int.Parse(readyUsersValueLabel.GetText().Trim()));
		}

		public void AssertReadyUserEqualToAll()
		{
			Log.Info("Asserting All users count == Ready Users count");
			Assert.IsTrue(Store.ReadyUsersCount == Store.AllUsersCount, "All users count is not equal to ready users count");
		}

		public void OpenProjectDetails()
		{
			Log.Info("Opening project details");
			editProjectButton.Click();
		}

		public void AssertFinalizingCount(int count)
		{
			Log.Info("Asserting Finalized user amount is equal to: " + count);
			var counter = 0;
			Label completedUsersLabel = new Label(By.XPath($"//div[@class='ibox'][1]/div[@class='ibox-content']//a[contains(@data-bind, 'completedNumber')][text()[contains(.,'{count}')]]"), "Finalized users label");
			while (!completedUsersLabel.IsPresent() && counter<35)
			{
				Thread.Sleep(50000);
				Browser.GetDriver().Navigate().Refresh();
				Thread.Sleep(10000);
				counter++;
			}
            Assert.IsTrue(int.Parse(finalizingValueLabel.GetText().Trim()) == count, "Finalizing users amount is invalid");
		}

		public void OpenFinalizingUsers()
		{
			Log.Info("Opening finalizing users");
			finalizingValueLabel.Click();
		}

		public void AssertDiscoveredUserCount(string count)
		{
			Log.Info("Asserting user count is valid");
			Assert.IsTrue(Store.AllUsersCount.Trim() == count.Trim(), "All users count is invalid");
		}

		#region [Settings popup]

		private readonly Label timestampLabel =
			new Label(By.XPath("//div[contains(@class, 'modal fade in')]//ul[contains(@class, 'm-t')]//li"), "Timestamp label");

		private readonly Button closeButton =
			new Button(By.XPath("//div[contains(@class, 'modal fade in')]//button[contains(text(), 'Close')]"),
				"Close popup button");

		private readonly Label discoveryLabel =
			new Label(By.XPath("//div[contains(@class, 'modal fade in')]//strong[contains(text(), 'Discovery')]"),
				"Discovery label");

		private readonly Button discoverySwitherButton =
			new Button(By.XPath("//div[contains(@class, 'modal fade in')]//input[contains(@id, 'discoveryEnabled')]/.."),
				"Discovery switcher");

		private readonly Button runDiscoveryButton =
			new Button(By.XPath("//div[contains(@class, 'modal fade in')]//button[contains(@data-bind, 'startDiscovery')]"),
				"Run discovery button");

		public void AssertTime()
		{
			Log.Info("Assertimg timestamp time");
			var stamp = timestampLabel.GetText();
			var timeStamp = stamp.Substring(13, stamp.Length - 13);
			var dateTime = Convert.ToDateTime(timeStamp);
			var currenTime = DateTime.Now;
			var difference = currenTime - dateTime;
			Assert.IsTrue(difference.Minutes < 20, "Time stamp is in the different TimeZone");
		}

		public void RunDiscovery()
		{
			Log.Info("Running discovery");
			runDiscoveryButton.Click();
		}

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

		public void AssertNoDiscoveryInfo()
		{
			Log.Info("Asserting settings does not contain discovery info");
			discoveryLabel.WaitForElementDisappear();
		}

		#endregion
	}
}