using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public class EditProjectWorkflowBase<T> : WizardWorkflow
    {
        //private static By _locator = By.Id("editProjectContainer");

        protected readonly By ProjectNameStep = By.XPath("//button[contains(@data-translation, 'WhatShouldWeCallProject')]");
        protected readonly By ProjectNameText = By.Id("projectName");

        protected readonly By _projectDescriptionText = By.Id("projectDescription");

        protected readonly By _addTenantButton = By.XPath("//button[contains(@data-bind, 'addTenant')]");
        protected readonly By _addTenantPairButton = By.XPath("//button[contains(@data-bind, 'addTenantMatch')]");
        protected readonly By _addDomainPairButton = By.XPath("//button[contains(@data-bind, 'addDomainMatch')]");

        protected readonly By _userListManualRadio = By.Id("manual");
        protected readonly By _userListKeepExistingRadio = By.Id("keepExisting");

        protected readonly By _discoveryMethodUsersAllRadio = By.Id("migrateAllRadio");
        protected readonly By _discoveryMethodUsersByGroupsRadio = By.Id("groupsRadio");

        protected readonly By _discoveryGroupDropdownText = By.Id("groupSelector");

        protected readonly By _removeAttributeMatchButton = By.XPath("//button[contains(@data-bind, 'removeAttributeMatch')]");
        protected readonly By _addAttributeMatchButton = By.XPath("//button[contains(@data-bind, 'addAttributeMatch')]");

        protected readonly By _createUsersRadio = By.Id("createUsers");
        protected readonly By _dontCreateUsersRadio = By.Id("dontCreateUsers");

        protected readonly By _discoveryMethodGroupsAllRadio = By.Id("migrateAllRadio");
        protected readonly By _discoveryMethodGroupsUploadRadio = By.Id("uploadGroupsRadio");
        protected readonly By _discoveryMethodGroupsNoSync = By.Id("noSyncRadio");

        protected readonly By _createGroupsRadio = By.Id("createGroups");
        protected readonly By _dontCreateGroupsRadio = By.Id("dontCreateGroups");

        protected readonly By _defineMigrationWaveYesRadio = By.Id("wavesYes");
        protected readonly By _defineMigrationWaveNoRadio = By.Id("wavesNo");

        protected readonly By _migrationWaveNameText = By.Id("waveName");
        protected readonly By _migrationWaveGroupSelectorDropdownText = By.Id("groupSelector");

        protected readonly By _addAnotherMigrationWaveButton = By.XPath("//button[contains(@data-bind, 'addWave')]");

        protected readonly By _syncScheduleYesRadio = By.Id("wavesYes1");
        protected readonly By _syncScheduleNoRadio = By.Id("wavesNo1");

        protected readonly By _syncScheduleStartOnDateText = By.Id("startDate");
        protected readonly By _syncScheduleStartOnTimeText = By.Id("startTime");

        protected readonly By _syncIntervalText = By.XPath("//input[contains(@data-bind, 'interval')]");
        protected readonly By _syncIntervalByHoursButton = By.XPath("//button[contains(@data-bind, 'migrationWaveSyncInterval.hour')]");
        protected readonly By _syncIntervalByDaysButton = By.XPath("//button[contains(@data-bind, 'migrationWaveSyncInterval.day')]");
        protected readonly By _syncIntervalByWeeksButton = By.XPath("//button[contains(@data-bind, 'migrationWaveSyncInterval.week')]");

        protected readonly By _syncIntervalMaxSyncCountText = By.Id("maxSyncCount");
        protected readonly By _syncScheduleAddButton = By.XPath("//button[contains(@data-bind, 'addSchedule')]");

        protected readonly By _addressBookDontSyncRadio = By.Id("abDontSync");
        protected readonly By _addressBookFromSourceToTargetRadio = By.Id("abSyncSrcTgt");
        protected readonly By _addressBookFromTargetToSourceRadio = By.Id("abSyncTgtSrc");
        protected readonly By _addressBookBothDirectionsRadio = By.Id("abSyncBiDi");
        protected readonly By _addressBookGroupDropdownText = By.Id("groupSelector");

        protected readonly By _freeBusyYesRadio = By.Id("fbYes");
        protected readonly By _freeBusyNoRadio = By.Id("fbNo");
        protected readonly By _freeBusyAllUsersRadio = By.Id("migrateAllGroupsRadio");
        protected readonly By _freeBusyImportGroupsRadio = By.Id("importGroupsRadio");
        protected readonly By _freeBusyGroupDropdownText = By.Id("groupSelector");

        protected readonly By _publicFoldersYesRadio = By.Id("yesFolders");
        protected readonly By _publicFoldersNoRadio = By.Id("noFolders");
        protected readonly By _publicFoldersCsvFileRadio = By.Id("csvFile");
        protected readonly By _publicFoldersManualRadio = By.Id("manual");
        protected readonly By _publicFoldersSelectFileButton = By.XPath("//button[contains(@data-bind, 'filesSelected')]");

        public EditProjectWorkflowBase(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(editProjectPage, webDriver) { }
        
        public T ProjectName(string projectName)
        {
            var projectNameElement = FindVisibleElement(ProjectNameText);
            projectNameElement.SendKeys(projectName);

            ClickNext();

            return GetWorkflow();
        }

        public T ProjectDescription(string projectDescription)
        {
            var projectDescriptionElement = FindVisibleElement(_projectDescriptionText);
            projectDescriptionElement.SendKeys(projectDescription);

            ClickNext();

            return GetWorkflow();
        }

        public T AddTenant(string tenantUsername, string tenantPassword, bool isFinished = false)
        {
            using (var popup = ClickAddTenant())
            {
                var tenantAddPage = popup.Page;
                tenantAddPage.AuthorizeTenant(tenantUsername, tenantPassword);
            }

            if (isFinished)
                ClickNext();
            
            return GetWorkflow();
        }

        public T SyncSchedule(bool isEnabled)
        {
            ClickElementBy(isEnabled ? _syncScheduleYesRadio : _syncScheduleNoRadio);
            ClickNext();

            return GetWorkflow();
        }

        public T AddMigrationWave(string migrationWave, bool isFinished = false)
        {
            throw new NotImplementedException();
        }

        public T SelectMigrationWave(string migrationWave, bool isFinished = false)
        {
            throw new NotImplementedException();
        }

        public T Submit()
        {
            ClickNext();

            return GetWorkflow();
        }

        protected DisposablePopupPage<O365SignInPage> ClickAddTenant()
        {
            return ClickPopupElementBy<O365SignInPage>(_addTenantButton);
        }

        private T GetWorkflow()
        {
            return (T)Activator.CreateInstance(typeof(T), (EditProjectPage)CurrentPage, WebDriver);
        }
    }

    public class IntegrationProjectWorkflow : EditProjectWorkflowBase<IntegrationProjectWorkflow>
    {
        public IntegrationProjectWorkflow(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(editProjectPage, webDriver) { }
    }

    public class EmailWithDiscoveryProjectWorkflow : EditProjectWorkflowBase<EmailWithFileProjectWorkflow>
    {
        public EmailWithDiscoveryProjectWorkflow(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(editProjectPage, webDriver) { }
    }

    public class EmailWithFileProjectWorkflow : EditProjectWorkflowBase<EmailWithFileProjectWorkflow>
    {
        public EmailWithFileProjectWorkflow(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(editProjectPage, webDriver) { }
    }

    public class OnPremEmailFromFileWorkflow : EditProjectWorkflowBase<OnPremEmailFromFileWorkflow>
    {
        public OnPremEmailFromFileWorkflow(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(editProjectPage, webDriver) { }
    }
}
