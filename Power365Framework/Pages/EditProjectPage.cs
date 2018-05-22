using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Workflows;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Pages
{   
    public class EditProjectPage : PageBase
    {
        private static By _locator = By.Id("editProjectContainer");

        private readonly By _integrationRadio = By.Id("integrationRadio");
        private readonly By _emailWithDiscoveryRadio = By.Id("mailWithDiscoveryRadio");
        private readonly By _emailFromFileRadio = By.Id("mailOnlyRadio");
        private readonly By _emailFromFileOnPremRadio = By.Id("mailonlyOnPremRadio");
        
        private readonly By _projectNameStep = By.XPath("//button[contains(@data-translation, 'WhatShouldWeCallProject')]");
        private readonly By _projectNameText = By.Id("projectName");

        private readonly By _projectDescriptionText = By.Id("projectDescription");

        private readonly By _addTenantButton = By.XPath("//button[contains(@data-bind, 'addTenant')]");
        private readonly By _addTenantPairButton = By.XPath("//button[contains(@data-bind, 'addTenantMatch')]");
        private readonly By _addDomainPairButton = By.XPath("//button[contains(@data-bind, 'addDomainMatch')]");

        private readonly By _userListManualRadio = By.Id("manual");
        private readonly By _userListKeepExistingRadio = By.Id("keepExisting");

        private readonly By _firstTenantMatchGroup = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(@name, 'tenantMatchGroup')][1]");

        private readonly By _discoveryMethodUsersAllRadio = By.Id("migrateAllRadio");
        private readonly By _discoveryMethodUsersByGroupsRadio = By.Id("groupsRadio");

        private readonly By _discoveryGroupDropdownText = By.Id("groupSelector");

        private readonly By _removeAttributeMatchButton = By.XPath("//button[contains(@data-bind, 'removeAttributeMatch')]");
        private readonly By _addAttributeMatchButton = By.XPath("//button[contains(@data-bind, 'addAttributeMatch')]");

        private readonly By _createUsersRadio = By.Id("createUsers");
        private readonly By _dontCreateUsersRadio = By.Id("dontCreateUsers");

        private readonly By _discoveryMethodGroupsAllRadio = By.Id("migrateAllRadio");
        private readonly By _discoveryMethodGroupsUploadRadio = By.Id("uploadGroupsRadio");
        private readonly By _discoveryMethodGroupsNoSync = By.Id("noSyncRadio");

        private readonly By _createGroupsRadio = By.Id("createGroups");
        private readonly By _dontCreateGroupsRadio = By.Id("dontCreateGroups");

        private readonly By _defineMigrationWaveYesRadio = By.Id("wavesYes");
        private readonly By _defineMigrationWaveNoRadio = By.Id("wavesNo");

        private readonly By _migrationWaveNameText = By.Id("waveName");
        private readonly By _migrationWaveGroupSelectorDropdownText = By.Id("groupSelector");

        private readonly By _addAnotherMigrationWaveButton = By.XPath("//button[contains(@data-bind, 'addWave')]");

        private readonly By _syncScheduleYesRadio = By.Id("wavesYes1");
        private readonly By _syncScheduleNoRadio = By.Id("wavesNo1");

        private readonly By _syncScheduleStartOnDateText = By.Id("startDate");
        private readonly By _syncScheduleStartOnTimeText = By.Id("startTime");

        private readonly By _syncIntervalText = By.XPath("//input[contains(@data-bind, 'interval')]");
        private readonly By _syncIntervalByHoursButton = By.XPath("//button[contains(@data-bind, 'migrationWaveSyncInterval.hour')]");
        private readonly By _syncIntervalByDaysButton = By.XPath("//button[contains(@data-bind, 'migrationWaveSyncInterval.day')]");
        private readonly By _syncIntervalByWeeksButton = By.XPath("//button[contains(@data-bind, 'migrationWaveSyncInterval.week')]");

        private readonly By _syncIntervalMaxSyncCountText = By.Id("maxSyncCount");
        private readonly By _syncScheduleAddButton = By.XPath("//button[contains(@data-bind, 'addSchedule')]");

        private readonly By _addressBookDontSyncRadio = By.Id("abDontSync");
        private readonly By _addressBookFromSourceToTargetRadio = By.Id("abSyncSrcTgt");
        private readonly By _addressBookFromTargetToSourceRadio = By.Id("abSyncTgtSrc");
        private readonly By _addressBookBothDirectionsRadio = By.Id("abSyncBiDi");
        private readonly By _addressBookGroupDropdownText = By.Id("groupSelector");

        private readonly By _freeBusyYesRadio = By.Id("fbYes");
        private readonly By _freeBusyNoRadio = By.Id("fbNo");
        private readonly By _freeBusyAllUsersRadio = By.Id("migrateAllGroupsRadio");
        private readonly By _freeBusyImportGroupsRadio = By.Id("importGroupsRadio");
        private readonly By _freeBusyGroupDropdownText = By.Id("groupSelector");

        private readonly By _publicFoldersYesRadio = By.Id("yesFolders");
        private readonly By _publicFoldersNoRadio = By.Id("noFolders");
        private readonly By _publicFoldersCsvFileRadio = By.Id("csvFile");
        private readonly By _publicFoldersManualRadio = By.Id("manual");
        private readonly By _publicFoldersSelectFileButton = By.XPath("//button[contains(@data-bind, 'filesSelected')]");
        
        public EditProjectPage(IWebDriver webDriver) 
            : base(_locator, webDriver) { }

        public void SelectProjectType(ProjectType projectType)
        {
            By projectRadio = null;
            switch (projectType)
            {
                case ProjectType.Integration:
                    projectRadio = _integrationRadio;
                    break;
                case ProjectType.EmailWithDiscovery:
                    projectRadio = _emailWithDiscoveryRadio;
                    break;
                case ProjectType.EmailByFile:
                    projectRadio = _emailFromFileRadio;
                    break;
                case ProjectType.OnPremEmailByFile:
                    projectRadio = _emailFromFileOnPremRadio;
                    break;
                default:
                    throw new Exception("Invalid ProjectType");
            }

            var element = FindExistingElement(projectRadio);
            element.Click();
        }

        public void SelectMigrationWave(string migrationWave)
        {
            var migrationWaveNameElement = FindVisibleElement(_migrationWaveNameText);
            migrationWaveNameElement.SendKeys(migrationWave);
        }

        public void SetMigrationWaveName(string migrationWave)
        {
            var migrationWaveNameElement = FindVisibleElement(_migrationWaveNameText);
            migrationWaveNameElement.SendKeys(migrationWave);

        }
        public void SetProjectName(string projectName)
        {
            var projectNameElement = FindVisibleElement(_projectNameText);
            projectNameElement.SendKeys(projectName);
        }

        public void SetProjectDescription(string projectDescription)
        {
            var projectDescriptionElement = FindVisibleElement(_projectDescriptionText);
            projectDescriptionElement.SendKeys(projectDescription);
        }

        public void SetSyncSchedule(bool isEnabled)
        {
            ClickElementBy(isEnabled ? _syncScheduleYesRadio : _syncScheduleNoRadio);
        }

        public void AddTenant(string tenantUsername, string tenantPassword)
        {
            using (var popup = ClickPopupElementBy<O365SignInPage>(_addTenantButton))
            {
                var tenantAddPage = popup.Page;
                tenantAddPage.AuthorizeTenant(tenantUsername, tenantPassword);
            }
        }

        public void AddMigrationWave(string migrationWave)
        {
            var waveName = FindVisibleElement(_migrationWaveNameText);
            waveName.SendKeys(migrationWave);
        }

        public void SelectTenantMatchGroup()
        {
            var firstMachGroup = FindVisibleElement(_firstTenantMatchGroup);
            firstMachGroup.Click();
        }

        public void AddADGroupName(String groupName)
        {
            var discoveryGroupDropdownText = FindVisibleElement(_discoveryGroupDropdownText);
            discoveryGroupDropdownText.SendKeys(groupName);

        }

    }
}