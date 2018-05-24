using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public class AddPublicFolderWorkflow : WizardWorkflow
    {
        private static By _locator = By.XPath("//span[@data-translation='DoYouAlreadyHaveListOfPublicFoldersThatYouWouldLikeToSync']");

        private readonly By _publicFolderListStep = By.XPath("//span[@data-translation='DoYouAlreadyHaveListOfPublicFoldersThatYouWouldLikeToSync']");
        private readonly By _uploadListStep = By.XPath("//*/span[@data-translation='UploadYourPublicFolderMigrationList']");
        private readonly By _tenantPairStep = By.XPath("//span[@data-translation='WhichTenantPairWouldYouLikeToUseForThePublicFolderMigration']");
        private readonly By _publicFolderPathStep = By.XPath("//span[@data-translation='WhatIsThePathToThePublicFolderThatYouWantToMigrate']");

        private readonly By _syncDepthStep = By.XPath("//span[@data-translation='DoYouWantToSynchronizeOnlyTheTopLevelFolderOrAllSubFolders']");

        private readonly By _syncScheduleStep = By.XPath("//span[@data-translation='DoYouWantToRunThisMigrationOnAScheduleOrOnDemand']");
        private readonly By _syncStartStep = By.XPath("//span[@data-translation='WhenDoYouWantToStartThisMigration']");
        private readonly By _syncIntervalStep = By.XPath("//span[@data-translation='RunPublicFolderMigrationEvery']");
        private readonly By _syncCountStep = By.XPath("//span[@data-translation='WhatIsTheMaximumNumberOfTimesThatYouWantToRunThisMigration']");

        private readonly By _conflictResolutionStep = By.XPath("//span[@data-translation='HowDoYouWantToResolveConflicts']");
        private readonly By _confirmationStep = By.XPath("//span[@data-translation='OkSoYouWantToMigratePublicFoldersFrom']");
        
        private readonly By _csvFileRadio = By.XPath("//span[@data-translation='YesIWantToUploadMyList']");
        private readonly By _manualRadio = By.XPath("//span[@data-translation='NoLetsChooseThemNow']");

        private readonly By _selectFileButton = By.XPath("//input[@type='file']");
        private readonly By _uploadedFileLabel = By.XPath("//h4[contains(@data-bind, 'file().name')]");

        private readonly By _sourcePathText = By.Id("sourcePath");
        private readonly By _targetPathText = By.Id("targetPath");

        private readonly By _topLevelOnlyRadio = By.XPath("//span[@data-translation='TopLevelOnly']");
        private readonly By _allSubLevelsRadio = By.XPath("//span[@data-translation='AllSubfolders']");
        
        private readonly By _schedule = By.XPath("//span[@data-translation='Schedule']");
        private readonly By _onDemand = By.XPath("//span[@data-translation='OnDemand']");
        
        private readonly By _useLastUpdated = By.XPath("//span[@data-translation='UseLastUpdated']");
        private readonly By _sourceAlwaysOverwritesTarget = By.XPath("//span[@data-translation='SourceAlwaysOverwritesTarget']");
        
        private readonly string _tenantPairRadioFormat = "//label/*[contains(text(), '{0}')]";

        protected readonly ManagePublicFoldersPage ManagePublicFoldersPage;

        public AddPublicFolderWorkflow(ManagePublicFoldersPage managePublicFoldersPage, IWebDriver webDriver)
            : base(_locator, webDriver)
        {
            ManagePublicFoldersPage = managePublicFoldersPage;
        }
     
        public AddPublicFolderWorkflow UploadList(string path)
        {
            ValidateStepBy(_publicFolderListStep);

            ClickElementBy(_csvFileRadio);
            
            ValidateStepBy(_uploadListStep);

            var selectFileInputElement = FindExistingElement(_selectFileButton);
            selectFileInputElement.SendKeys(path);

            if (!IsElementVisible(_uploadedFileLabel, 15))
                throw new Exception(string.Format("File '{0}' failed to upload."));

            return this;
        }
           
        public AddPublicFolderWorkflow ChooseFolders()
        {
            ValidateStepBy(_publicFolderListStep);

            ClickElementBy(_manualRadio);

            ClickNext();

            return this;
        }

        public AddPublicFolderWorkflow TenantPair(string sourceTenant, string targetTenant)
        {
            ValidateStepBy(_tenantPairStep);

            var tenantPairLocator = By.XPath(string.Format(_tenantPairRadioFormat, sourceTenant));

            ClickElementBy(tenantPairLocator);

            ClickNext();

            return this;
        }

        public AddPublicFolderWorkflow PathMapping(string sourcePath, string targetPath)
        {
            ValidateStepBy(_publicFolderPathStep);

            var sourcePathTextElement = FindVisibleElement(_sourcePathText);
            sourcePathTextElement.SendKeys(sourcePath);

            ClickNext();

            var targetPathTextElement = FindVisibleElement(_targetPathText);
            targetPathTextElement.SendKeys(targetPath);

            ClickNext();

            return this;
        }

        public AddPublicFolderWorkflow SyncScope(bool topLevelOnly)
        {
            ValidateStepBy(_syncDepthStep);

            if (topLevelOnly)
                ClickElementBy(_topLevelOnlyRadio);
            else
                ClickElementBy(_allSubLevelsRadio);

            ClickNext();

            return this;
        }

        public AddPublicFolderWorkflow Schedule(SyncSchedule schedule)
        {
            ValidateStepBy(_syncScheduleStep);

            ClickElementBy(_schedule);

            ClickNext();

            return this;
        }

        public AddPublicFolderWorkflow OnDemand()
        {
            ValidateStepBy(_syncScheduleStep);

            ClickElementBy(_onDemand);

            ClickNext();

            return this;
        }

        public AddPublicFolderWorkflow Conflicts(bool lastUpdate)
        {
            ValidateStepBy(_conflictResolutionStep);

            if (lastUpdate)
                ClickElementBy(_useLastUpdated);
            else
                ClickElementBy(_sourceAlwaysOverwritesTarget);

            ClickNext();

            return this;
        }

        public AddPublicFolderWorkflow AddAnother()
        {
            ValidateStepBy(_confirmationStep);

            throw new NotImplementedException();
        }

        public ManagePublicFoldersPage Submit()
        {
            ValidateStepBy(_confirmationStep);
            
            ClickNext();

            return ManagePublicFoldersPage;
        }
       
    }
}
