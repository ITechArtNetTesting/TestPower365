using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public class EditProjectWorkflowBase<T> : WizardWorkflow
    {
        //private static By _locator = By.XPath("//*/span[@data-translation='ChooseYourProjectType']");
        private static By _locator = By.Id("editProjectContainer");
       
        private readonly By _projectTypeStep = By.XPath("//*/span[@data-translation='ChooseYourProjectType']");
        private readonly By _projectNameStep = By.XPath("//*/span[@data-translation='WhatShouldWeCallProject']");

        private readonly By _projectDescriptionStep = By.XPath("//*/span[@data-translation='DescribeThisProjectInJustAFewWords']");

        private readonly By _addTenantsStep = By.XPath("//*/span[@data-translation='WeHaveToAddYourTenants']");
        private readonly By _reviewTenantsStep = By.XPath("//*/span[@data-translation='LetsReviewYourTenantPairs']");

        private readonly By _selectTenantMachStep = By.XPath("//*/span[@data-translation='DomainsYouHaveSelectedFrom']");
        private readonly By _discoverUsersStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToDiscoverUsersFrom']");
        private readonly By _discoverADGroupsStep = By.XPath("//*/span[@data-translation='WhichADGroupsShouldWeUseToDiscoverUsers']");
        private readonly By _reviewGroupsStep = By.XPath("//*/span[@data-translation='LetsReviewSelectedADGroups']");
        private readonly By _matchSourceUsersStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToMatchSourceUsers']");
        private readonly By _createUsersStep = By.XPath("//*/span[@data-translation='DoYouWantToCreateUsersInTheTarget']");
        private readonly By _discoveDistribuitionGroupsStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToDiscoverDistribuitionGroupsFrom']");
        private readonly By _matchDistribuitionGroupsStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToMatchSourceDistributionGroups']");
        private readonly By _createGroupsInTargetSteps = By.XPath("//*/span[@data-translation='DoYouWnatToCreateDistributionGroupsInTheTarget']");

        private readonly By _migrationWavesSteps = By.XPath("//*/span[@data-translation='MigrationWaves']");

        private readonly By _defineMigrationWavesSteps = By.XPath("//*/span[@data-translation='WouldYouLikeToDefineYourMigrationWavesNow']");
        private readonly By _wavesYes = By.XPath("//label[contains(@for, 'wavesYes')]");     
        private readonly By _wavesNo = By.XPath("//label[contains(@for, 'wavesNo')]");

        private readonly By _callMigrationWavesSteps = By.XPath("//*/span[@data-translation='WhatWouldYouLikeToCallThisMigrationWaveFrom']");

        private readonly By _assignGroupToWaveSteps = By.XPath("//*/span[@data-translation='WhichActiveDirectoryGroupIn']");
        private readonly By _assignSyncSheduleSteps = By.XPath("//*/span[@data-translation='DoYouWantToAssignASyncScheduleToAnyOfYourMigrationWavesNow']");
        private readonly By _createAddressBookSteps = By.XPath("//*/span[@data-translation='WouldYouLikeToCreateUnifiedAddressBook']");
        private readonly By _shareCalendarSteps = By.XPath("//*/span[@data-translation='WouldYouLikeToShareCalendarAvailabilityBetween']");

        private readonly By _identifyUsersThatWillShareCalendarAvailabilitySteps = By.XPath("//*/span[@data-translation='HowShouldWeIdentifyUsersThatWillShareCalendarAvailability']");
        private readonly By _groupToUseCalendar = By.XPath("//*/span[@data-translation='WhichActiveDirectoryGroup']");

        private readonly By _migratePublicFoldersSteps = By.XPath("//*/span[@data-translation='DoYouWantToMigratePublicFolders']");
        private readonly By _foldersYes = By.XPath("//label[contains(@for, 'yesFolders')]");
        private readonly By _foldersNo = By.XPath("//label[contains(@for, 'NoThanks')]");

        private readonly By _publicFolderListStep = By.XPath("//span[@data-translation='DoYouAlreadyHaveListOfPublicFoldersThatYouWouldLikeToSync']");
        private readonly By _keepMyExistingPublicFolderRadio = By.XPath("//span[@data-translation='KeepMyExistingPublicFolderMigrations']");

        private readonly By _tenantsAndAzureADConnectStatusStep = By.XPath("//span[@data-translation='BelowAreYourCurrentTenantsAndTheirAzureADConnectStatus']");
        private readonly By _downloadDirSyncStep = By.XPath("//span[@data-translation='LetsDownloadPower365DirectorySyncLiteApplication']");

        private readonly By _configuringPower365Step = By.XPath("//span[@data-translation='YouWillNeedInformationBelowWhenConfiguringPower365']");

        private readonly By _passwordToSecurePower365Steps = By.XPath("//*/span[@data-translation='EnterPasswordToSecurePower365']");
        private readonly By _emailRewritingSteps = By.XPath("//*/span[@data-translation='EmailRewriting']");

        private readonly By _doYouWantToHaveEmailRewrittenSteps = By.XPath("//*/span[@data-translation='DoYouWantToHaveEmailRewritten']");

        private readonly By _exchangeOnlineEmailRewriteGroupSteps = By.XPath("//*/span[@data-translation='ExchangeOnlineEmailRewriteGroups']");
        private readonly By _emailSignaturesSteps = By.XPath("//*/span[@data-translation='EmailSignatures']");

        protected readonly EditProjectPage EditProjectPage;

        public EditProjectWorkflowBase(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(T)), _locator, webDriver)
        {
            EditProjectPage = editProjectPage;
        }

        public T YesMigratePublicFolders( bool isFinished = false)
        {
            ValidateStepBy(_migratePublicFoldersSteps);
            ClickElementBy(_foldersYes);

            if (isFinished)
                ClickNext();
            WaitForLoadComplete();
            return GetWorkflow();
        }
        

        public T TenantsAndAzureADConnectStatus()
        {
            ValidateStepBy(_tenantsAndAzureADConnectStatusStep);

            ClickNext();

            return GetWorkflow();
        }

        public T NoMigratePublicFolders(bool isFinished = false)
        {
            ValidateStepBy(_migratePublicFoldersSteps);
            ClickElementBy(_foldersNo);

            if (isFinished)
                ClickNext();
           
            return GetWorkflow();
        }

        public T KeepMyExistingPublicFolder()
        {
            ValidateStepBy(_publicFolderListStep);

            ClickElementBy(_keepMyExistingPublicFolderRadio);

            ClickNext();
            WaitForLoadComplete();
            return GetWorkflow();
        }


        public T SelectGroupToUseCalendar(bool isFinished = false)
        {
            ValidateStepBy(_groupToUseCalendar);


            if (isFinished)
                ClickNext();
          //  throw new NotImplementedException();
          return GetWorkflow();
        }

       
        public T IdentifyUsersThatWillShareCalendarAvailability(bool isFinished = false)
        {
            ValidateStepBy(_identifyUsersThatWillShareCalendarAvailabilitySteps);

            WaitForLoadComplete();
            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
           return GetWorkflow();
        }

        public T WantToHaveEmailRewritten(bool isFinished = false)
        {
            ValidateStepBy(_doYouWantToHaveEmailRewrittenSteps);


            if (isFinished)
                ClickNext();
            // throw new NotImplementedException();
            return GetWorkflow();
        }

        public T DownloadDirSync(bool isFinished = false)
        {
            ValidateStepBy(_downloadDirSyncStep);
            WaitForLoadComplete();

         
            if (isFinished)               
                ClickNext();
            //throw new NotImplementedException();
            return GetWorkflow();
        }

        public T ExchangeOnlineEmailRewriteGroupSteps(bool isFinished = false)
        {
            ValidateStepBy(_exchangeOnlineEmailRewriteGroupSteps);
           

            if (isFinished)
                ClickNext();
            WaitForLoadComplete();
            // throw new NotImplementedException();
            return GetWorkflow();
        }

        public T PasswordToSecurePower365(bool isFinished = false)
        {
            ValidateStepBy(_passwordToSecurePower365Steps);


            if (isFinished)
                ClickNext();
            //throw new NotImplementedException();
           return GetWorkflow();
        }
        public T EmailSignatures(bool isFinished = false)
        {
            ValidateStepBy(_emailSignaturesSteps);


            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
            return GetWorkflow();
        }


        public T EmailRewriting(bool isFinished = false)
        {
            ValidateStepBy(_emailRewritingSteps);


            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
           return GetWorkflow();
        }


        public T ConfiguringPower365(bool isFinished = false)
        {
            ValidateStepBy(_configuringPower365Step);


            if (isFinished)
                ClickNext();
            //throw new NotImplementedException();
            return GetWorkflow();
        }

       public T ShareCalendar(bool isFinished = false)
        {
            ValidateStepBy(_shareCalendarSteps);
            

            if (isFinished)
                ClickNext();
            WaitForLoadComplete();
            //throw new NotImplementedException();
            return GetWorkflow();
        }

        public T CreateAddressBook(bool isFinished = false)
        {
            ValidateStepBy(_createAddressBookSteps);
           
            if (isFinished)
                ClickNext();
            WaitForLoadComplete();
            //  throw new NotImplementedException();
            return GetWorkflow();
        }
       
        public T AssignSyncSheduleOfWave(bool isFinished = false)
        {
            ValidateStepBy(_assignSyncSheduleSteps);
         

            if (isFinished)
                ClickNext();

            WaitForLoadComplete();
            //throw new NotImplementedException();
            return GetWorkflow();
        }

        public T AssignGroupToWave(bool isFinished = false)
        {
            ValidateStepBy(_assignGroupToWaveSteps);


            if (isFinished)
                ClickNext();
            WaitForLoadComplete();
            // throw new NotImplementedException();
            return GetWorkflow();
        }

        public T MigrationWaves(bool isFinished = false)
        {
            ValidateStepBy(_migrationWavesSteps);


            if (isFinished)
                ClickNext();
            WaitForLoadComplete();
            // throw new NotImplementedException();
            return GetWorkflow();
        }
        public T DefineMigrationWaves(bool yesLetsDoIt = false, bool isFinished = false )
        {
            ValidateStepBy(_defineMigrationWavesSteps);

            ClickElementBy(yesLetsDoIt ? _wavesYes : _wavesNo);
            
            if (isFinished)
                ClickNext();
            WaitForLoadComplete();
            //throw new NotImplementedException();
            return GetWorkflow();
        }

        public T DiscoverUsers(bool isFinished = false)
        {
            ValidateStepBy(_discoverUsersStep);


            if (isFinished)
                ClickNext();
          //  throw new NotImplementedException();
           return GetWorkflow();
        }

        public T DiscoverADGroups(bool isFinished = false)
        {
            ValidateStepBy(_discoverADGroupsStep);


            if (isFinished)
                ClickNext();
            //throw new NotImplementedException();
            return GetWorkflow();
        }
        public T ReviewGroups(bool isFinished = false)
        {
            ValidateStepBy(_reviewGroupsStep);


            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
            return GetWorkflow();
        }
        public T MatchSourceUsers(bool isFinished = false)
        {
            ValidateStepBy(_matchSourceUsersStep);


            if (isFinished)
                ClickNext();
            //throw new NotImplementedException();
           return GetWorkflow();
        }
        public T CreateUsers(bool isFinished = false)
        {
            ValidateStepBy(_createUsersStep);


            if (isFinished)
                ClickNext();
            //throw new NotImplementedException();
            return GetWorkflow();
        }

        public T DiscoveDistribuitionGroups(bool isFinished = false)
        {
            ValidateStepBy(_discoveDistribuitionGroupsStep);


            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
            return GetWorkflow();
        }
        public T MatchDistribuitionGroups(bool isFinished = false)
        {
            ValidateStepBy(_matchDistribuitionGroupsStep);


            if (isFinished)
                ClickNext();
            //throw new NotImplementedException();
           return GetWorkflow();
        }
        public T CreateGroupsInTarget(bool isFinished = false)
        {
            ValidateStepBy(_createGroupsInTargetSteps);


            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
            return GetWorkflow();
        }
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
               

        public EditProjectWorkflowBase(By locator, EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(T)), locator, webDriver)
        {
            EditProjectPage = editProjectPage;
        }

        public T ProjectType(ProjectType projectType)
        {
            //Probably don't need to validate steps becasue GetWorkflow will validate on init.
            ValidateStepBy(_projectTypeStep);

            EditProjectPage.SelectProjectType(projectType);

            ClickNext();
            //throw new NotImplementedException();
             return GetWorkflow();
        }

        public T ProjectName(string projectName=null)
        {
            ValidateStepBy(_projectNameStep);

           if (projectName!=null) EditProjectPage.SetProjectName(projectName);

            ClickNext();
            //  throw new NotImplementedException();
           return GetWorkflow();
       
        }
        
        public T ProjectDescription(string projectDescription)
        {
            ValidateStepBy(_projectDescriptionStep);

            //don't change the project description
            if (projectDescription!=null) EditProjectPage.SetProjectDescription(projectDescription);

            ClickNext();
            // throw new NotImplementedException();
            return GetWorkflow();
        }

        public T AddTenant(string tenantUsername, string tenantPassword, bool isFinished = false)
        {
            ValidateStepBy(_addTenantsStep);

            EditProjectPage.AddTenant(tenantUsername, tenantPassword);

            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
           return GetWorkflow();
        }

        public T AddTenant( bool isFinished = false)  
        {
            ValidateStepBy(_addTenantsStep);          

            if (isFinished)
                ClickNext();
          //  throw new NotImplementedException();
         return GetWorkflow();
        } 

        public T ReviewTenant()
        {
            ValidateStepBy(_reviewTenantsStep);

            ClickNext();
           // throw new NotImplementedException();
          return GetWorkflow();
        }
        public T SyncSchedule(bool isEnabled)
        {
            EditProjectPage.SetSyncSchedule(isEnabled);

            ClickNext();
           // throw new NotImplementedException();
           return GetWorkflow();
        }

        public T SetMigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.SetMigrationWaveName(migrationWave);

            if (isFinished)
                ClickNext();
            //throw new NotImplementedException();
          return GetWorkflow();
        }

        public T AddMigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.AddMigrationWave(migrationWave);

            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
           return GetWorkflow();
        }
        
        public T SelectMigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.SelectMigrationWave(migrationWave);

            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
           return GetWorkflow();
        }

        public T SelectDomain(bool isFinished = false)
        {
            ValidateStepBy(_selectTenantMachStep);
            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
             return GetWorkflow();
        }
               
        public T SelectTenantMachGroup(bool isFinished = false)
        {
            ValidateStepBy(_selectTenantMachStep);
            EditProjectPage.SelectTenantMatchGroup();

            if (isFinished)
                ClickNext();
          //  throw new NotImplementedException();
           return GetWorkflow();
        }

        public T AddADGroup(string groupName, bool isFinished = false)
        {
            EditProjectPage.AddADGroupName(groupName);

            if (isFinished)
                ClickNext();
           // throw new NotImplementedException();
            return GetWorkflow();
        }

        public T Submit()
        {
            ClickNext();
            //throw new NotImplementedException();
             return GetWorkflow();
        }
        
        //private T GetWorkflow(By locator)
        //{
        //    return (T)Activator.CreateInstance(typeof(T), locator, EditProjectPage, WebDriver);
        //}

        private T GetWorkflow()
        {
            return (T)Activator.CreateInstance(typeof(T), EditProjectPage, WebDriver);
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

        public EmailWithFileProjectWorkflow UploadUserList(string path)
        {
            return this;
        }
    }

    public class OnPremEmailFromFileWorkflow : EditProjectWorkflowBase<OnPremEmailFromFileWorkflow>
    {
        public OnPremEmailFromFileWorkflow(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(editProjectPage, webDriver) { }
    }
}
