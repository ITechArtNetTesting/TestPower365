using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Internal;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Product.Framework.Elements;
using Product.Framework.Enums;

namespace Product.Framework.Forms
{
	public class UsersForm : BaseForm
	{
        private readonly Label startTimeOfLastRollback = new Label(By.XPath("//div[@class='modal-body']//div[@class='table-responsive table-frame m-t-sm']//tbody//td[ancestor::tr//span[contains(text(),'Rollback')]][4]//span[last()]"), "Start time of last job");

        private readonly Label endTimeOfLastRollback = new Label(By.XPath("//div[@class='modal-body']//div[@class='table-responsive table-frame m-t-sm']//tbody//td[ancestor::tr//span[contains(text(),'Rollback')]][5]//span[last()]"), "Start time of last job");

        private readonly Button groupActionsDropdownButton = new Button(By.XPath("//div[@class='ibox m-t-lg']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"), "Actions dropdown in groups edit page");

        private readonly Button newMigrationWave = new Button(By.XPath("//span[contains(@data-bind,'addWave')]"), "New migration wave button");

        private readonly Button migrationWaves = new Button(By.XPath("//a[contains(@href,'waves')]//span"), "Migration waves button");

        private static readonly By TitleLocator =
			By.XPath("//div[@id='users']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]");

		private readonly Button actionsDropdownButton =
			new Button(
				By.XPath(
					"//div[@id='users']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"),
				"Actions dropdown");

		private readonly Button backToDashboardButton = new Button(
			By.XPath("//button[contains(@data-bind, 'goToDashboard')]"), "Back to dashboard button");

		private readonly TextBox chooseFilesInput =
			new TextBox(By.XPath("//div[contains(@class, 'modal in')]//input[@type='file']"), "Choose files input");

		private readonly Button closeFilterButton =
			new Button(By.XPath("//div[@class='panel-footer']//button[text()='Close']"), "Close filter button");

		private readonly Button closeModalWindowButton =
			new Button(By.XPath("//div[contains(@class, 'modal fade in')]//div[@class='modal-footer']//button[text()='Close']"),
				"Close modal window button");

		private readonly Button confirmYesButton =
			new Button(
				By.XPath("//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), 'Yes')]"),
				"Confirm button");

		private readonly Button disabledApplyActionButton =
			new Button(By.XPath("//button[contains(@data-bind, 'applyAction')][@disabled='']"), "Disabled apply button");
           

        private Button enabledApplyActionButton =
			new Button(By.XPath("//button[contains(@data-bind, 'applyAction')][not(@disabled='')]"), "Enabled apply button");

		private readonly Button enabledArchiveButton =
			new Button(By.XPath("//button[contains(text(), 'Archive')][not(@disabled='')]"), "Enabled archive button");

		private readonly Button enabledEditButton =
			new Button(By.XPath("//button[contains(text(), 'Edit')][not(@disabled='')]"), "Enabled edit button");

		private readonly Button enabledExportButton =
			new Button(By.XPath("//button[contains(text(), 'Export')][not(@disabled='')]"), "Enabled export button");

		protected readonly Button expandedActionsDropdownButton =
			new Button(
				By.XPath(
					"//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')][contains(@aria-expanded, 'true')]"),
				"Expanded actions dropdown");

		private readonly Button filterButton = new Button(By.XPath("//a//span[contains(text(), 'Filter')]"), "Filter button");

		private readonly Button fixErrorsButton =
			new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'goFixErrors')]"),
				"Fix errors button");

		private readonly Button groupFilterButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'Group')]]/..//i[contains(@class, 'fa-filter')]"),
				"Group filter button");

		private readonly Button importButton = new Button(By.XPath("//a[contains(@data-bind, 'uploadUser')]"), "Import button");

		private readonly Label importedLabel = new Label(By.XPath("//h3[contains(text(), 'successfully imported')]"),         

            "Imported label");

		private readonly Button okButton =
			new Button(
				By.XPath("//div[contains(@class, 'modal in')]//button[contains(@class, 'pull-right')][not(@disabled='')]"),
				"OK button");

		private readonly Button profileFilterButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'Profile')]]/..//i[contains(@class, 'fa-sort')]"),
				"Profile filter button");

       private readonly Button sampleFileButton =
			new Button(
				By.XPath("//div[contains(@class, 'modal fade in')]//a[contains(@href, 'DownloadUserMigrationsForUpdateTemplate')]"),
				"Sample file button");

		private readonly TextBox searchTextBox = new TextBox(By.XPath("//div[contains(@class, 'search-group')]//input"),
			"Search textbox");

		private readonly Button selectAllButton =
			new Button(By.XPath("//th//input[contains(@data-bind, 'allSelectedChecked')]"), "Select all checkbox");

		private readonly Label selectedFilesLabel = new Label(By.XPath("//h3[contains(text(), 'Selected Files')]"),
			"Selected files label");

		private readonly Button sortArchiveSizeButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Archive')]]//i"),
				"Sort Archive size button");

		private readonly Button sortedAscArchiveSizeButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Archive')]]//i[contains(@class, 'fa-sort-asc')]"),
				"Archive size sorted ASC button");

		private readonly Button sortedAscGroupButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Wave')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),
				"Group sorted ASC button");

		private readonly Button sortedAscMailboxSizeButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//span[text()[contains(.,'Mailbox')]]//i[contains(@class, 'fa-sort-asc')]"),
				"Mailbox size sorted ASC button");

		private readonly Button sortedAscProfileButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Profile')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),
				"Profile sorted ASC button");

		private readonly Button sortedAscSourceButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Source')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),
				"Source sorted ASC button");

		private readonly Button sortedAscStateButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Status')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),
				"State sorted ASC button");

		private readonly Button sortedAscTargetButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Target')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),
				"Target sorted ASC button");

		private readonly Button sortGroupButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Wave')]]/ancestor::th//i"),
				"Sort group button");

		private readonly Button sortMailboxSizeButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//span[text()[contains(.,'Mailbox')]]//i"),
				"Sort mailbox size button");

		private readonly Button sortProfileButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Profile')]]/ancestor::th//i"),
				"Sort profile button");
	
		private readonly Button sortSourceButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Source')]]/ancestor::th//i"),
				"Sort source button");

		private readonly Button sortStatusButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Status')]]/ancestor::th//i"),
				"Sort state button");

		private readonly Button sortTargetButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Target')]]/ancestor::th//i"),
				"Sort target button");

		private readonly Button stateFilterButton =
			new Button(
				By.XPath(
					"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'State')]]/..//i[contains(@class, 'fa-filter')]"),
				"State filter button");    
        private Button enabledOkProfileButton => new Button(By.XPath("//button[contains(@data-bind, 'addToMigrationProfile')][not(@disabled='')]"), "Enabled OK profile button");

        private Button userDetailsRefreshButton => new Button(By.XPath("//button[contains(@data-bind, 'refresh.run')][not(@disabled='')]"), "User Details Refresh button");
        private Button userDetailsCloseButton => new Button(By.XPath("//button[contains(@data-dismiss, 'modal')][not(@disabled='')]"), "User Details Close button");

        private readonly Label openedFilterLabel = new Label(By.XPath("//div[contains(@role, 'tooltip') and contains(@id, 'popover')]"), "Opened filter");
		private readonly Label noDataLabel = new Label(By.XPath("//td[contains(text(), 'No data available')]"), "No data available label");
        
        protected Label descriptionLabel => new Label(By.XPath("//*[contains(@data-bind, 'projectDescription')]"), "Description Label");
	    protected string ProfileRowLocator = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]";
        protected string ProfileModifyLocator = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]//*[contains(text(), 'Modify')]";
        protected string ProfileLabelLocator = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]//label";
        protected string ProfileRadioLocator = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), 'Defa')]]//input";
        private readonly Button CompleteDetailsButton = new Button(By.XPath("//button[text()='Complete']"), "Complite button on details form");
        private readonly Button CutoverDetailsButton = new Button(By.XPath("//button[text()='Cutover']"), "Cutover button on details form");
        private readonly Label ImportCompleteLabel = new Label(By.XPath("//span[@data-translation='UploadWasASuccessExclamationPoint']"), "Label Upload Was A Success");
        private readonly string ProgressBar_100 = "//div[@class='progress' and ancestor::tr//td//span[contains(text(),'{0}')]]/div[contains(@style,'width: 100%;')]";

        public UsersForm() : base(TitleLocator, "Users list form")
		{
            descriptionLabel.WaitForElementPresent();

		}

		public UsersForm(By locator, string name) : base(locator, name)
		{
		    descriptionLabel.WaitForElementPresent();
        }

        public void СloseSuccessfulImportWindow()
        {
            Button closeButton =
                new Button(By.XPath("//div[@class='modal-body']//*[contains(text(), 'Finish')]"),
                    "Close modal successful window button");
            closeButton.Click();
        }              

        public void AssertMigrationJobWasStopped(int SelectedUser)
        {
            WaitForAjaxLoad();
            Assert.IsTrue(Browser.GetDriver().FindElements(By.XPath("//div[@id='users']//tr//td[4]//span"))[SelectedUser].Text.Contains("Stopping"));
        }


        public void AssertCutoverCompliteDetailsIsDisabled()
        {
            Assert.IsTrue(!CompleteDetailsButton.IsPresent(), "Complete button is present");
            Assert.IsTrue(!CutoverDetailsButton.IsPresent(), "Cutover button is present");
        }

        public void CheckActionIsDisabled(ActionType type)
        {
            WaitForAjaxLoad();
            SelectAction(type);
            CheckApplyButtonIsDisabled();
        }

        public void CheckApplyButtonIsDisabled()
        {
            WaitForAjaxLoad();
            Assert.IsFalse(enabledApplyActionButton.IsPresent(),"Appply button is enable");
        }

        public void AssertCutoverCompliteDetailsIsEnabled()
        {
            WaitForAjaxLoad();
            Assert.IsTrue(CompleteDetailsButton.IsPresent(), "Complete button is not present");
            Assert.IsTrue(CutoverDetailsButton.IsPresent(), "Cutover button is not present");
        }

        //modify after verify
        public void CheckActionIsEnabled(ActionType type)
        {
            WaitForAjaxLoad();
            SelectAction(type);
            CheckApplyButtonIsEnabled();
        }

        //delete after verify
        public void CheckApplyButtonIsEnabled()
        {
            WaitForAjaxLoad();
            Assert.IsTrue(enabledApplyActionButton.IsPresent(), "Appply button is not enable");
        }

        public void CheckApplyButton(bool isEnabled)
        {
            WaitForAjaxLoad();
            if (isEnabled)
            {
                Assert.IsTrue(enabledApplyActionButton.IsPresent(), "Appply button is not enable");
            }
            else {
                Assert.IsFalse(enabledApplyActionButton.IsPresent(), "Appply button is enable");
            }
        }             

        public void PerformSearch(string search)
		{
			Log.Info("Searching: " + search);
			ScrollToTop();
            WaitForRowsAppear();
			searchTextBox.ClearSetText(search);
			searchTextBox.PressEnter();
          //  WaitForAjaxLoad();
            Thread.Sleep(2000);//Badd fixme
		}

	    public void ModifyProfile(string profile)
	    {
            Log.Info("Mofigying profile: "+profile);
            new Button(By.XPath(String.Format(ProfileModifyLocator, profile)), "Profile modify button").Click();
	    }

	    public void SwitchFilter(FilterState state)
		{
			Log.Info("Opening filter");
			ScrollToTop();
			filterButton.Click();
		    if (state == FilterState.Open)
		    {
		        try
		        {
                    openedFilterLabel.WaitForElementPresent(5000);
		        }
		        catch (Exception)
		        {
		            filterButton.Click();
                }
		    }
		    if (state == FilterState.Closed)
		    {
		        try
		        {
                    openedFilterLabel.WaitForElementDisappear(5000);
		        }
		        catch (Exception)
		        {
		            filterButton.Click();
                }
		    }
            WaitForAjaxLoad();

		}

	    public void ClickProfileOk()
	    {
            Log.Info("Clicling Profile OK button");
            enabledOkProfileButton.Click();
	    }

	    public void AssertApplyIsEnabled()
		{
			Log.Info("Asserting Apply actions button is enabeld");
			enabledApplyActionButton.WaitForElementPresent();
		}

	    public void SelectProfile(string profile)
	    {
            Log.Info("Selecting profile: "+profile);
	        Label profileLabel = new Label(By.XPath(String.Format(ProfileLabelLocator, profile)), profile + " profile label");
	        RadioButton profileRadioButton = new RadioButton(By.XPath(String.Format(ProfileRadioLocator, profile)), profile+" profile radiobutton");
            profileLabel.Click();
	            try
	            {
	                profileRadioButton.WaitForSelected(5000);
	            }
	            catch (Exception)
	            {
	                Log.Info("Radiobutton is not ready");
	                profileLabel.Click();
	        }
    }

	    public void AssertApplyIsDisabled()
		{
			Log.Info("Asserting Apply action button is disabled");
			disabledApplyActionButton.WaitForElementPresent();
		}

		public void Apply()
		{
			Log.Info("Applying action");
			enabledApplyActionButton.Click();
		}

		public void OpenActionsDropdown()
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

		public void ChooseAction(string action)
		{
			Log.Info("Choosing action: " + action);
			var actionButton =
				new Button(
					By.XPath(
						$"//button[contains(@aria-expanded, 'true')]/ancestor::div[contains(@class, 'dropdown')]//ul[contains(@class, 'dropdown-menu')]//li[contains(@data-toggle, 'tooltip')]//*[contains(text(), '{action}')]"),
					action + " option");
			actionButton.Click();
		}

		public void WaitForRowsAppear()
		{
			Log.Info("Waiting till rows appear");
			Label rowLabel = new Label(By.XPath("//td[contains(@data-bind, 'showDetails')]"), "Row label");
			rowLabel.WaitForElementPresent();
		}

		public void SelectAction(ActionType type)
		{
            Log.Info($"Select action {type}");
            OpenActionsDropdown();
			ChooseAction(type.GetValue());
		}

		public void AssertImportFailed()
		{
			Log.Info("Asserting import failed");           
            fixErrorsButton.WaitForElementPresent();
		}

        public void AssertImportSuccessful()
        {
            Log.Info("Asserting import failed");
            ImportCompleteLabel.WaitForElementIsVisible();          
            Assert.IsFalse(fixErrorsButton.IsElementVisible(), "Asserting import failed");
        }
       
		public void Archive()
		{
			Log.Info("Archiving entry");
			ScrollToTheBottom();
			enabledArchiveButton.Click();
		}

		public void SortSource()
		{
			Log.Info("Sorting source");
			ScrollToTop();
			sortSourceButton.Click();
            WaitForAjaxLoad();
		}

		public void SortProfile()
		{
			Log.Info("Sorting profile");
			ScrollToTop();
			sortProfileButton.Click();
            WaitForAjaxLoad();
        }

		public void SortMailboxSize()
		{
			Log.Info("Sorting mailbox size");
			ScrollToTop();
			sortMailboxSizeButton.Click();
            WaitForAjaxLoad();
        }

		public void SortArchiveSize()
		{
			Log.Info("Sorting archive size");
			ScrollToTop();
			sortArchiveSizeButton.Click();
            WaitForAjaxLoad();
        }

		public void SortTarget()
		{
			Log.Info("Sorting target");
			ScrollToTop();
			sortTargetButton.Click();
            WaitForAjaxLoad();
        }

		public bool IsNoDataLabelExist()
		{
			bool result;
			try
			{
				noDataLabel.WaitForElementPresent(5000);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public void SortStatus()
		{
			Log.Info("Sorting state");
			ScrollToTop();
			sortStatusButton.Click();
            WaitForAjaxLoad();
		}

        public bool IsElementPresent(string name, string locator)
        {
            return new Label(By.XPath(locator), name).IsPresent();
        }

		public void OpenDetailsByLocator(string locator)
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

        public void CheckDetailsTime(DateTime startTime,DateTime endTime)
        {           
            DateTime UIStartTime = DateTime.Parse(startTimeOfLastRollback.GetText());
            DateTime UIEndTime = DateTime.Parse(endTimeOfLastRollback.GetText());
            Assert.IsTrue(timesApproximatelyEqual(startTime, UIStartTime));
            Assert.IsTrue(timesApproximatelyEqual(endTime, UIEndTime));
        }

        private bool timesApproximatelyEqual(DateTime firstTime, DateTime secondTime)
        {
            bool result = true;
            result = firstTime.Month == secondTime.Month;
            result = firstTime.Day == secondTime.Day;
            result = firstTime.Year == secondTime.Year;
            result = firstTime.Hour == secondTime.Hour;
            result = (Math.Abs(secondTime.Minute - firstTime.Minute)) <= 5;
            return result;
            
        }

        public void DetailsRefresh()
        {
            userDetailsRefreshButton.WaitForElementPresent();
            userDetailsRefreshButton.Click();
        }

        public void DetailsClose()
        {
            userDetailsCloseButton.WaitForElementPresent();
            userDetailsCloseButton.Click();
        }

		public new void ScrollToTheBottom()
		{
			Log.Info("Scrolling to the bottom of the page");
			((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 10)");
		}

		public new void ScrollToTop()
		{
			Log.Info("Scrolling to the Top of the page");
			((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("window.scrollTo(0, 0)");
		}

		public void ScrollToElement(IWebElement element)
		{
            Log.Info("Scrolling to element: "+element);
		   ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView(false);", element);
		}

		public void SelectEntryBylocator(string locator)
		{
            // NOTE: disbled scrolling as obsolete 
			// ScrollToTop();
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
			ScrollToElement(entryLabel.GetElement());
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
        
        private readonly string _rowTextAncestorFormat = "/ancestor::tr//*[contains(text(), '{0}')]";
        private readonly string _lowerCaseTextLocatorFormat = "//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]]";
        public void WaitForState(string entry, State state, int timeout = 5000, int pollIntervalSec = 0)
        {
            var value = state.GetValue();
            if (state.GetValue().ToLower() == "synced")
                value = "complete";

            var rowEntryLocator = string.Format(_lowerCaseTextLocatorFormat, entry.ToLowerInvariant());
            var rowTextAncestorLocator = string.Format(_rowTextAncestorFormat, value);
            var rowEntryTextLocator = string.Format("{0}{1}", rowEntryLocator, rowTextAncestorLocator);

            var rowEntryTextValue = By.XPath(rowEntryTextLocator);
            if (!IsElementExists(rowEntryTextValue, timeout / 1000, pollIntervalSec))
                throw new Exception(string.Format("Entry of '{0}' with state '{1}' was not found.", entry, value));
        }

        public void WaitForAnyState(string entry, State[] states, int timeout = 5000, int pollIntervalSec = 0)
        {
            var bys = new List<By>();
            foreach (var state in states)
            {
                var value = state.GetValue();
                if (state.GetValue().ToLower() == "synced")
                    value = "complete";
                var rowEntryLocator = string.Format(_lowerCaseTextLocatorFormat, entry.ToLowerInvariant());
                var rowTextAncestorLocator = string.Format(_rowTextAncestorFormat, value);
                var rowEntryTextLocator = string.Format("{0}{1}", rowEntryLocator, rowTextAncestorLocator);

                var rowEntryTextValue = By.XPath(rowEntryTextLocator);
                bys.Add(rowEntryTextValue);
            }
            
            if(!IsAnyElementExists(bys.ToArray(), timeout / 1000, pollIntervalSec))
                throw new Exception(string.Format("Entry of '{0}' with any state '{1}' was not found.", entry, string.Join(", ", states)));
        }

        public void WaitForState2(string locator, State state, int timeout)
		{
			Log.Info($"Waiting for line with {locator} locator in {state.GetValue()} state");
                     
            var entryLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.Trim().ToLower()}')]]/ancestor::tr//*[text()='{state.GetValue()}']"),
					"");
            var entryNonStateLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.Trim().ToLower()}')]]/ancestor::tr"),
					"");
			var counter = 0;
			if (state.GetValue().ToLower() == "synced")
			{
				var twiceSyncedLabel =
					new Label(
						By.XPath(
							$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//*[text()[contains(.,'complete')]]"),
						"");
				while (!entryLabel.IsPresent() && !twiceSyncedLabel.IsPresent() && counter < 40)
				{
					Thread.Sleep(timeout);
					Browser.GetDriver().Navigate().Refresh();
					counter++;
					entryNonStateLabel.WaitForElementPresent();
				}
				if (entryLabel.IsPresent())
				{
					entryLabel.WaitForElementPresent();
				}
				else
				{
					twiceSyncedLabel.WaitForElementPresent();
				}
			}
			else
			{
             while (!entryLabel.IsPresent() && counter < 65)
                {
					Thread.Sleep(timeout/2);
                   if (entryLabel.IsPresent()) { break; };
                    Thread.Sleep(timeout / 2);
                    Browser.GetDriver().Navigate().Refresh();
                    counter++;
					entryNonStateLabel.WaitForElementPresent();
                  
                }
				entryLabel.WaitForElementPresent();
			}
		}

		public void ExportUsers()
		{
			Log.Info("Exporting");
			ScrollToTheBottom();
			enabledExportButton.Click();
		}

		public void ConfirmSync()
		{
			Log.Info("Confirming sync");
		    try
		    {
		        confirmYesButton.Click();
            }
		    catch (Exception)
		    {
                Log.Info("Confirmation window did not appear");
		        enabledApplyActionButton =
		            new Button(By.XPath("//button[contains(@data-bind, 'applyAction')][not(@disabled='')]"), "Enabled apply button");
                enabledApplyActionButton.Click();
		        confirmYesButton.Click();
            }
			
		}

		public void ConfirmPrepare()
		{
			Log.Info("Confirming preparing");
			confirmYesButton.Click();
			try
			{
				confirmYesButton.WaitForElementDisappear(5000);
			}
			catch (Exception)
			{
				Log.Info("Yes button is not ready");
				confirmYesButton.Click();
			}
		}

		public void ConfirmStop()
		{
			Log.Info("Confirming stop");
			confirmYesButton.Click();
		}

		public void ConfirmComplete()
		{
			Log.Info("Confirming complete");
			confirmYesButton.Click();
		}

		public void ConfirmCutover()
		{
			Log.Info("Confirming cutover");
			confirmYesButton.Click();
		}

		public void ConfirmArchive()
		{
			Log.Info("Confirming archive");
			confirmYesButton.Click();
		}

        public void ConfirmAction()
        {
            Log.Info("Confirming action");
            confirmYesButton.Click();
        }

        public void ConfirmRollback(bool resetPermissions = true)
        {
            Log.Info("Confirm Rollback");
            if(resetPermissions)
            {
                SetResetPermissions();
            }
            else
            {
                SetDontResetPermissions();
            }
            SetSureCheckbox();

            Rollback();
        }

		public void SelectAllLines()
		{
			Log.Info("Selecting all lines");
			ScrollToTop();
			try
			{
				selectAllButton.GetElement().Click();
				Assert.IsTrue(selectAllButton.GetElement().Selected);
			}
			catch (Exception)
			{
				Log.Info("Select all button is not ready");
				Thread.Sleep(3000);
				selectAllButton.GetElement().Click();
			}
		}

		public void OpenEditDialog()
		{
			Log.Info("Opening Edit dialog");
			ScrollToTheBottom();
			enabledEditButton.Click();
		}

		public void VerifyLineisExist(string locator)
		{
			Log.Info("Verifying line exists by locator: " + locator);
			var lineLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
					locator + " line");
			lineLabel.WaitForElementPresent();
		}

	    public void VerifyLineisExist(string locator, int count)
		{
			Log.Info("Verifying line exists by locator: " + locator);
			var lineLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
					locator + " line");
			lineLabel.WaitForElementPresent(count);
		}

		public bool IsLineExist(string locator)
		{
			bool result;
			Log.Info("Verifying line exists by locator: " + locator);
			var lineLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
					locator + " line");
			try
			{
				lineLabel.WaitForElementPresent(30000);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public void VerifyLineNotExist(string locator)
		{
			Log.Info("Verifying line not exists by locator: " + locator);
			var lineLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
					locator + " line");
			lineLabel.WaitForElementDisappear();
		}


        public void PerfomActionForUser(string locator, ActionType  type)
        {
            ScrollToTop();
            Log.Info(type +" user by locator: " + locator);
            WaitForAjaxLoad();
            SelectEntryBylocator(locator);
            SelectAction(type);
            Apply();
        }


        public void AssertUserHaveSyncingState(string locator)
		{
			Log.Info($"Asserting user with locator: {locator} contains syncing state");
			var syncingState =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.Trim().ToLower()}')]]/ancestor::tr//*[text()='Syncing']"),
					locator + " syncing state");
			syncingState.WaitForElementPresent();
		}

        public void AssertUserHasState(string locator, string state)
        {
            Log.Info($"Asserting user with locator: {locator} contains syncing state");
            var syncingState =
                new Label(
                    By.XPath(
                        $"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.Trim().ToLower()}')]]/ancestor::tr//*[text()='{state}']"),
                    locator + " syncing state");
            syncingState.WaitForElementPresent();
        }

        public void OpenStateFilter()
		{
			Log.Info("Opening state filter");
			ScrollToTop();
			stateFilterButton.Click();
		}

		public void OpenProjectOverview()
		{
			Log.Info("Opening project overview");
			ScrollToTop();
			backToDashboardButton.Click();
		}

		public void OpenGroupFilter()
		{
			Log.Info("Opening group filter");
			ScrollToTop();
			groupFilterButton.Click();
		}

		public void OpenProfileFilter()
		{
			Log.Info("Open profile filter");
			ScrollToTop();
			profileFilterButton.Click();
		}

		public void CloseModalWindow()
		{
			Log.Info("Closing filter");
			closeFilterButton.Click();
		}

		public void OpenImportDialog()
		{
			Log.Info("Opening Import dialog");
			ScrollToTop();
			importButton.Click();
		}

		public void ChooseFile()
		{
			Log.Info("Selecting file");
			try
			{
				chooseFilesInput.WaitForElementPresent(10000);
			}
			catch (Exception)
			{
				Log.Info("Modal dialog is not ready");
				importButton.Click();
			}
			chooseFilesInput.GetElement().SendKeys(Path.GetFullPath("resources/user-migration-template.csv"));
		}

		public void ChooseFile(string fileName)
		{
			Log.Info("Selecting file");
			try
			{
				chooseFilesInput.WaitForElementPresent(10000);
			}
			catch (Exception)
			{
				Log.Info("Modal dialog is not ready");
				importButton.Click();
			}
			chooseFilesInput.GetElement().SendKeys(Path.GetFullPath(RunConfigurator.ResourcesPath+ fileName));
		}

		public void WaitTillSelectedFilesAppear()
		{
			Log.Info("Wait till selected files appear");
			selectedFilesLabel.WaitForElementPresent();
		}

		public void ConfirmUploading()
		{
			Log.Info("Confirming upload");
			okButton.Click();
		}

		public void WaitTillImportedLabeleAppear()
		{
			Log.Info("Waiting till imported lable appear");
			importedLabel.WaitForElementPresent();
		}

		public void CloseImportedWindow()
		{
			Log.Info("Closing imported window");
			closeModalWindowButton.Click();
		}

		public void DownloadSample()
		{
			Log.Info("Downloading sample");
			sampleFileButton.Click();
		}

		public void VerifyLinesCountAndProperties(string source, string target, int count)
		{
			Log.Info($"Asserting that user list contains {count} lines with {source} source and {target} target");
			BaseElement.WaitForElementPresent(
				By.XPath(
					$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{source.ToLower()}')]]/ancestor::tr//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{target.ToLower()}')]]/ancestor::tr"),
				"");
			Assert.IsTrue(
				Browser.GetDriver()
					.FindElements(
						By.XPath(
							$"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{source.ToLower()}')]]/ancestor::tr//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{target.ToLower()}')]]/ancestor::tr"))
					.Count == count, "Invalid count of lines");
		}

		public void VerifyLinesCountAndProperties(string group, int count)
		{
			Log.Info($"Asserting that user list contains {count} lines with {group} locator");
			var linesLabel = new Label(
				By.XPath(
					$"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//*[text()[contains(.,'{group}')]]/ancestor::tr"),
				"");
			linesLabel.WaitForSeveralElementsPresent(count);
			Assert.IsTrue(
				Browser.GetDriver()
					.FindElements(
						By.XPath(
							$"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//*[text()[contains(.,'{group}')]]/ancestor::tr"))
					.Count == count, "Invalid count of lines");
		}

        public void CheckMigrationWavesIsVisible()
        {
            Assert.IsTrue(migrationWaves.IsElementVisible());
        }

        public void OpenMigrationwaves()
        {
            Log.Info("Opening Migration waves");
            migrationWaves.Click();
        }

        public void CheckNewMigrationWaveButtonIsVisible()
        {
            Assert.IsTrue(newMigrationWave.IsElementVisible());
        }

        public void PerfomActionForGroup(string locator, ActionType type)
        {
            ScrollToTop();
            Log.Info(type + " user by locator: " + locator);
            WaitForAjaxLoad();
            SelectEntryBylocator(locator);
            SelectGroupAction(type);
            Apply();
        }

        public void SelectGroupAction(ActionType type)
        {
            OpenGroupActionsDropdown();
            ChooseAction(type.GetValue());
        }

        private void OpenGroupActionsDropdown()
        {
            Log.Info("Opening Actions dropdown");
            ScrollToElement(groupActionsDropdownButton.GetElement());
            groupActionsDropdownButton.Click();
            try
            {
                expandedActionsDropdownButton.WaitForElementPresent(5000);
            }
            catch (Exception)
            {
                Log.Info("Actions dropdown is not ready");
                groupActionsDropdownButton.Click();
            }
        }

        public void VerifyLinesCount(int count)
		{
			Log.Info("Verifying lines count is equal to: " + count);
			var locator = By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr");
			var lineButton = new Button(locator, "");
			lineButton.WaitForSeveralElementsPresent(count);
			Assert.IsTrue(Browser.GetDriver().FindElements(locator).Count == count);
		}

		public void AssertSourceSorted()
		{
			Log.Info("Asserting source is sorted");
			BaseElement.WaitForElementIsClickable(
				By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
			var existingList =
				GetEntriesText(
					Browser.GetDriver()
						.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
					"source");
			var sortedList = new List<string>();
			if (sortedAscSourceButton.IsPresent())
			{
				sortedList = Store.SourceList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = Store.SourceList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Source sorting error");
			}
		}

		public void AssertTargetSorted()
		{
			Log.Info("Asserting target sorted");
			BaseElement.WaitForElementIsClickable(
				By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
			var existingList =
				GetEntriesText(
					Browser.GetDriver()
						.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
					"target");
			var sortedList = new List<string>();
			if (sortedAscTargetButton.IsPresent())
			{
				sortedList = Store.TargetList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = Store.TargetList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Target sorting error");
			}
		}

		public void AssertGroupSorted()
		{
			Log.Info("Asserting group sorted");
			BaseElement.WaitForElementIsClickable(
				By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
			var existingList =
				GetEntriesText(
					Browser.GetDriver()
						.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
					"group");
			var sortedList = new List<string>();
			if (sortedAscGroupButton.IsPresent())
			{
				sortedList = Store.GroupList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = Store.GroupList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Group sorting error");
			}
		}

		public void AssertProfileSorted()
		{
			Log.Info("Asserting profile sorted");
			BaseElement.WaitForElementIsClickable(
				By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
			var existingList =
				GetEntriesText(
					Browser.GetDriver()
						.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
					"profile");
			var sortedList = new List<string>();
			if (sortedAscProfileButton.IsPresent())
			{
				sortedList = Store.ProfileList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = Store.ProfileList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Profile sorting error");
			}
		}

		public void AssertGroupIsFilteredFor(string group)
		{
			Log.Info("Asserting group is filtered for: " + group);
			foreach (var groupEntry in Store.GroupList)
			{
				Assert.IsTrue(string.Equals(groupEntry.Trim(), @group.Trim(), StringComparison.CurrentCultureIgnoreCase),
					"Group is not filtered properly");
			}
		}

		public void AssertProfileIsFilteredFor(string profile)
		{
			Log.Info("Asserting profile is filtered for: " + profile);
			foreach (var profileEntry in Store.ProfileList)
			{
				Assert.IsTrue(string.Equals(profileEntry.Trim(), @profile.Trim(), StringComparison.CurrentCultureIgnoreCase),
					"Profile is not filtered properly");
			}
		}

		public void AssertStatusSorted()
		{
			Log.Info("Asserting State sorted");
			BaseElement.WaitForElementIsClickable(
				By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
			var existingList =
				GetEntriesText(
					Browser.GetDriver()
						.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
					"state");
			var sortedList = new List<string>();
			if (sortedAscStateButton.IsPresent())
			{
				sortedList = Store.StateList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = Store.StateList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "State sorting error");
			}
		}

		public void AssertStateIsFilteredFor(string state)
		{
			Log.Info("Asserting state is filtered for: " + state);
			foreach (var stateEntry in Store.StateList)
			{
				Assert.IsTrue(stateEntry.Trim().ToLower() == state.Trim().ToLower(), "State is not filtered properly");
			}
		}

		public void AssertMailboxesSorted()
		{
			Log.Info("Asserting mailbox sorted");
			BaseElement.WaitForElementIsClickable(
				By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
			var existingList =
				GetEntriesText(
					Browser.GetDriver()
						.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
					"mailbox");
			var sortedList = new List<string>();
			if (sortedAscMailboxSizeButton.IsPresent())
			{
				sortedList = Store.MailboxSizeList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = Store.MailboxSizeList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Mailbox size sorting error");
			}
		}

		public void AssertArchivesSorted()
		{
			Log.Info("Asserting archives sorted");
			BaseElement.WaitForElementIsClickable(
				By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
			var existingList =
				GetEntriesText(
					Browser.GetDriver()
						.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
					"archive");
			var sortedList = new List<string>();
			if (sortedAscArchiveSizeButton.IsPresent())
			{
				sortedList = Store.MailboxSizeList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = Store.MailboxSizeList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Archive sorting error");
			}
		}

		public void StoreEntriesData()
		{
            WaitForAjaxLoad();
            Store.SourceList.Clear();
			Store.TargetList.Clear();
			Store.StateList.Clear();
			Store.ProgressList.Clear();
			Store.GroupList.Clear();
			Store.MailboxSizeList.Clear();
			Store.ArchiveSizeList.Clear();
			Store.ProfileList.Clear();
			BaseElement.WaitForElementIsClickable(
				By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr"));
           
            var elements = Browser.GetDriver()
				.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr"));
                     foreach (var element in elements)
			{
				Store.SourceList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Source')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
				Store.TargetList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Target')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
				Store.StateList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Status')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
				Store.GroupList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Wave')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
				Store.ProfileList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Profile')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
				try
				{
					Store.MailboxSizeList.Add(
						element.FindElement(
								By.XPath(
									".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Size')]]/ancestor::th/preceding::th)]//*[normalize-space(text())]"))
							.Text);
				}
				catch (Exception)
				{
					Log.Info("Empty size");
				}
				try
				{
					Store.ArchiveSizeList.Add(
						element.FindElement(
								By.XPath(
									".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Size')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
							.Text);
				}
				catch (Exception)
				{
					Log.Info("Empty size");
				}
			}
		}

		private List<string> GetEntriesText(ReadOnlyCollection<IWebElement> elements, string cell)
		{
			var resultList = new List<string>();
			if (cell == "source")
			{
				foreach (var element in elements)
				{
					resultList.Add(
						element.FindElement(
							By.XPath(
								".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Source')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
							.Text);
				}
			}
			if (cell == "target")
			{
				foreach (var element in elements)
				{
					resultList.Add(
						element.FindElement(
							By.XPath(
								".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Target')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
							.Text);
				}
			}
			if (cell == "state")
			{
				foreach (var element in elements)
				{
					resultList.Add(
						element.FindElement(
							By.XPath(
								".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Status')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
							.Text);
				}
			}
			if (cell == "group")
			{
				foreach (var element in elements)
				{
					resultList.Add(
						element.FindElement(
							By.XPath(
								".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Wave')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
							.Text);
				}
			}
			if (cell == "mailbox")
			{
				foreach (var element in elements)
				{
					resultList.Add(
						element.FindElement(
							By.XPath(
								".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Size')]]/ancestor::th/preceding::th)]//*[normalize-space(text())]"))
							.Text);
				}
			}
			if (cell == "archive")
			{
				foreach (var element in elements)
				{
					resultList.Add(
						element.FindElement(
							By.XPath(
								".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Size')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
							.Text);
				}
			}
			if (cell == "profile")
			{
				foreach (var element in elements)
				{
					resultList.Add(
						element.FindElement(
							By.XPath(
								".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Profile')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
							.Text);
				}
			}
			return resultList;
		}

		public void WaitUntilMailboxesSizeAppear()
		{
			Log.Info("Waiting until mailboxes size appear");
			var counter = 0;
			while (!IsMailboxesSizeEmpty() && counter < 31)
			{
				Thread.Sleep(20000);
				Browser.GetDriver().Navigate().Refresh();
				Thread.Sleep(10000);
			}
		}

		public void WaitUntilArchiveSizeAppear()
		{
			Log.Info("Waiting until archive size appear");
			var counter = 0;
			while (!IsArchiveSizeEmpty() && counter < 31)
			{
				Thread.Sleep(20000);
				Browser.GetDriver().Navigate().Refresh();
				Thread.Sleep(10000);
			}
		}

		private bool IsMailboxesSizeEmpty()
		{
			StoreEntriesData();
			var result = true;
			foreach (var size in Store.MailboxSizeList)
			{
				if (!size.Trim().ToLower().Contains("b"))
				{
					result = false;
				}
			}
			return result;
		}

		private bool IsArchiveSizeEmpty()
		{
			StoreEntriesData();
			var result = true;
			foreach (var size in Store.ArchiveSizeList)
			{
				if (!size.Trim().ToLower().Contains("b"))
				{
					result = false;
				}
			}
			return result;
		}

		#region [Group filter]

		public void SelectFilterGroup(string group)
		{
			Log.Info($"Selecting {group}");
			var itemButton = new Button(By.XPath($"//div[contains(@class, 'checkbox')]//*[contains(text(), '{group}')]"), $"{group} button");
			RadioButton itemRadioButton = new RadioButton(By.XPath($"//*[contains(text(),'{group}')]/ancestor::div[contains(@class, 'checkbox')]//input"), group+" radiobutton");
			itemButton.Click();
			try
			{
				itemRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				itemButton.Click();
			}
            WaitForAjaxLoad();
		}

		public void CheckFilterGroup(string group)
		{
			Log.Info($"Selecting {group}");
			var itemButton = new Button(By.XPath($"//div[contains(@class, 'checkbox')]//*[contains(text(), '{group}')]"), $"{group} button");
			RadioButton itemRadioButton = new RadioButton(By.XPath($"//*[contains(text(),'{group}')]/ancestor::div[contains(@class, 'checkbox')]//input"), group + " radiobutton");
			itemButton.Click();
			try
			{
				itemRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				itemButton.Click();
			}
            WaitForAjaxLoad();
		}

		public void UncheckFilterGroup(string group)
		{
			Log.Info($"Selecting {group}");
			var itemButton = new Button(By.XPath($"//div[contains(@class, 'checkbox')]//*[contains(text(), '{group}')]"), $"{group} button");
			RadioButton itemRadioButton = new RadioButton(By.XPath($"//*[contains(text(),'{group}')]/ancestor::div[contains(@class, 'checkbox')]//input"), group + " radiobutton");
			itemButton.Click();
			try
			{
				itemRadioButton.WaitForUnselected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				itemButton.Click();
			}
            WaitForAjaxLoad();
        }

		#endregion

		#region[Add to wave]

		private readonly Button enabledAddToWaveButton =
			new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'addToMigrationGroup')][not(@disabled='')]"),
				"Enabled Add to wave button");

		public void SelectMigrationGroup(string group)
		{
			Log.Info("Selecting migration group: " + group);
			var migrationGroupButton =
				new Button(By.XPath($"//div[contains(@class, 'modal in')]//*[contains(text(), '{group}')]"),
					group + " migration group button");
			migrationGroupButton.Click();
			try
			{
				enabledAddToWaveButton.WaitForElementPresent(4000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				migrationGroupButton.Click();
			}
		}

		public void AddToWave()
		{
			Log.Info("Adding to wave");
			enabledAddToWaveButton.Click();
		}

		#endregion

		#region [State filter]

		private readonly Button notReadyButton = new Button(By.XPath("//label[contains(@for, 'Not Ready1')]"),
			"Not ready checkbox");

		private readonly Button inProgressButton = new Button(By.XPath("//label[contains(@for, 'In Progress3')]"),
			"In progress checkbox");

		private readonly Button errorButton = new Button(By.XPath("//label[contains(@for, 'Error5')]"), "Error checkbox");
		private readonly Button matchedButton = new Button(By.XPath("//label[contains(@for, 'Matched2')]"), "Matched checkbox");

		private readonly Button completeFilterButton = new Button(By.XPath("//label[contains(@for, 'Complete4')]"),
			"Complete checkbox");

		private readonly Button noneButton = new Button(By.XPath("//label[contains(@for, '1None0')]"), "None checkbox");

		private readonly Button notMatchButton = new Button(By.XPath("//label[contains(@for, '1Not Match1')]"),
			"Not match checkbox");

		private readonly Button multipleMatchesButton = new Button(By.XPath("//label[contains(@for, '1Multiple Matches2')]"),
			"Multiple matches checkbox");

		private readonly Button syncingButton = new Button(By.XPath("//label[contains(@for, '3Syncing6')]"),
			"Syncing checkbox");

		private readonly Button syncedButton = new Button(By.XPath("//label[contains(@for, '3Synced7')]"), "Synced checkbox");

		private readonly Button stoppingButton = new Button(By.XPath("//label[contains(@for, '3Stopping8')]"),
			"Stopping checkbox");

		private readonly Button licenseErrorButton = new Button(By.XPath("//label[contains(@for, '5License Error12')]"),
			"Licence error checkbox");

		private readonly Button syncErrorButton = new Button(By.XPath("//label[contains(@for, '5Sync Error13')]"),
			"Sync error checkbox");


		public void SetNotReady()
		{
			Log.Info("Checking Not ready checkbox");
			notReadyButton.Click();
		}

		public void SetInProgress()
		{
			Log.Info("Checking In progress checkbox");
			inProgressButton.Click();
		}

		public void SetError()
		{
			Log.Info("Checking Error checkbox");
			errorButton.Click();
		}

		public void SetMatched()
		{
			Log.Info("Checking Matched checkbox");
			matchedButton.Click();
            WaitForAjaxLoad();
        }

		public void SetComplete()
		{
			Log.Info("Checking Complete checkbox");
			completeButton.Click();
		}

        public void SetSynced()
        {
            Log.Info("Checking Synced checkbox");
            syncedButton.Click();
        }
        #endregion

        #region [User details]
        private readonly Button closeUserDetailsButton =
			new Button(
				By.XPath(
					"//div[contains(@class, 'modal in')]//div[contains(@class, 'modal-lg')]//button[contains(@data-dismiss, 'modal')][contains(@class, 'btn')]"),
				"Close user details button");

		private readonly Button refreshButton =
			new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'refresh')]"), "Refresh button");

		private Button logsButton =
			new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportSyncJobLogs')]"), "Logs button");

        private Button rollbackLogsButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportRollbackJobLogs')]"), "Rollback logs button");

		private readonly Label syncedStateLabel =
			new Label(By.XPath("//*[contains(@data-bind, 'migrationState')][contains(text(), 'complete')]"), "State label");

		private readonly Button completeButton =
			new Button(
				By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'complete')]"),
				"Complete button");

		private readonly Button enabledDetailsStopButton =
			new Button(
				By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'stop')]"),
				"Enabled details stop button");

		private readonly Button enabledDetailsSyncButton =
			new Button(
				By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'sync')]"),
				"Enabled details sync button");

        private readonly Button prepareDetailsButton =
            new Button(
                By.XPath("//div[contains(@class, 'modal in')]//button[contains(., 'Prepare')]"),
                "Enabled details Prepare button");

        private readonly Button disabledCutoverButton =
			new Button(By.XPath("//div[contains(@class, 'modal in')]//button//i[contains(@class, 'fa-mail-forward')]"),
				"Cutover button");

		private readonly Label dangerProgressBarLabel =
			new Label(By.XPath("//div[contains(@class, 'modal in')]//div[contains(@class, 'progress-bar-danger')]"),
				"Danger progress bar label");

        private readonly Button startedSortButton = new Button(By.XPath("//*[contains(text(), 'Started')]"), "Started sort button");

        private readonly Label migrationStateLabel = new Label(By.XPath("//*[contains(@data-bind, 'migrationState')]"),"State");

        public void CloseUserDetails()
		{
			Log.Info("Closing user details");
			closeUserDetailsButton.Click();
		}

	    public void WaitForJobSortedByTime()
	    {
	        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
	        DateTimeFormatInfo dtfi = culture.DateTimeFormat;
	        dtfi.DateSeparator = "/";
	        string localDate = DateTime.Now.ToString("MM/dd/yyyy h", dtfi);
            try
	        {
	            new Label(By.XPath($"//*[contains(text(), '{localDate}')]"), "").WaitForElementPresent(10000);
            }
	        catch (Exception)
	        {
	            SortStartedJobs();
	            new Label(By.XPath($"//*[contains(text(), '{localDate}')]"), "").WaitForElementPresent(10000);
            }
            
        }

	    public void SortStartedJobs()
	    {
            Log.Info("Sorting started jobs");
            startedSortButton.Click();
	    }

	    public void CompleteSync()
		{
			Log.Info("Completing job");
			completeButton.Click();
		}
		public void AssertDetailsStopButtonIsEnabled()
		{
			Log.Info("Asserting stop button is enabled");  
           Assert.IsTrue(enabledDetailsStopButton.WaitForElementPresent(), "Asserting stop button is not enabled");
		}

		public void AssertDetailsSyncButtonIsEnabled()
		{
			Log.Info("Asserting details sync button is enabled");
			enabledDetailsSyncButton.WaitForElementPresent();
		}
		public void AssertDetailsSyncButtonIsDisabled()
		{
			Log.Info("Asserting details sync button is disabled");
			Assert.IsTrue(enabledDetailsSyncButton.WaitForElementDisappear(), "Asserting details sync button is visible");
		}

		public void SyncFromDetails()
		{
			Log.Info("Syncing");
            WaitForAjaxLoad();
			enabledDetailsSyncButton.Click();
		}

        public void StopFromDetails()
        {
            Log.Info("Stoping");
            WaitForAjaxLoad();
            enabledDetailsStopButton.Click();
        }

        public void PrepareFromDetails()
        {
            Log.Info("Syncing");
            WaitForAjaxLoad();
            prepareDetailsButton.Click();
        }

        public void AssertCutoverButton()
		{
			Log.Info("Asserting Cutover button");
			disabledCutoverButton.WaitForElementPresent();
		}

        public string GetStateFromDetails()
        {
          Log.Info("Get mail state");         
          return  migrationStateLabel.GetText();
        }
              

        public void WaitForJobIsCreated()
		{
			Log.Info("Waiting for job is created");
			var jobLabel = new Label(By.XPath("//tr[contains(*, 'Sync')]"), "Job label");
			var counter = 0;
			while (!jobLabel.IsPresent() && counter < 30)
			{
				Thread.Sleep(30000);
				RefreshData();
				counter++;
				Assert.IsTrue(!dangerProgressBarLabel.IsPresent(), "SYNC ERROR!");
			}
			jobLabel.WaitForElementPresent();
			Assert.IsTrue(!dangerProgressBarLabel.IsPresent(), "SYNC ERROR!");
		}

	    public void WaitForSyncJobAppear(int count)
	    {
	        Log.Info("Waiting for several jobs are created: "+count);
	        var jobLabel = new Label(By.XPath("//tr[contains(*, 'Sync')]"), "Job label");
            bool ready = false;
	        int counter = 0;
	        while (!ready && counter < 20)
	        {
	            try
	            {
	                jobLabel.WaitForSeveralElementsPresent(count, 30000);
	                ready = true;
	            }
	            catch (Exception)
	            {
	                counter++;
	                RefreshData();
	            }
	        }
	    }
        public void WaitForSyncedState()
		{
			Log.Info("Waiting for synced state");
			var counter = 0;
			while (!syncedStateLabel.IsPresent() && counter < 60)
			{
				Thread.Sleep(30000);
				RefreshData();
				counter++;
				Assert.IsTrue(!dangerProgressBarLabel.IsPresent(), "SYNC ERROR!");
			}
			syncedStateLabel.WaitForElementPresent();
			Assert.IsTrue(!dangerProgressBarLabel.IsPresent(), "SYNC ERROR!");
		}

		public void DownloadLogs()
		{
			Log.Info("Downloading logs");
		    try
		    {
		        logsButton.Click();
            }
		    catch (Exception)
		    {
		       Log.Info("First click failed");
		       logsButton =
		            new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportSyncJobLogs')]"), "Logs button");
		       logsButton.Click();
            }
			
		}

	    public void DownloadRollbackLogs()
	    {
	        Log.Info("Downloading rollback logs");
	        try
	        {
	            rollbackLogsButton.Click();
	        }
	        catch (Exception)
	        {
	            Log.Info("First click failed");
	            rollbackLogsButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportRollbackJobLogs')]"), "Rollback logs button");
                rollbackLogsButton.Click();
            }

	    }

        public void RefreshData()
		{
			Log.Info("Refreshing data");
			refreshButton.Click();
		}

        public void WaitForState_DetailPage(string entry, State state,  int timeout = 5000, int pollIntervalSec = 0)
        {
            var _migrationStateTextLocatorFormat = "//*[contains(@data-bind, 'migrationState')][contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]"; 
            var value = state.GetValue();

            if (state.GetValue().ToLower() == "synced")
                value = "complete";

            var rowEntryTextValue = string.Format(_migrationStateTextLocatorFormat, value.ToLower());
            var stateLocator = By.XPath(rowEntryTextValue);
            var refreshElementTextValue = refreshButton.GetLocator();

            if (!IsElementExists(stateLocator, () => ClickElementBy(refreshElementTextValue), timeout / 1000, pollIntervalSec))
                throw new Exception(string.Format("Entry of '{0}' with state '{1}' was not found.", entry, value));
        }

        public void WaitForJobIsCreated(String entry, State state, int timeout = 5000, int pollIntervalSec = 0)
        {
            Log.Info("Waiting for job is created");
            var jobLabel ="//td[contains(*, '{0}')]";
            var value = state.GetValue();
          
            var rowEntryTextValue = string.Format(jobLabel, value);
            var stateLocator = By.XPath(rowEntryTextValue);
            var refreshElementTextValue = refreshButton.GetLocator();

            if (!IsElementExists(stateLocator, () => ClickElementBy(refreshElementTextValue), timeout / 1000, pollIntervalSec))
                throw new Exception(string.Format("Entry of '{0}' with state '{1}' was not found.", entry, value));
        }

        public void JobProgressBarShouldShownCorrectProgress(String state)
        {
            Log.Info("Verify progressBar 100 %");
           // var value = state.GetValue();

            var stateProgressBar = string.Format(ProgressBar_100, state);
            var progressBarLabel = new Label(By.XPath(stateProgressBar), "State {state}:Progress bar 100%") ;
            Assert.IsTrue(progressBarLabel.IsElementVisible(),"Progress bar 100% is not visible");
        }

        public bool VerifyStateIS(string state)
		{
           
            Log.Info("Verifying state is: " + state);
			var stateLabel = new Label(By.XPath($"//*[contains(@data-bind, 'migrationState')][contains(text(), '{state}')]"),
				"State label");
			var counter = 0;

			while (!stateLabel.IsPresent() && counter < 10)
			{
				Thread.Sleep(30000);
				RefreshData();
				counter++;
			}
			return stateLabel.WaitForElementPresent();

        }
        #endregion

	    #region [Rollback modal]

	    private readonly Label resetPermissionsLabel = new Label(By.XPath("//label[contains(@for, 'resetPermissions')]"), "Reset permissions label");
        private readonly RadioButton resetPermissionsRadioButton = new RadioButton(By.Id("resetPermissions"), "Reset permissions button");
        private readonly Label dontResetPermissionsLabel = new Label(By.XPath("//label[contains(@for, 'dontResetPermissions')]"), "Dont reset permissions label");
        private readonly RadioButton dontResetPermissionsRadioButton = new RadioButton(By.Id("dontResetPermissions"), "Dont reset permissions radiobutton");
        private readonly Label sureRollbackLabel = new Label(By.XPath("//label[contains(@for, 'rollbackCheckbox')]"), "Sure to rollback label");
        private readonly RadioButton sureRollbackRadioButton = new RadioButton(By.Id("rollbackCheckbox"), "Sure to rollback radiobutton");
        private readonly Button rollbackButton = new Button(By.XPath("//button[contains(@data-bind, 'rollback') and not(@disabled='')]"), "Rollback button");
        private readonly Button rollbackButton_modalWindow = new Button(By.XPath("//*[@class='modal in']//button[not(@disabled='')]/span[text()= 'Rollback']"), "Rollback button in modal window");
        private readonly Label rollbackWindowTitle = new Label(By.XPath("//*[@class='modal in']//span[text()= 'Perform a rollback']"), "Rollback modal window title");
        private readonly Button rollbackCancelButton_modalWindow = new Button(By.XPath("//*[@class='modal in']//button[not(@disabled='')]/span[text()= 'Cancel']"), "Rollback Cancel button in modal window");
        private readonly Label jobRollback = new Label(By.XPath("//tr[contains(*, 'Rollback In Progress')]"), "Job Rollback In Progress label");

        public void SetResetPermissions()
	    {
	        Log.Info("Setting reset permissions");
	        resetPermissionsLabel.Click();
	        try
	        {
	            resetPermissionsRadioButton.WaitForSelected(5000);
	        }
	        catch (Exception)
	        {
	            resetPermissionsLabel.Click();
	        }
	    }

	    public void SetDontResetPermissions()
	    {
	        Log.Info("Setting dont reset permissions");
	        dontResetPermissionsLabel.Click();
	        try
	        {
	            dontResetPermissionsRadioButton.WaitForSelected(5000);
	        }
	        catch (Exception)
	        {
	            dontResetPermissionsLabel.Click();
            }
	    }

	    public void SetSureCheckbox()
	    {
            Log.Info("Setting sure checkbox");
	        sureRollbackLabel.Click();
	        try
	        {
	            sureRollbackRadioButton.WaitForSelected(5000);
	        }
	        catch (Exception)
	        {
	            sureRollbackLabel.Click();
            }
        }

        public void AssertRollBackIsDisabled()
        {
            Log.Info("Clicking Rollback is disabled");
            Assert.IsFalse(rollbackButton.IsElementVisible(), "Rollback button is not disabled");
        }

        public void Rollback()
	    {
            Log.Info("Clicking Rollback");
            rollbackButton.Click();
	    }

        public void RollbackClick_modalWindow()
        {
            Log.Info("Clicking Rollback");
            rollbackButton_modalWindow.Click();
            WaitForAjaxLoad();
        }

        public void RollbackCancelClick_modalWindow()
        {
            Log.Info("Clicking Rollback");
            rollbackCancelButton_modalWindow.Click();
            WaitForAjaxLoad();
        }
        public void AssertRollbackModalWindowIsShown()
        {
           Assert.IsTrue(rollbackWindowTitle.IsElementVisible(), "Rollback button is not disabled");  
        }
        

        public void AssertState(State state)
        {
            string stateValue = state.GetValue();

            Log.Info($"Assert state {stateValue}");
            
            WaitForAjaxLoad();            
            Assert.IsTrue(VerifyStateIS(stateValue), $"State not equals {state}");
        }

        public void AssertRollBackJobNotStarted()
        {
            WaitForAjaxLoad();
            Assert.IsFalse(jobRollback.IsElementVisible(),"Rollback start" );
            AssertState(State.Synced);
            
            
        }
        #endregion
    }
}