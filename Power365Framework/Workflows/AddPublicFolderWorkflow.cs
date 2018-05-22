using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public class AddPublicFolderWorkflow : WizardWorkflow
    {
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
        //private readonly By _ = By.XPath("//span[@data-translation='']");
        //private readonly By _ = By.XPath("//span[@data-translation='']");
        
        protected readonly ManagePublicFoldersPage ManagePublicFoldersPage;

        public AddPublicFolderWorkflow(ManagePublicFoldersPage managePublicFoldersPage, IWebDriver webDriver)
            : base(managePublicFoldersPage, webDriver)
        {
            ManagePublicFoldersPage = managePublicFoldersPage;
        }
     
        public AddPublicFolderWorkflow UploadList(string path)
        {
            ValidateStepBy(_publicFolderListStep);


            throw new NotImplementedException();

            ValidateStepBy(_uploadListStep);
        }
           
        public AddPublicFolderWorkflow ChooseFolders()
        {
            ValidateStepBy(_publicFolderListStep);
            throw new NotImplementedException();
        }

        public AddPublicFolderWorkflow TenantPair(string sourceTenant, string targetTenant)
        {
            ValidateStepBy(_tenantPairStep);
            throw new NotImplementedException();
        }

        public AddPublicFolderWorkflow PathMapping(string sourcePath, string targetPath)
        {
            ValidateStepBy(_publicFolderPathStep);
            throw new NotImplementedException();
        }

        public AddPublicFolderWorkflow SyncScope()
        {
            ValidateStepBy(_syncDepthStep);
            throw new NotImplementedException();
        }

        public AddPublicFolderWorkflow Schedule(SyncSchedule schedule)
        {
            ValidateStepBy(_syncScheduleStep);
            throw new NotImplementedException();
        }

        public AddPublicFolderWorkflow OnDemand()
        {
            ValidateStepBy(_syncScheduleStep);
            throw new NotImplementedException();
        }

        public AddPublicFolderWorkflow Conflicts(bool isLastUpdated = false)
        {
            ValidateStepBy(_conflictResolutionStep);
            
            throw new NotImplementedException();
        }

        public AddPublicFolderWorkflow AddAnother()
        {
            ValidateStepBy(_confirmationStep);

            throw new NotImplementedException();
        }

        public AddPublicFolderWorkflow Submit()
        {
            ValidateStepBy(_confirmationStep);
            
            ClickNext();

            return this;
        }
       
    }
}
