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
        RunConfigurator configurator;

        Store store;

        private static readonly By TitleLocator =
			By.XPath("//div[@id='users']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]");

		private readonly Button actionsDropdownButton ;

		private readonly Button backToDashboardButton ;

		private readonly TextBox chooseFilesInput ;

		private readonly Button closeFilterButton ;

		private readonly Button closeModalWindowButton ;

		private readonly Button confirmYesButton ;

		private readonly Button disabledApplyActionButton ;

		private Button enabledApplyActionButton ;

		private readonly Button enabledArchiveButton ;

		private readonly Button enabledEditButton ;

		private readonly Button enabledExportButton ;

		protected readonly Button expandedActionsDropdownButton ;

		private readonly Button filterButton ;

		private readonly Button fixErrorsButton ;

		private readonly Button groupFilterButton ;

		private readonly Button importButton ;

		private readonly Label importedLabel ;

		private readonly Button okButton ;

		private readonly Button profileFilterButton ;

		private readonly Button sampleFileButton ;

		private readonly TextBox searchTextBox ;

		private readonly Button selectAllButton ;

		private readonly Label selectedFilesLabel ;

		private readonly Button sortArchiveSizeButton ;

		private readonly Button sortedAscArchiveSizeButton ;

		private readonly Button sortedAscGroupButton ;

		private readonly Button sortedAscMailboxSizeButton ;

		private readonly Button sortedAscProfileButton ;

		private readonly Button sortedAscSourceButton ;

		private readonly Button sortedAscStateButton ;

		private readonly Button sortedAscTargetButton ;

		private readonly Button sortGroupButton ;

		private readonly Button sortMailboxSizeButton ;

		private readonly Button sortProfileButton ;
	
		private readonly Button sortSourceButton ;

		private readonly Button sortStatusButton ;

		private readonly Button sortTargetButton ;

		private readonly Button stateFilterButton ;
        private Button enabledOkProfileButton ;

        private Button userDetailsRefreshButton ;
        private Button userDetailsCloseButton ;

        private readonly Label openedFilterLabel ;
		private readonly Label noDataLabel ;
        
        protected Label descriptionLabel ;
	    protected string ProfileRowLocator = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]";
        protected string ProfileModifyLocator = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]//*[contains(text(), 'Modify')]";
        protected string ProfileLabelLocator = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]//label";
        protected string ProfileRadioLocator = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), 'Defa')]]//input";        

        public UsersForm(Guid driverId,Store store) : base(TitleLocator, "Users list form",driverId)
		{           
            this.store = store;
            this.driverId = driverId;
            configurator = new RunConfigurator(store);
            actionsDropdownButton =new Button(By.XPath("//div[@id='users']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"),"Actions dropdown",driverId);
            backToDashboardButton = new Button(By.XPath("//button[contains(@data-bind, 'goToDashboard')]"), "Back to dashboard button",driverId);
            chooseFilesInput =new TextBox(By.XPath("//div[contains(@class, 'modal in')]//input[@type='file']"), "Choose files input",driverId);
            closeFilterButton =new Button(By.XPath("//div[@class='panel-footer']//button[text()='Close']"), "Close filter button",driverId);
            closeModalWindowButton =new Button(By.XPath("//div[contains(@class, 'modal fade in')]//div[@class='modal-footer']//button[text()='Close']"),"Close modal window button",driverId);
            confirmYesButton =new Button(By.XPath("//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), 'Yes')]"),"Confirm button",driverId);
            disabledApplyActionButton =new Button(By.XPath("//button[contains(@data-bind, 'applyAction')][@disabled='']"), "Disabled apply button",driverId);
            enabledApplyActionButton =new Button(By.XPath("//button[contains(@data-bind, 'applyAction')][not(@disabled='')]"), "Enabled apply button",driverId);
            enabledArchiveButton =new Button(By.XPath("//button[contains(text(), 'Archive')][not(@disabled='')]"), "Enabled archive button",driverId);
            enabledEditButton =new Button(By.XPath("//button[contains(text(), 'Edit')][not(@disabled='')]"), "Enabled edit button",driverId);
            enabledExportButton =new Button(By.XPath("//button[contains(text(), 'Export')][not(@disabled='')]"), "Enabled export button",driverId);
            expandedActionsDropdownButton =new Button(By.XPath("//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')][contains(@aria-expanded, 'true')]"),"Expanded actions dropdown",driverId);
            filterButton = new Button(By.XPath("//a//span[contains(text(), 'Filter')]"), "Filter button",driverId);
            fixErrorsButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'goFixErrors')]"),"Fix errors button",driverId);
            groupFilterButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'Group')]]/..//i[contains(@class, 'fa-filter')]"),"Group filter button",driverId);
            importButton = new Button(By.XPath("//a[contains(@data-bind, 'uploadUser')]"), "Import button",driverId);
            importedLabel = new Label(By.XPath("//h3[contains(text(), 'successfully imported')]"),"Imported label",driverId);
            okButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@class, 'pull-right')][not(@disabled='')]"),"OK button",driverId);
            profileFilterButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'Profile')]]/..//i[contains(@class, 'fa-sort')]"),"Profile filter button",driverId);
            sampleFileButton =new Button(By.XPath("//div[contains(@class, 'modal fade in')]//a[contains(@href, 'DownloadUserMigrationsForUpdateTemplate')]"),"Sample file button",driverId);
            searchTextBox = new TextBox(By.XPath("//div[contains(@class, 'search-group')]//input"),"Search textbox",driverId);
            selectAllButton =new Button(By.XPath("//th//input[contains(@data-bind, 'allSelectedChecked')]"), "Select all checkbox",driverId);
            selectedFilesLabel = new Label(By.XPath("//h3[contains(text(), 'Selected Files')]"),"Selected files label",driverId);
            sortArchiveSizeButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Archive')]]//i"),"Sort Archive size button",driverId);
            sortedAscArchiveSizeButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Archive')]]//i[contains(@class, 'fa-sort-asc')]"),"Archive size sorted ASC button",driverId);
            sortedAscGroupButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Wave')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),"Group sorted ASC button",driverId);
            sortedAscMailboxSizeButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//span[text()[contains(.,'Mailbox')]]//i[contains(@class, 'fa-sort-asc')]"),"Mailbox size sorted ASC button",driverId);
            sortedAscProfileButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Profile')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),"Profile sorted ASC button",driverId);
            sortedAscSourceButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Source')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),"Source sorted ASC button",driverId);
            sortedAscStateButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Status')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),"State sorted ASC button",driverId);
            sortedAscTargetButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Target')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"),"Target sorted ASC button",driverId);
            sortGroupButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Wave')]]/ancestor::th//i"),"Sort group button",driverId);
            sortMailboxSizeButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//span[text()[contains(.,'Mailbox')]]//i"),"Sort mailbox size button",driverId);
            sortProfileButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Profile')]]/ancestor::th//i"),"Sort profile button",driverId);
            sortSourceButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Source')]]/ancestor::th//i"),"Sort source button",driverId);
            sortStatusButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Status')]]/ancestor::th//i"),"Sort state button",driverId);
            sortTargetButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Target')]]/ancestor::th//i"),"Sort target button",driverId);
            stateFilterButton =new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'State')]]/..//i[contains(@class, 'fa-filter')]"),"State filter button",driverId);
            enabledOkProfileButton = new Button(By.XPath("//button[contains(@data-bind, 'addToMigrationProfile')][not(@disabled='')]"), "Enabled OK profile button",driverId);
            userDetailsRefreshButton = new Button(By.XPath("//button[contains(@data-bind, 'refresh.run')][not(@disabled='')]"), "User Details Refresh button",driverId);
            userDetailsCloseButton = new Button(By.XPath("//button[contains(@data-dismiss, 'modal')][not(@disabled='')]"), "User Details Close button",driverId);
            openedFilterLabel = new Label(By.XPath("//div[contains(@role, 'tooltip') and contains(@id, 'popover')]"), "Opened filter",driverId);
            noDataLabel = new Label(By.XPath("//td[contains(text(), 'No data available')]"), "No data available label",driverId);
            descriptionLabel = new Label(By.XPath("//*[contains(@data-bind, 'projectDescription')]"), "Description Label",driverId);
            enabledAddToWaveButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'addToMigrationGroup')][not(@disabled='')]"),"Enabled Add to wave button",driverId);
            closeUserDetailsButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//div[contains(@class, 'modal-lg')]//button[contains(@data-dismiss, 'modal')][contains(@class, 'btn')]"),"Close user details button",driverId);
            refreshButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'refresh')]"), "Refresh button",driverId);
            logsButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportSyncJobLogs')]"), "Logs button",driverId);
            rollbackLogsButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportRollbackJobLogs')]"), "Rollback logs button",driverId);
            syncedStateLabel =new Label(By.XPath("//*[contains(@data-bind, 'migrationState')][contains(text(), 'complete')]"), "State label",driverId);
            completeButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'complete')]"),"Complete button",driverId);
            enabledDetailsStopButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'stop')]"),"Enabled details stop button",driverId);
            enabledDetailsSyncButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'sync')]"),"Enabled details sync button",driverId);
            disabledCutoverButton =new Button(By.XPath("//div[contains(@class, 'modal in')]//button//i[contains(@class, 'fa-mail-forward')]"),"Cutover button",driverId);
            dangerProgressBarLabel =new Label(By.XPath("//div[contains(@class, 'modal in')]//div[contains(@class, 'progress-bar-danger')]"),"Danger progress bar label",driverId);
            startedSortButton = new Button(By.XPath("//*[contains(text(), 'Started')]"), "Started sort button",driverId);
            notReadyButton = new Button(By.XPath("//label[contains(@for, 'Not Ready1')]"),"Not ready checkbox",driverId);
            inProgressButton = new Button(By.XPath("//label[contains(@for, 'In Progress3')]"),"In progress checkbox",driverId);
            errorButton = new Button(By.XPath("//label[contains(@for, 'Error5')]"), "Error checkbox",driverId);
            matchedButton = new Button(By.XPath("//label[contains(@for, 'Matched2')]"), "Matched checkbox",driverId);
            completeFilterButton = new Button(By.XPath("//label[contains(@for, 'Complete4')]"),"Complete checkbox",driverId);
            noneButton = new Button(By.XPath("//label[contains(@for, '1None0')]"), "None checkbox",driverId);
            notMatchButton = new Button(By.XPath("//label[contains(@for, '1Not Match1')]"),"Not match checkbox",driverId);
            multipleMatchesButton = new Button(By.XPath("//label[contains(@for, '1Multiple Matches2')]"),"Multiple matches checkbox",driverId);
            syncingButton = new Button(By.XPath("//label[contains(@for, '3Syncing6')]"),"Syncing checkbox",driverId);
            syncedButton = new Button(By.XPath("//label[contains(@for, '3Synced7')]"), "Synced checkbox",driverId);
            stoppingButton = new Button(By.XPath("//label[contains(@for, '3Stopping8')]"),"Stopping checkbox",driverId);
            licenseErrorButton = new Button(By.XPath("//label[contains(@for, '5License Error12')]"),"Licence error checkbox",driverId);
            syncErrorButton = new Button(By.XPath("//label[contains(@for, '5Sync Error13')]"),"Sync error checkbox",driverId);
            resetPermissionsLabel = new Label(By.XPath("//label[contains(@for, 'resetPermissions')]"), "Reset permissions label",driverId);
            resetPermissionsRadioButton = new RadioButton(By.Id("resetPermissions"), "Reset permissions button",driverId);
            dontResetPermissionsLabel = new Label(By.XPath("//label[contains(@for, 'dontResetPermissions')]"), "Dont reset permissions label",driverId);
            dontResetPermissionsRadioButton = new RadioButton(By.Id("dontResetPermissions"), "Dont reset permissions radiobutton",driverId);
            sureRollbackLabel = new Label(By.XPath("//label[contains(@for, 'rollbackCheckbox')]"), "Sure to rollback label",driverId);
            rollbackButton = new Button(By.XPath("//button[contains(@data-bind, 'rollback') and not(@disabled='')]"), "Rollback button",driverId);
            sureRollbackRadioButton = new RadioButton(By.Id("rollbackCheckbox"), "Sure to rollback radiobutton",driverId);
            descriptionLabel.WaitForElementPresent();
		}

		public UsersForm(By locator, string name,Guid driverId) : base(locator, name,driverId)
		{
            this.driverId = driverId;
            actionsDropdownButton = new Button(By.XPath("//div[@id='users']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"), "Actions dropdown", driverId);
            backToDashboardButton = new Button(By.XPath("//button[contains(@data-bind, 'goToDashboard')]"), "Back to dashboard button", driverId);
            chooseFilesInput = new TextBox(By.XPath("//div[contains(@class, 'modal in')]//input[@type='file']"), "Choose files input", driverId);
            closeFilterButton = new Button(By.XPath("//div[@class='panel-footer']//button[text()='Close']"), "Close filter button", driverId);
            closeModalWindowButton = new Button(By.XPath("//div[contains(@class, 'modal fade in')]//div[@class='modal-footer']//button[text()='Close']"), "Close modal window button", driverId);
            confirmYesButton = new Button(By.XPath("//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), 'Yes')]"), "Confirm button", driverId);
            disabledApplyActionButton = new Button(By.XPath("//button[contains(@data-bind, 'applyAction')][@disabled='']"), "Disabled apply button", driverId);
            enabledApplyActionButton = new Button(By.XPath("//button[contains(@data-bind, 'applyAction')][not(@disabled='')]"), "Enabled apply button", driverId);
            enabledArchiveButton = new Button(By.XPath("//button[contains(text(), 'Archive')][not(@disabled='')]"), "Enabled archive button", driverId);
            enabledEditButton = new Button(By.XPath("//button[contains(text(), 'Edit')][not(@disabled='')]"), "Enabled edit button", driverId);
            enabledExportButton = new Button(By.XPath("//button[contains(text(), 'Export')][not(@disabled='')]"), "Enabled export button", driverId);
            expandedActionsDropdownButton = new Button(By.XPath("//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')][contains(@aria-expanded, 'true')]"), "Expanded actions dropdown", driverId);
            filterButton = new Button(By.XPath("//a//span[contains(text(), 'Filter')]"), "Filter button", driverId);
            fixErrorsButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'goFixErrors')]"), "Fix errors button", driverId);
            groupFilterButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'Group')]]/..//i[contains(@class, 'fa-filter')]"), "Group filter button", driverId);
            importButton = new Button(By.XPath("//a[contains(@data-bind, 'uploadUser')]"), "Import button", driverId);
            importedLabel = new Label(By.XPath("//h3[contains(text(), 'successfully imported')]"), "Imported label", driverId);
            okButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@class, 'pull-right')][not(@disabled='')]"), "OK button", driverId);
            profileFilterButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'Profile')]]/..//i[contains(@class, 'fa-sort')]"), "Profile filter button", driverId);
            sampleFileButton = new Button(By.XPath("//div[contains(@class, 'modal fade in')]//a[contains(@href, 'DownloadUserMigrationsForUpdateTemplate')]"), "Sample file button", driverId);
            searchTextBox = new TextBox(By.XPath("//div[contains(@class, 'search-group')]//input"), "Search textbox", driverId);
            selectAllButton = new Button(By.XPath("//th//input[contains(@data-bind, 'allSelectedChecked')]"), "Select all checkbox", driverId);
            selectedFilesLabel = new Label(By.XPath("//h3[contains(text(), 'Selected Files')]"), "Selected files label", driverId);
            sortArchiveSizeButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Archive')]]//i"), "Sort Archive size button", driverId);
            sortedAscArchiveSizeButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Archive')]]//i[contains(@class, 'fa-sort-asc')]"), "Archive size sorted ASC button", driverId);
            sortedAscGroupButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Wave')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"), "Group sorted ASC button", driverId);
            sortedAscMailboxSizeButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//span[text()[contains(.,'Mailbox')]]//i[contains(@class, 'fa-sort-asc')]"), "Mailbox size sorted ASC button", driverId);
            sortedAscProfileButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Profile')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"), "Profile sorted ASC button", driverId);
            sortedAscSourceButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Source')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"), "Source sorted ASC button", driverId);
            sortedAscStateButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Status')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"), "State sorted ASC button", driverId);
            sortedAscTargetButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Target')]]/ancestor::th//i[contains(@class, 'fa-sort-asc')]"), "Target sorted ASC button", driverId);
            sortGroupButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Wave')]]/ancestor::th//i"), "Sort group button", driverId);
            sortMailboxSizeButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//span[text()[contains(.,'Mailbox')]]//i"), "Sort mailbox size button", driverId);
            sortProfileButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Profile')]]/ancestor::th//i"), "Sort profile button", driverId);
            sortSourceButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Source')]]/ancestor::th//i"), "Sort source button", driverId);
            sortStatusButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Status')]]/ancestor::th//i"), "Sort state button", driverId);
            sortTargetButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead//*[text()[contains(.,'Target')]]/ancestor::th//i"), "Sort target button", driverId);
            stateFilterButton = new Button(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//span[text()[contains(.,'State')]]/..//i[contains(@class, 'fa-filter')]"), "State filter button", driverId);
            enabledOkProfileButton = new Button(By.XPath("//button[contains(@data-bind, 'addToMigrationProfile')][not(@disabled='')]"), "Enabled OK profile button", driverId);
            userDetailsRefreshButton = new Button(By.XPath("//button[contains(@data-bind, 'refresh.run')][not(@disabled='')]"), "User Details Refresh button", driverId);
            userDetailsCloseButton = new Button(By.XPath("//button[contains(@data-dismiss, 'modal')][not(@disabled='')]"), "User Details Close button", driverId);
            openedFilterLabel = new Label(By.XPath("//div[contains(@role, 'tooltip') and contains(@id, 'popover')]"), "Opened filter", driverId);
            noDataLabel = new Label(By.XPath("//td[contains(text(), 'No data available')]"), "No data available label", driverId);
            descriptionLabel = new Label(By.XPath("//*[contains(@data-bind, 'projectDescription')]"), "Description Label", driverId);
            enabledAddToWaveButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'addToMigrationGroup')][not(@disabled='')]"), "Enabled Add to wave button", driverId);
            closeUserDetailsButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//div[contains(@class, 'modal-lg')]//button[contains(@data-dismiss, 'modal')][contains(@class, 'btn')]"), "Close user details button", driverId);
            refreshButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(@data-bind, 'refresh')]"), "Refresh button", driverId);
            logsButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportSyncJobLogs')]"), "Logs button", driverId);
            rollbackLogsButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportRollbackJobLogs')]"), "Rollback logs button", driverId);
            syncedStateLabel = new Label(By.XPath("//*[contains(@data-bind, 'migrationState')][contains(text(), 'complete')]"), "State label", driverId);
            completeButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'complete')]"), "Complete button", driverId);
            enabledDetailsStopButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'stop')]"), "Enabled details stop button", driverId);
            enabledDetailsSyncButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'sync')]"), "Enabled details sync button", driverId);
            disabledCutoverButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button//i[contains(@class, 'fa-mail-forward')]"), "Cutover button", driverId);
            dangerProgressBarLabel = new Label(By.XPath("//div[contains(@class, 'modal in')]//div[contains(@class, 'progress-bar-danger')]"), "Danger progress bar label", driverId);
            startedSortButton = new Button(By.XPath("//*[contains(text(), 'Started')]"), "Started sort button", driverId);
            notReadyButton = new Button(By.XPath("//label[contains(@for, 'Not Ready1')]"), "Not ready checkbox", driverId);
            inProgressButton = new Button(By.XPath("//label[contains(@for, 'In Progress3')]"), "In progress checkbox", driverId);
            errorButton = new Button(By.XPath("//label[contains(@for, 'Error5')]"), "Error checkbox", driverId);
            matchedButton = new Button(By.XPath("//label[contains(@for, 'Matched2')]"), "Matched checkbox", driverId);
            completeFilterButton = new Button(By.XPath("//label[contains(@for, 'Complete4')]"), "Complete checkbox", driverId);
            noneButton = new Button(By.XPath("//label[contains(@for, '1None0')]"), "None checkbox", driverId);
            notMatchButton = new Button(By.XPath("//label[contains(@for, '1Not Match1')]"), "Not match checkbox", driverId);
            multipleMatchesButton = new Button(By.XPath("//label[contains(@for, '1Multiple Matches2')]"), "Multiple matches checkbox", driverId);
            syncingButton = new Button(By.XPath("//label[contains(@for, '3Syncing6')]"), "Syncing checkbox", driverId);
            syncedButton = new Button(By.XPath("//label[contains(@for, '3Synced7')]"), "Synced checkbox", driverId);
            stoppingButton = new Button(By.XPath("//label[contains(@for, '3Stopping8')]"), "Stopping checkbox", driverId);
            licenseErrorButton = new Button(By.XPath("//label[contains(@for, '5License Error12')]"), "Licence error checkbox", driverId);
            syncErrorButton = new Button(By.XPath("//label[contains(@for, '5Sync Error13')]"), "Sync error checkbox", driverId);
            resetPermissionsLabel = new Label(By.XPath("//label[contains(@for, 'resetPermissions')]"), "Reset permissions label", driverId);
            resetPermissionsRadioButton = new RadioButton(By.Id("resetPermissions"), "Reset permissions button", driverId);
            dontResetPermissionsLabel = new Label(By.XPath("//label[contains(@for, 'dontResetPermissions')]"), "Dont reset permissions label", driverId);
            dontResetPermissionsRadioButton = new RadioButton(By.Id("dontResetPermissions"), "Dont reset permissions radiobutton", driverId);
            sureRollbackLabel = new Label(By.XPath("//label[contains(@for, 'rollbackCheckbox')]"), "Sure to rollback label", driverId);
            rollbackButton = new Button(By.XPath("//button[contains(@data-bind, 'rollback') and not(@disabled='')]"), "Rollback button", driverId);
            sureRollbackRadioButton = new RadioButton(By.Id("rollbackCheckbox"), "Sure to rollback radiobutton", driverId);
            descriptionLabel.WaitForElementPresent();
        }


        public void StopSyncing()
        {
            Log.Info("Stoping syncing");
            enabledDetailsStopButton.Click();
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
            new Button(By.XPath(String.Format(ProfileModifyLocator, profile)), "Profile modify button",driverId).Click();
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
	        Label profileLabel = new Label(By.XPath(String.Format(ProfileLabelLocator, profile)), profile + " profile label",driverId);
	        RadioButton profileRadioButton = new RadioButton(By.XPath(String.Format(ProfileRadioLocator, profile)), profile+" profile radiobutton",driverId);
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
					action + " option",driverId);
			actionButton.Click();
		}

		public void WaitForRowsAppear()
		{
			Log.Info("Waiting till rows appear");
			Label rowLabel = new Label(By.XPath("//td[contains(@data-bind, 'showDetails')]"), "Row label",driverId);
			rowLabel.WaitForElementPresent();
		}

		public void SelectAction(ActionType type)
		{
			OpenActionsDropdown();
			ChooseAction(type.GetValue());
		}

		public void AssertImportFailed()
		{
			Log.Info("Asserting import failed");
			fixErrorsButton.WaitForElementPresent();
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
            return new Label(By.XPath(locator), name,driverId).IsPresent();
        }

		public void OpenDetailsByLocator(string locator)
		{
             Log.Info($"Opening details by locator: {locator}");
            //BaseElement.WaitForElementPresent(By.XPath(
            //            $"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
            //        $"{locator} details button");
            new Element(By.XPath($"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"), $"{locator} details button", driverId).WaitForElementPresent();
            var detailsButton =
				new Button(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
					$"{locator} details button",driverId);
			ScrollToElement(detailsButton.GetElement());
			detailsButton.DoubleClick();
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
            //((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 10)");
            ((IJavaScriptExecutor)Driver.GetDriver(driverId)).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 10)");
        }

		public new void ScrollToTop()
		{
			Log.Info("Scrolling to the Top of the page");
            //((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("window.scrollTo(0, 0)");
            ((IJavaScriptExecutor)Driver.GetDriver(driverId)).ExecuteScript("window.scrollTo(0, 0)");
        }

		public void ScrollToElement(IWebElement element)
		{
            Log.Info("Scrolling to element: "+element);
            //((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView(false);", element);
            ((IJavaScriptExecutor)Driver.GetDriver(driverId)).ExecuteScript("arguments[0].scrollIntoView(false);", element);
        }

		public void SelectEntryBylocator(string locator)
		{
            // NOTE: disbled scrolling as obsolete 
			// ScrollToTop();
			Log.Info("Selecting checkbox for: " + locator);
            //BaseElement.WaitForElementPresent(By.XPath(
            //			$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//input"),
            //		locator + " entry checkbox");
            new Element(By.XPath($"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//input"), locator + " entry checkbox", driverId).WaitForElementPresent();
            var entryCheckboxButton =
				new RadioButton(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//input"),
					locator + " entry checkbox",driverId);
			var entryLabel = new Button(By.XPath(
					$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
				locator + " entry label",driverId);
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
        //@@@ Needs overhaul

        private readonly string _rowInputAncestor = "/ancestor::tr//input";
        private readonly string _rowTextAncestorFormat = "/ancestor::tr//*[contains(text(), '{0}')]";
        private readonly string _lowerCaseTextLocatorFormat = "//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]]";

        public void WaitForState(string entry, State state, int timeout = 5000, int pollIntervalSec = 0)
        {
            var value = state.GetValue();
            var rowEntryLocator = string.Format(_lowerCaseTextLocatorFormat, entry.ToLowerInvariant());
            var rowTextAncestorLocator = string.Format(_rowTextAncestorFormat, value);
            var rowEntryTextLocator = string.Format("{0}{1}", rowEntryLocator, rowTextAncestorLocator);

            var rowEntryTextValue = By.XPath(rowEntryTextLocator);
            if (!IsElementExists(rowEntryTextValue, timeout / 1000, pollIntervalSec))
                throw new Exception(string.Format("Entry of '{0}' with state '{1}' was not found.", entry, value));
        }


		public void WaitForState2(string locator, State state, int timeout)
		{
			Log.Info($"Waiting for line with {locator} locator in {state.GetValue()} state");
                     
            var entryLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.Trim().ToLower()}')]]/ancestor::tr//*[text()='{state.GetValue()}']"),
					"",driverId);
            var entryNonStateLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.Trim().ToLower()}')]]/ancestor::tr"),
					"",driverId);
			var counter = 0;
			if (state.GetValue().ToLower() == "synced")
			{
				var twiceSyncedLabel =
					new Label(
						By.XPath(
							$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//*[text()[contains(.,'complete')]]"),
						"",driverId);
				while (!entryLabel.IsPresent() && !twiceSyncedLabel.IsPresent() && counter < 40)
				{
					Thread.Sleep(timeout);
                    //Browser.GetDriver().Navigate().Refresh();
                    Driver.GetDriver(driverId).Navigate().Refresh();
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
                    //Browser.GetDriver().Navigate().Refresh();
                    Driver.GetDriver(driverId).Navigate().Refresh();
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
		            new Button(By.XPath("//button[contains(@data-bind, 'applyAction')][not(@disabled='')]"), "Enabled apply button",driverId);
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
					locator + " line",driverId);
			lineLabel.WaitForElementPresent();
		}

	    public void VerifyLineisExist(string locator, int count)
		{
			Log.Info("Verifying line exists by locator: " + locator);
			var lineLabel =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
					locator + " line",driverId);
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
					locator + " line",driverId);
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
					locator + " line",driverId);
			lineLabel.WaitForElementDisappear();
		}

		public void SyncUserByLocator(string locator)
		{
			ScrollToTop();
			Log.Info("Syncing user by locator: " + locator);
            WaitForAjaxLoad();
			SelectEntryBylocator(locator);
			SelectAction(ActionType.Sync);
			Apply();
		}

        public void RollbackUserByLocator(string locator)
        {
            ScrollToTop();
            Log.Info("Syncing user by locator: " + locator);
            WaitForAjaxLoad();
            SelectEntryBylocator(locator);
            SelectAction(ActionType.Rollback);
            Apply();
        }

        public void AssertUserHaveSyncingState(string locator)
		{
			Log.Info($"Asserting user with locator: {locator} contains syncing state");
			var syncingState =
				new Label(
					By.XPath(
						$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.Trim().ToLower()}')]]/ancestor::tr//*[text()='Syncing']"),
					locator + " syncing state",driverId);
			syncingState.WaitForElementPresent();
		}

        public void AssertUserHasState(string locator, string state)
        {
            Log.Info($"Asserting user with locator: {locator} contains syncing state");
            var syncingState =
                new Label(
                    By.XPath(
                        $"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.Trim().ToLower()}')]]/ancestor::tr//*[text()='{state}']"),
                    locator + " syncing state",driverId);
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
			chooseFilesInput.GetElement().SendKeys(Path.GetFullPath(configurator.ResourcesPath+ fileName));
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
            //BaseElement.WaitForElementPresent(
            //	By.XPath(
            //		$"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{source.ToLower()}')]]/ancestor::tr//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{target.ToLower()}')]]/ancestor::tr"),
            //	"");
            new Element(By.XPath($"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{source.ToLower()}')]]/ancestor::tr//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{target.ToLower()}')]]/ancestor::tr"), "", driverId).WaitForElementPresent();
            //Assert.IsTrue(
            //	Browser.GetDriver()
            //		.FindElements(
            //			By.XPath(
            //				$"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{source.ToLower()}')]]/ancestor::tr//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{target.ToLower()}')]]/ancestor::tr"))
            //		.Count == count, "Invalid count of lines");
            Assert.IsTrue(
                Driver.GetDriver(driverId)
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
				"",driverId);
			linesLabel.WaitForSeveralElementsPresent(count);
            //Assert.IsTrue(
            //	Browser.GetDriver()
            //		.FindElements(
            //			By.XPath(
            //				$"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//*[text()[contains(.,'{group}')]]/ancestor::tr"))
            //		.Count == count, "Invalid count of lines");
            Assert.IsTrue(
                Driver.GetDriver(driverId)
                    .FindElements(
                        By.XPath(
                            $"//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//*[text()[contains(.,'{group}')]]/ancestor::tr"))
                    .Count == count, "Invalid count of lines");
        }

		public void VerifyLinesCount(int count)
		{
			Log.Info("Verifying lines count is equal to: " + count);
			var locator = By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr");
			var lineButton = new Button(locator, "",driverId);
			lineButton.WaitForSeveralElementsPresent(count);
            //Assert.IsTrue(Browser.GetDriver().FindElements(locator).Count == count);
            Assert.IsTrue(Driver.GetDriver(driverId).FindElements(locator).Count == count);
        }

		public void AssertSourceSorted()
		{
			Log.Info("Asserting source is sorted");
            //BaseElement.WaitForElementIsClickable(
            //	By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
            new Element(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"), "", driverId).WaitForElementPresent();
            //var existingList =
            //	GetEntriesText(
            //		Browser.GetDriver()
            //			.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
            //		"source");
            var existingList =
                GetEntriesText(
                    Driver.GetDriver(driverId)
                        .FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
                    "source");
            var sortedList = new List<string>();
			if (sortedAscSourceButton.IsPresent())
			{
				sortedList = store.SourceList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = store.SourceList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Source sorting error");
			}
		}

		public void AssertTargetSorted()
		{
			Log.Info("Asserting target sorted");
            //BaseElement.WaitForElementIsClickable(
            //	By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
            new Element(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"), "", driverId).WaitForElementPresent();
            //var existingList =
            //	GetEntriesText(
            //		Browser.GetDriver()
            //			.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
            //		"target");
            var existingList =
                GetEntriesText(
                    Driver.GetDriver(driverId)
                        .FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
                    "target");
            var sortedList = new List<string>();
			if (sortedAscTargetButton.IsPresent())
			{
				sortedList = store.TargetList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = store.TargetList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Target sorting error");
			}
		}

		public void AssertGroupSorted()
		{
			Log.Info("Asserting group sorted");
            //BaseElement.WaitForElementIsClickable(
            //	By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
            new Element(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"), "", driverId).WaitForElementPresent();
            //var existingList =
            //	GetEntriesText(
            //		Browser.GetDriver()
            //			.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
            //		"group");
            var existingList =
                GetEntriesText(
                    Driver.GetDriver(driverId)
                        .FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
                    "group");
            var sortedList = new List<string>();
			if (sortedAscGroupButton.IsPresent())
			{
				sortedList = store.GroupList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = store.GroupList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Group sorting error");
			}
		}

		public void AssertProfileSorted()
		{
			Log.Info("Asserting profile sorted");
            //BaseElement.WaitForElementIsClickable(
            //	By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
            new Element(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"), "", driverId).WaitForElementPresent();
            //var existingList =
            //	GetEntriesText(
            //		Browser.GetDriver()
            //			.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
            //		"profile");
            var existingList =
                GetEntriesText(
                    Driver.GetDriver(driverId)
                        .FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
                    "profile");
            var sortedList = new List<string>();
			if (sortedAscProfileButton.IsPresent())
			{
				sortedList = store.ProfileList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = store.ProfileList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Profile sorting error");
			}
		}

		public void AssertGroupIsFilteredFor(string group)
		{
			Log.Info("Asserting group is filtered for: " + group);
			foreach (var groupEntry in store.GroupList)
			{
				Assert.IsTrue(string.Equals(groupEntry.Trim(), @group.Trim(), StringComparison.CurrentCultureIgnoreCase),
					"Group is not filtered properly");
			}
		}

		public void AssertProfileIsFilteredFor(string profile)
		{
			Log.Info("Asserting profile is filtered for: " + profile);
			foreach (var profileEntry in store.ProfileList)
			{
				Assert.IsTrue(string.Equals(profileEntry.Trim(), @profile.Trim(), StringComparison.CurrentCultureIgnoreCase),
					"Profile is not filtered properly");
			}
		}

		public void AssertStatusSorted()
		{
			Log.Info("Asserting State sorted");
            //BaseElement.WaitForElementIsClickable(
            //	By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
            new Element(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"), "", driverId).WaitForElementPresent();
            //var existingList =
            //	GetEntriesText(
            //		Browser.GetDriver()
            //			.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
            //		"state");
            var existingList =
                GetEntriesText(
                    Driver.GetDriver(driverId)
                        .FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
                    "state");
            var sortedList = new List<string>();
			if (sortedAscStateButton.IsPresent())
			{
				sortedList = store.StateList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = store.StateList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "State sorting error");
			}
		}

		public void AssertStateIsFilteredFor(string state)
		{
			Log.Info("Asserting state is filtered for: " + state);
			foreach (var stateEntry in store.StateList)
			{
				Assert.IsTrue(stateEntry.Trim().ToLower() == state.Trim().ToLower(), "State is not filtered properly");
			}
		}

		public void AssertMailboxesSorted()
		{
			Log.Info("Asserting mailbox sorted");
            //BaseElement.WaitForElementIsClickable(
            //	By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
            new Element(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"), "", driverId).WaitForElementPresent();
            //var existingList =
            //	GetEntriesText(
            //		Browser.GetDriver()
            //			.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
            //		"mailbox");
            var existingList =
                GetEntriesText(
                    Driver.GetDriver(driverId)
                        .FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
                    "mailbox");
            var sortedList = new List<string>();
			if (sortedAscMailboxSizeButton.IsPresent())
			{
				sortedList = store.MailboxSizeList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = store.MailboxSizeList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Mailbox size sorting error");
			}
		}

		public void AssertArchivesSorted()
		{
			Log.Info("Asserting archives sorted");
            //BaseElement.WaitForElementIsClickable(
            //	By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"));
            new Element(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//tr"), "", driverId).WaitForElementPresent();
            //var existingList =
            //	GetEntriesText(
            //		Browser.GetDriver()
            //			.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
            //		"archive");
            var existingList =
                GetEntriesText(
                    Driver.GetDriver(driverId)
                        .FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr")),
                    "archive");
            var sortedList = new List<string>();
			if (sortedAscArchiveSizeButton.IsPresent())
			{
				sortedList = store.MailboxSizeList.OrderBy(i => i).ToList();
			}
			else
			{
				sortedList = store.MailboxSizeList.OrderByDescending(i => i).ToList();
			}
			for (var i = 0; i < existingList.Count; i++)
			{
				Assert.IsTrue(sortedList[i] == existingList[i], "Archive sorting error");
			}
		}

		public void StoreEntriesData()
		{
            store.SourceList.Clear();
            store.TargetList.Clear();
            store.StateList.Clear();
            store.ProgressList.Clear();
            store.GroupList.Clear();
            store.MailboxSizeList.Clear();
            store.ArchiveSizeList.Clear();
            store.ProfileList.Clear();
            //BaseElement.WaitForElementIsClickable(
            //	By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr"));
            new Element(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr"), "", driverId).WaitForElementPresent();
            //var elements = Browser.GetDriver()
            //	.FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr"));
            var elements = Driver.GetDriver(driverId)
                .FindElements(By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//tbody//td/ancestor::tr"));
            foreach (var element in elements)
			{
                store.SourceList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Source')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
                store.TargetList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Target')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
                store.StateList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Status')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
                store.GroupList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Wave')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
                store.ProfileList.Add(
					element.FindElement(
						By.XPath(
							".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Profile')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
						.Text);
				try
				{
                    store.MailboxSizeList.Add(
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
                    store.ArchiveSizeList.Add(
						element.FindElement(
								By.XPath(
									".//td[count(//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]//thead/tr/th//*[text()[contains(.,'Size')]]/ancestor::th/preceding::th)+1]//*[normalize-space(text())]"))
							.Text);
				}
				catch (Exception e)
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
                //Browser.GetDriver().Navigate().Refresh();
                Driver.GetDriver(driverId).Navigate().Refresh();
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
                //Browser.GetDriver().Navigate().Refresh();
                Driver.GetDriver(driverId).Navigate().Refresh();
                Thread.Sleep(10000);
			}
		}

		private bool IsMailboxesSizeEmpty()
		{
			StoreEntriesData();
			var result = true;
			foreach (var size in store.MailboxSizeList)
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
			foreach (var size in store.ArchiveSizeList)
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
			var itemButton = new Button(By.XPath($"//div[contains(@class, 'checkbox')]//*[contains(text(), '{group}')]"), $"{group} button",driverId);
			RadioButton itemRadioButton = new RadioButton(By.XPath($"//*[contains(text(),'{group}')]/ancestor::div[contains(@class, 'checkbox')]//input"), group+" radiobutton",driverId);
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
			var itemButton = new Button(By.XPath($"//div[contains(@class, 'checkbox')]//*[contains(text(), '{group}')]"), $"{group} button",driverId);
			RadioButton itemRadioButton = new RadioButton(By.XPath($"//*[contains(text(),'{group}')]/ancestor::div[contains(@class, 'checkbox')]//input"), group + " radiobutton",driverId);
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
		}

		public void UncheckFilterGroup(string group)
		{
			Log.Info($"Selecting {group}");
			var itemButton = new Button(By.XPath($"//div[contains(@class, 'checkbox')]//*[contains(text(), '{group}')]"), $"{group} button",driverId);
			RadioButton itemRadioButton = new RadioButton(By.XPath($"//*[contains(text(),'{group}')]/ancestor::div[contains(@class, 'checkbox')]//input"), group + " radiobutton",driverId);
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
		}

		#endregion

		#region[Add to wave]

		private readonly Button enabledAddToWaveButton ;

		public void SelectMigrationGroup(string group)
		{
			Log.Info("Selecting migration group: " + group);
			var migrationGroupButton =
				new Button(By.XPath($"//div[contains(@class, 'modal in')]//*[contains(text(), '{group}')]"),
					group + " migration group button",driverId);
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

		private readonly Button notReadyButton ;

		private readonly Button inProgressButton ;

		private readonly Button errorButton ;
		private readonly Button matchedButton ;

		private readonly Button completeFilterButton ;

		private readonly Button noneButton ;

		private readonly Button notMatchButton ;

		private readonly Button multipleMatchesButton ;

		private readonly Button syncingButton ;

		private readonly Button syncedButton ;

		private readonly Button stoppingButton ;

		private readonly Button licenseErrorButton ;

		private readonly Button syncErrorButton ;


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
		}

		public void SetComplete()
		{
			Log.Info("Checking Complete checkbox");
			completeButton.Click();
		}

		#endregion

		#region [User details]
		private readonly Button closeUserDetailsButton ;

		private readonly Button refreshButton ;

		private Button logsButton ;

        private Button rollbackLogsButton ;

		private readonly Label syncedStateLabel ;

		private readonly Button completeButton ;

		private readonly Button enabledDetailsStopButton ;

		private readonly Button enabledDetailsSyncButton ;

		private readonly Button disabledCutoverButton ;

		private readonly Label dangerProgressBarLabel ;

        private readonly Button startedSortButton ;

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
	            new Label(By.XPath($"//*[contains(text(), '{localDate}')]"), "",driverId).WaitForElementPresent(10000);
            }
	        catch (Exception e)
	        {
	            SortStartedJobs();
	            new Label(By.XPath($"//*[contains(text(), '{localDate}')]"), "",driverId).WaitForElementPresent(10000);
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
			enabledDetailsStopButton.WaitForElementPresent();
		}

        public void AssertProgressAndState()
        {
            Log.Info("Checking progress");
            //string LastProgressColor = Browser.GetDriver().FindElements(By.XPath("//*/td/div[@class='progress']/div")).LastOrDefault().GetCssValue("background-color");
            string LastProgressColor = Driver.GetDriver(driverId).FindElements(By.XPath("//*/td/div[@class='progress']/div")).LastOrDefault().GetCssValue("background-color");
            //string LastJobState = Browser.GetDriver().FindElements(By.XPath("//*/div[@class='modal-content']//*/div[@class='table-responsive table-frame m-t-sm']//*/tbody/tr/td[2]/span")).LastOrDefault().Text;
            string LastJobState = Driver.GetDriver(driverId).FindElements(By.XPath("//*/div[@class='modal-content']//*/div[@class='table-responsive table-frame m-t-sm']//*/tbody/tr/td[2]/span")).LastOrDefault().Text;
            Assert.AreEqual(LastProgressColor == "rgba(37, 107, 147, 1)", LastJobState == "Synced");
        }

        public void AssertDetailsSyncButtonIsEnabled()
		{
			Log.Info("Asserting details sync button is enabled");
			enabledDetailsSyncButton.WaitForElementPresent();
		}

        public void AssertSyncingWasStoped()
        {
            Log.Info("Checking last syncing was stoped");
            //IList<IWebElement> Jobs = Browser.GetDriver().FindElements(By.XPath("//*/div[@class='modal-content']//*/div[@class='table-responsive table-frame m-t-sm']//*/tbody/tr/td[2]/span"));
            IList<IWebElement> Jobs = Driver.GetDriver(driverId).FindElements(By.XPath("//*/div[@class='modal-content']//*/div[@class='table-responsive table-frame m-t-sm']//*/tbody/tr/td[2]/span"));
            string LastJobState = Jobs[Jobs.Count - 1].Text;
            Assert.AreEqual("Stopped", LastJobState);
        }

        public void AssertDetailsSyncButtonIsDisabled()
		{
			Log.Info("Asserting details sync button is disabled");
			enabledDetailsSyncButton.WaitForElementDisappear();
		}

		public void SyncFromDetails()
		{
			Log.Info("Syncing");
			enabledDetailsSyncButton.Click();
		}

		public void AssertCutoverButton()
		{
			Log.Info("Asserting Cutover button");
			disabledCutoverButton.WaitForElementPresent();
		}

		public void WaitForJobIsCreated()
		{
			Log.Info("Waiting for job is created");
			var jobLabel = new Label(By.XPath("//tr[contains(*, 'Sync')]"), "Job label",driverId);
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
	        var jobLabel = new Label(By.XPath("//tr[contains(*, 'Sync')]"), "Job label",driverId);
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
		    catch (Exception e)
		    {
		       Log.Info("First click failed");
		       logsButton =
		            new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportSyncJobLogs')]"), "Logs button",driverId);
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
	        catch (Exception e)
	        {
	            Log.Info("First click failed");
	            rollbackLogsButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//a[contains(@href, 'ExportRollbackJobLogs')]"), "Rollback logs button",driverId);
                rollbackLogsButton.Click();
            }

	    }

        public void RefreshData()
		{
			Log.Info("Refreshing data");
			refreshButton.Click();
		}

		public void VerifyStateIS(string state)
		{
			Log.Info("Verifying state is: " + state);
			var stateLabel = new Label(By.XPath($"//*[contains(@data-bind, 'migrationState')][contains(text(), '{state}')]"),
				"State label",driverId);
			var counter = 0;
			while (!stateLabel.IsPresent() && counter < 10)
			{
				Thread.Sleep(30000);
				RefreshData();
				counter++;
			}
			stateLabel.WaitForElementPresent();
		}
        #endregion

	    #region [Rollback modal]

	    private readonly Label resetPermissionsLabel ;
        private readonly RadioButton resetPermissionsRadioButton ;
        private readonly Label dontResetPermissionsLabel ;
        private readonly RadioButton dontResetPermissionsRadioButton ;
        private readonly Label sureRollbackLabel ;
        private readonly RadioButton sureRollbackRadioButton ;
        private readonly Button rollbackButton ;

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

	    public void Rollback()
	    {
            Log.Info("Clicking Rollback");
            rollbackButton.Click();
	    }

	    #endregion
    }
}