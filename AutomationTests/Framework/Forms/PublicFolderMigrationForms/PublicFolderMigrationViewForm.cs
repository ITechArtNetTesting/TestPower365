using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderMigrationViewForm : UsersForm
	{
		private static readonly By TitleLocator = By.XPath("//a[contains(@href, 'PublicFolderMigration/Edit')]");

		public PublicFolderMigrationViewForm() : base(TitleLocator, "Public folder migration view")
		{
		}

		private readonly Button addPublicFolderMigrationButton = new Button(By.XPath("//a[contains(@href, 'PublicFolderMigration/Edit')]"), "Add public folder migration button");
		private readonly Button actionsDropdownButton =
		new Button(
			By.XPath(
				"//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"),
			"Actions dropdown");
		private new readonly Button expandedActionsDropdownButton =
			new Button(
				By.XPath(
					"//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')][contains(@aria-expanded, 'true')]"),
				"Expanded actions dropdown");
		private readonly Button toggleViewButton = new Button(By.XPath("//a[contains(@data-bind, 'toggleSelectionViewMode')]"), "Toggle view button");
		private readonly Button sortSourceButton = new Button(By.XPath("//span[text()='Source'][contains(@data-bind, 'sortModel')]"), "Sort source button");
		private readonly Label sourceSortedAscLabel = new Label(By.XPath("//span[text()='Source']//i[contains(@class, 'fa-sort-asc')]"),"Source sorted ASC label");
		private readonly Label sourceSortedDescLabel = new Label(By.XPath("//span[text()='Source']//i[contains(@class, 'fa-sort-desc')]"), "Source sorted DESC label");
		private readonly Button sortSourcePathButton = new Button(By.XPath("//span[text()='Source Path'][contains(@data-bind, 'sortModel')]"), "Sort Source Path button");
		private readonly Label sourcePathSortedAscLabel = new Label(By.XPath("//span[text()='Source Path']//i[contains(@class, 'fa-sort-asc')]"), "Source Path ASC label");
		private readonly Label sourcePathSortedDescLabel = new Label(By.XPath("//span[text()='Source Path']//i[contains(@class, 'fa-sort-desc')]"), "Source Path DESC label");
		private readonly Button sortTargetButton = new Button(By.XPath("//span[text()='Target'][contains(@data-bind, 'sortModel')]"), "Sort Target button");
		private readonly Label targetSortedAscLabel = new Label(By.XPath("//span[text()='Target']//i[contains(@class, 'fa-sort-asc')]"), "Target ASC label");
		private readonly Label targetSortedDescLabel = new Label(By.XPath("//span[text()='Target']//i[contains(@class, 'fa-sort-desc')]"), "Target DESC label");
		private readonly Button sortTargetPathButton = new Button(By.XPath("//span[text()='Target Path'][contains(@data-bind, 'sortModel')]"), "Sort Target Path button");
		private readonly Label targetPathSortedAscLabel = new Label(By.XPath("//span[text()='Target Path']//i[contains(@class, 'fa-sort-asc')]"), "Target Path ASC label");
		private readonly Label targetPathSortedDescLabel = new Label(By.XPath("//span[text()='Target Path']//i[contains(@class, 'fa-sort-desc')]"), "Target Path DESC label");
		private readonly Button sortStatusButton = new Button(By.XPath("//span[text()='Status'][contains(@data-bind, 'sortModel')]"), "Sort Status button");
		private readonly Label statusSortedAscLabel = new Label(By.XPath("//span[text()='Status']//i[contains(@class, 'fa-sort-asc')]"), "Status ASC label");
		private readonly Label statusSortedDescLabel = new Label(By.XPath("//span[text()='Status']//i[contains(@class, 'fa-sort-desc')]"), "Target Path DESC label");
		private readonly Button sortLastSyncButton = new Button(By.XPath("//span[text()='Last Sync'][contains(@data-bind, 'sortModel')]"), "Sort Last Sync button");
		private readonly Label lastSyncSortedAscLabel = new Label(By.XPath("//span[text()='Last Sync']//i[contains(@class, 'fa-sort-asc')]"), "Last Sync ASC label");
		private readonly Label lastSyncSortedDescLabel = new Label(By.XPath("//span[text()='Last Sync']//i[contains(@class, 'fa-sort-desc')]"), "Last Sync Path DESC label");
		private readonly Button sortNextSyncButton = new Button(By.XPath("//span[text()='Next Sync'][contains(@data-bind, 'sortModel')]"), "Sort Next Sync button");
		private readonly Label nextSyncSortedAscLabel = new Label(By.XPath("//span[text()='Next Sync']//i[contains(@class, 'fa-sort-asc')]"), "Next Sync ASC label");
		private readonly Label nextSyncSortedDescLabel = new Label(By.XPath("//span[text()='Next Sync']//i[contains(@class, 'fa-sort-desc')]"), "Next Sync Path DESC label");
		private readonly Button sortConfllictResolutionButton = new Button(By.XPath("//span[text()='Conflict Resolution'][contains(@data-bind, 'sortModel')]"), "Sort Conflict Resolution button");
		private readonly Label nextConflictResolutionSortedAscLabel = new Label(By.XPath("//span[text()='Conflict Resolution']//i[contains(@class, 'fa-sort-asc')]"), "Conflict Resolution ASC label");
		private readonly Label conflictResolutionSortedDescLabel = new Label(By.XPath("//span[text()='Conflict Resolution']//i[contains(@class, 'fa-sort-desc')]"), "Conflict Resolution DESC label");
		public void AddPublicFolderMigration()
		{
			Log.Info("Adding public folder migration");
			addPublicFolderMigrationButton.Click();
		}

		public void ToggleView()
		{
			Log.Info("Toggling view");
			toggleViewButton.Click();
		}
        
        public void SelectAction(string action)
		{
			OpenActionsDropdown();
			ChooseAction(action);
		}

		public new void OpenActionsDropdown()
		{
			Log.Info("Opening Actions dropdown");
			ScrollToElement(actionsDropdownButton.GetElement());
			actionsDropdownButton.Click();
			try
			{
				expandedActionsDropdownButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Actions dropdown is not ready");
				actionsDropdownButton.Click();
			}
		}

		public new void SelectEntryBylocator(string locator)
		{
			ScrollToTop();
			Log.Info("Selecting checkbox for: " + locator);
			BaseElement.WaitForElementPresent(By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//input"),
					locator + " entry checkbox");
			var entryCheckboxButton =
				new RadioButton(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//input"),
					locator + " entry checkbox");
			var entryLabel = new Button(By.XPath(
					$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
				locator + " entry label");
			ScrollToElement(entryCheckboxButton.GetElement());
			try
			{
				entryLabel.Click();
				entryCheckboxButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Checkbox is not ready");
				entryLabel.Click();
			}
		}
		public new void AssertUserHaveSyncingState(string locator)
		{
			Log.Info($"Asserting user with locator: {locator} contains syncing state");
			var syncingState =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//*[text()='Syncing']"),
					locator + " syncing state");
			syncingState.WaitForElementPresent();
		}
		public new void OpenDetailsByLocator(string locator)
		{
			Log.Info($"Opening details by locator: {locator}");
			BaseElement.WaitForElementPresent(By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
					$"{locator} details button");
			var detailsButton =
				new Button(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
					$"{locator} details button");
			ScrollToElement(detailsButton.GetElement());
			detailsButton.DoubleClick();
		}
		public new void SyncUserByLocator(string locator)
		{
			ScrollToTop();
			Log.Info("Syncing user by locator: " + locator);
			Thread.Sleep(2000);
			SelectEntryBylocator(locator);
			SelectAction("Sync");
			Apply();
		}
		#region[Details modal]
		private readonly Label sourceTenantLabel = new Label(By.XPath("//dd[contains(@data-bind, 'sourceTenantName')]"), "Source tenant label");
		private readonly Label sourceTenantPathLabel = new Label(By.XPath("//dd[contains(@data-bind, 'sourceTenantPath')]"), "Source tenant label");
		private readonly Label targetTenantLabel = new Label(By.XPath("//dd[contains(@data-bind, 'targetTenantName')]"), "Target tenant label");
		private readonly Label targetTenantPathLabel = new Label(By.XPath("//dd[contains(@data-bind, 'targetTenantPath')]"), "Target tenant path label");
		private readonly Label copySubfoldersLabel = new Label(By.XPath("//dd[contains(@data-bind, 'recursive ?')]"), "Copy subfolders label");
		private readonly Label lastSyncLabel = new Label(By.XPath("//dd[contains(@data-bind, 'lastSyncCompleted')]"), "Last sync label");
		private readonly Label conflictResolutionLabel = new Label(By.XPath("//dd[contains(@data-bind, 'conflictResolutionString')]"), "");
		private readonly Label provisioningJobLabel = new Label(By.XPath("//*[text()[contains(., 'Provisioning')]]"), "Provisioning job label");
		private readonly Label contentCopyJobLabel = new Label(By.XPath("//*[text()[contains(., 'Content Copy')]]"), "Content Copy job label");
		private Label GetJobLine(string state)
		{
			return new Label(By.XPath($"//*[text()[contains(.,'{state}')]]/ancestor::tr"),state+ " line");  
		}

		public void WaitForProvisioningJobAppear(int count)
		{
			Log.Info("Waiting till provisioning jobs appear");
			bool ready = false;
			int counter = 0;
			while (!ready && counter<20)
			{
				try
				{
					provisioningJobLabel.WaitForSeveralElementsPresent(count, 30000);
					ready = true;
				}
				catch (Exception)
				{
					counter++;
					RefreshData();
				}
			}
		}

		public void WaitForContentCopyJobAppear(int count)
		{
			Log.Info("Waiting till content copy jobs appear");
			bool ready = false;
			int counter = 0;
			while (!ready && counter < 20)
			{
				try
				{
					contentCopyJobLabel.WaitForSeveralElementsPresent(count, 30000);
					ready = true;
				}
				catch (Exception)
				{
					counter++;
					RefreshData();
				}
			}
		}

		public void WaitForProvisioningJobDone()
		{
			Log.Info("Waiting till Provisioning job is done");
			int counter = 0;
			while (!GetLastProvisioningJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'State')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.ToLower().Contains("done") && counter<20)
			{
				Thread.Sleep(30000);
				RefreshData();
				Thread.Sleep(10000);
				counter++;
			}
		}

		public void WaitForContentCopyJobDone()
		{
			Log.Info("Waiting till Content Copy job is done");
			int counter = 0;
			while (!GetLastContentCopyJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'State')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.ToLower().Contains("done") && counter < 20)
			{
				Thread.Sleep(30000);
				RefreshData();
				Thread.Sleep(10000);
				counter++;
			}
		}

		public void VerifyProccessedFolders(string amount)
		{
			Log.Info("Verifying proccessed folders are : "+amount);
			Assert.IsTrue(GetLastProvisioningJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Processed')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains(amount+" Folder"), "Proccessed folders error");
		}

		public void VerifyProvisioningTimeStampsAreNotEmpty()
		{
			Log.Info("Verifying timestamps are not empty");
			Assert.IsTrue(GetLastProvisioningJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Started')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains("/"), "Invalid Started timestamp format");
			Assert.IsTrue(GetLastProvisioningJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Started')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains(":"), "Invalid Started timestamp format");
			Assert.IsTrue(GetLastProvisioningJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Completed')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains("/"), "Invalid Completed timestamp format");
			Assert.IsTrue(GetLastProvisioningJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Completed')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains(":"), "Invalid Completed timestamp format");
		}

		public void VerifyContentCopyTimeStampsAreNotEmpty()
		{
			Log.Info("Verifying timestamps are not empty");
			Assert.IsTrue(GetLastContentCopyJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Started')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains("/"), "Invalid Started timestamp format");
			Assert.IsTrue(GetLastContentCopyJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Started')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains(":"), "Invalid Started timestamp format");
			Assert.IsTrue(GetLastContentCopyJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Completed')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains("/"), "Invalid Completed timestamp format");
			Assert.IsTrue(GetLastContentCopyJob().FindElement(By.XPath(".//td[count(//th//*[text()[contains(., 'Completed')]]/ancestor::th/preceding-sibling::th)+1]//*[normalize-space(text())]")).Text.Contains(":"), "Invalid Completed timestamp format");
		}

		public void DownloadProvisioningLogs()
		{
			Log.Info("Downloading provisioning logs");
			GetLastProvisioningJob().FindElement(By.XPath(".//a//*[contains(text(), 'LOGS')]")).Click();
		}

		public IWebElement GetLastProvisioningJob()
		{
			var provisioningJobsList = Browser.GetDriver().FindElements(By.XPath("//*[text()[contains(., 'Provisioning')]]/ancestor::tr"));
			return provisioningJobsList[0];
		}

		public IWebElement GetLastContentCopyJob()
		{
			var provisioningJobsList = Browser.GetDriver().FindElements(By.XPath("//*[text()[contains(., 'Content Copy')]]/ancestor::tr"));
			return provisioningJobsList[0];
		}

		#endregion
	}
}
