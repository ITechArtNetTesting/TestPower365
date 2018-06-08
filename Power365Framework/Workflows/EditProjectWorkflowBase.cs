using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public class EditProjectWorkflowBase<T> : WizardWorkflow
    {
        private static By _locator = By.XPath("//*/span[@data-translation='ChooseYourProjectType']");

        private readonly By _projectTypeStep = By.XPath("//*/span[@data-translation='ChooseYourProjectType']");
        private readonly By _projectNameStep = By.XPath("//*/span[@data-translation='WhatShouldWeCallProject']");
        private readonly By _projectDescriptionStep = By.XPath("//*/span[@data-translation='DescribeThisProjectInJustAFewWords']");
        private readonly By _addTenantsStep = By.XPath("//*/span[@data-translation='WeHaveToAddYourTenants']");
        private readonly By _reviewTenantsStep = By.XPath("//*/span[@data-translation='LetsReviewYourTenantPairs']");
        private readonly By _selectDomainStep = By.XPath("//*/span[@data-translation='DomainsYouHaveSelectedFrom']");
        private readonly By _discoverUsersStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToDiscoverUsersFrom']");
        private readonly By _discoverGroupsStep = By.XPath("//*/span[@data-translation='WhichADGroupsShouldWeUseToDiscoverUsers']");
        private readonly By _reviewGroupsStep = By.XPath("//*/span[@data-translation='LetsReviewSelectedADGroups']");
        private readonly By _matchUsersStep = By.XPath("//*/span[@data-translation='HowWouldYouLikeToMatchSourceUsers']");


        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");

        protected readonly EditProjectPage EditProjectPage;

        public EditProjectWorkflowBase(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(T)), _locator, webDriver)
        {
            EditProjectPage = editProjectPage;
        }

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
            
            return GetWorkflow(_projectNameStep);
        }

        public T ProjectName(string projectName)
        {
            ValidateStepBy(_projectNameStep);

            EditProjectPage.SetProjectName(projectName);

            ClickNext();

            return GetWorkflow(_projectDescriptionStep);
        }
        
        public T ProjectDescription(string projectDescription)
        {
            ValidateStepBy(_projectDescriptionStep);

            EditProjectPage.SetProjectDescription(projectDescription);

            ClickNext();

            return GetWorkflow(_addTenantsStep);
        }

        public T AddTenant(string tenantUsername, string tenantPassword, bool isFinished = false)
        {
            ValidateStepBy(_addTenantsStep);

            EditProjectPage.AddTenant(tenantUsername, tenantPassword);

            if (isFinished)
                ClickNext();
            throw new NotImplementedException();
            //return GetWorkflow();
        }

        public T AddTenant( bool isFinished = false)  
        {
            ValidateStepBy(_addTenantsStep);          

            if (isFinished)
                ClickNext();
            throw new NotImplementedException();
            //return GetWorkflow();
        } 

        public T ReviewTenant()
        {
            ValidateStepBy(_reviewTenantsStep);

            ClickNext();
            throw new NotImplementedException();
            //return GetWorkflow();
        }
        public T SyncSchedule(bool isEnabled)
        {
            EditProjectPage.SetSyncSchedule(isEnabled);

            ClickNext();
            throw new NotImplementedException();
            //return GetWorkflow();
        }

        public T MigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.SetMigrationWaveName(migrationWave);

            if (isFinished)
                ClickNext();
            throw new NotImplementedException();
            // return GetWorkflow();
        }

        public T AddMigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.AddMigrationWave(migrationWave);

            if (isFinished)
                ClickNext();
            throw new NotImplementedException();
            //return GetWorkflow();
        }
        
        public T SelectMigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.SelectMigrationWave(migrationWave);

            if (isFinished)
                ClickNext();
            throw new NotImplementedException();
            // return GetWorkflow();
        }

        public T SelectDomain(bool isFinished = false)
        {
            if (isFinished)
                ClickNext();
            throw new NotImplementedException();
            // return GetWorkflow();
        }

        //public T SelectDomain(bool isFinished = false)
        //{
        //    if (isFinished)
        //        ClickNext();
        //    throw new NotImplementedException();
        //    // return GetWorkflow();
        //}

        public T SelectTenantMachGroup(bool isFinished = false)
        {
            EditProjectPage.SelectTenantMatchGroup();

            if (isFinished)
                ClickNext();
            throw new NotImplementedException();
            //  return GetWorkflow();
        }

        public T AddADGroup(string groupName, bool isFinished = false)
        {
            EditProjectPage.AddADGroupName(groupName);

            if (isFinished)
                ClickNext();
            throw new NotImplementedException();
            // return GetWorkflow();
        }

        public T Submit()
        {
            ClickNext();
            throw new NotImplementedException();
            // return GetWorkflow();
        }
        
        private T GetWorkflow(By locator)
        {
            return (T)Activator.CreateInstance(typeof(T), locator, EditProjectPage, WebDriver);
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
