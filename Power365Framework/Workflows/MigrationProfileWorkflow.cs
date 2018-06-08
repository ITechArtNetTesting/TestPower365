using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public class MigrationProfileWorkflow : WizardWorkflow
    {
        private static By _locator = By.Id("editMigrationProfileContainer");

        private By _profileName = By.Id("profileName");
        private By _whatShouldWeCallThisMigrationProfileStep = By.XPath("//*/span[@data-translation='WhatShouldWeCallThisMigrationProfile']");
        private By _createUsersInTargetStep = By.XPath("//*/span[@data-translation='DoYouWantToCreateUsersInTheTargetThatWeCannotFindAMatchFor']");
        private By _createDistributionGroupsInTargetStep = By.XPath("//*/span[@data-translation='DoYouWantToCreateNewDistributionGroupsInTheTargetTenant']");
        private By _syncDistributionGroupMethodStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToSyncDistributionGroups']");

        private By _willUsersNeedtoUpdateStep = By.XPath("//*/span[@data-translation='AnEmailNotificationWillBeSentToTheSourceUser']");
        private By _doYouWantToCopyLitigationHoldSettingsStep = By.XPath("//*/span[@data-translation='DoYouWantToCopyLitigationHoldSettingsAndData']");
        private By _wouldYouLikeToTranslateSourceEmailStep = By.XPath("//*/span[@data-translation='WouldYouLikeToTranslateSourceEmail']");
        private By _whatTypeOfMailboxContentWouldYouLikeToMigrateStep = By.XPath("//*/span[@data-translation='WhatTypeOfMailboxContentWouldYouLikeToMigrate']");
        private By _howWouldYouLikeToHandleLargeItemsStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleLargeItems']");
        private By _howWouldYouLikeToHandleBadItemsStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleBadItems']");
        private By _howWouldYouLikeToHandleFoldersThatCannotBeSyncedStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleFoldersThatCannotBeSynced']");

        private By _youAreAlmostDoneLetsReviewStep = By.XPath("//*/span[@data-translation='YouAreAlmostDoneLetsReview']");

        private By _doNotUpdateOutlookProfileRadio;
        private By _updateOutlookProfileRadio;
        private By _doNotWaitForProfileUpdateRadio;
        private By _waitForProfileUpdateRadio;

        private By _createUsersInTargetRadio;
        private By _doNotCreateUsersInTargetRadio;

        private By _createGroupsInTargetRadio;
        private By _doNotCreateGroupsInTargetRadio;

        private By _syncDistributionGroupsManually;
        private By _syncDistributionGroupsContinuously;

        private By _yesCopyLitigationHoldSettingsAndData;
        private By _noCopyLitigationHoldSettingsAndData;

        private By _yesTranslateEmailAddresses;
        private By _noTranslateEmailAddresses;

        private By _migrateEmailCheckbox;
        private By _migrateCalendarCheckbox;
        private By _migrateContactsCheckbox;
        private By _migrateTasksCheckbox;
        private By _migrateNotesCheckbox;

        private By _filterIncludeAllRadio;
        private By _filterByDateRangeRadio;
        private By _filterByAgeRadio;

        private By _filterDateRangeStart;
        private By _filterDateRangeEnd;

        private By _filterMessageAge;

        private By _largeItemsIncludeAll;
        private By _largeItemsFilterBySize;
        private By _largetItemsMaxSize;

        private By _badItemLimit;
        private By _badFolderLimit;

        private By _targetMailboxLicenseDefaultRadio;
        private By _targetMailboxLicensePreferredSkuRadio;

        private By _targetMailboxLicensePerferredSkuDropdown;

        protected readonly MigrationProfilesPage MigrationProfilesPage;

        public MigrationProfileWorkflow(MigrationProfilesPage migrationProfilesPage, By locator, IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(MigrationProfileWorkflow)), locator, webDriver)
        {
            MigrationProfilesPage = migrationProfilesPage;
        }

        public MigrationProfileWorkflow ProfileName(string name)
        {
            ValidateStepBy(_whatShouldWeCallThisMigrationProfileStep);
            InputElement profileNameField = new InputElement(_profileName, WebDriver);
            profileNameField.SendKeys(name);

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow UpdateOutlookProfiles(bool shouldUpdateProfiles = true, bool shouldWaitForUsers = true)
        {
            ValidateStepBy(_willUsersNeedtoUpdateStep);
            

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow CreateUsersInTarget(bool shouldCreateUsers = true)
        {
            ValidateStepBy(_createUsersInTargetStep);            

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow CreateGroupsInTarget(bool shouldCreateGroups = true)
        {
            ValidateStepBy(_createDistributionGroupsInTargetStep);          

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow SyncDistributionGroupsContinuously(bool shouldSyncContinuously = true)
        {
            ValidateStepBy(_syncDistributionGroupMethodStep);
         

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow CopyLitigationHoldSettingsAndData(bool shouldCopy = true)
        {
            ValidateStepBy(_doYouWantToCopyLitigationHoldSettingsStep);

           
            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow TranslateEmailAddress(bool shouldTranslate = true)
        {
            ValidateStepBy(_wouldYouLikeToTranslateSourceEmailStep);

           

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow MigrateMailboxContent(bool email=false, bool calendar=false, bool contacts=false, bool tasks=false, bool notes=false)
        {
            ValidateStepBy(_whatTypeOfMailboxContentWouldYouLikeToMigrateStep);

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow IncludeLargeItems(bool shouldInclude = true, int sizeFilter = 0)
        {
            ValidateStepBy(_howWouldYouLikeToHandleLargeItemsStep);
 

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow BadItemLimit(int limit)
        {
            ValidateStepBy(_howWouldYouLikeToHandleBadItemsStep);       

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow BadFolderLimit(int limit)
        {
            ValidateStepBy(_howWouldYouLikeToHandleFoldersThatCannotBeSyncedStep);

            

            ClickNext();

            return this;
        }

        public MigrationProfileWorkflow LicenseTargetMailboxes(MailboxLicenseType licenseType = MailboxLicenseType.Default)
        {
            ValidateStepBy(_willUsersNeedtoUpdateStep);

           
            ClickNext();

            return this;
        }

        public MigrationProfilesPage Submit()
        {
            ValidateStepBy(_youAreAlmostDoneLetsReviewStep);

            ClickNext();

            return MigrationProfilesPage;
        }
    }
}