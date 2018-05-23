using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public class EditProjectWorkflowBase<T> : WizardWorkflow
    {
        private readonly By _projectTypeStep = By.XPath("//*/span[@data-translation='ChooseYourProjectType']");
        private readonly By _projectNameStep = By.XPath("//*/span[@data-translation='WhatShouldWeCallProject']");
        private readonly By _projectDescriptionStep = By.XPath("//*/span[@data-translation='DescribeThisProjectInJustAFewWords']");
        private readonly By _addTenantsStep = By.XPath("//*/span[@data-translation='WeHaveToAddYourTenants']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");
        //private readonly By _ = By.XPath("//*/span[@data-translation='']");


        protected readonly EditProjectPage EditProjectPage;

        public EditProjectWorkflowBase(EditProjectPage editProjectPage, IWebDriver webDriver)
            : base(editProjectPage, webDriver)
        {
            EditProjectPage = editProjectPage;
        }
        
        public T ProjectType(ProjectType projectType)
        {
            ValidateStepBy(_projectTypeStep);

            EditProjectPage.SelectProjectType(projectType);

            ClickNext();

            return GetWorkflow();
        }

        public T ProjectName(string projectName)
        {
            ValidateStepBy(_projectNameStep);

            EditProjectPage.SetProjectName(projectName);

            ClickNext();

            return GetWorkflow();
        }
        
        public T ProjectDescription(string projectDescription)
        {
            ValidateStepBy(_projectDescriptionStep);

            EditProjectPage.SetProjectDescription(projectDescription);

            ClickNext();

            return GetWorkflow();
        }

        public T AddTenant(string tenantUsername, string tenantPassword, bool isFinished = false)
        {
            ValidateStepBy(_addTenantsStep);

            EditProjectPage.AddTenant(tenantUsername, tenantPassword);

            if (isFinished)
                ClickNext();
            
            return GetWorkflow();
        }

        public T SyncSchedule(bool isEnabled)
        {
            EditProjectPage.SetSyncSchedule(isEnabled);

            ClickNext();

            return GetWorkflow();
        }

       public T MigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.SetMigrationWaveName(migrationWave);

            if (isFinished)
                ClickNext();

            return GetWorkflow();
        }

        public T AddMigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.AddMigrationWave(migrationWave);

            if (isFinished)
                ClickNext();

            return GetWorkflow();
        }


        public T SelectMigrationWave(string migrationWave, bool isFinished = false)
        {
            EditProjectPage.SelectMigrationWave(migrationWave);

            if (isFinished)
                ClickNext();

            return GetWorkflow();
        }

        public T SelectTenantMachGroup(bool isFinished = false)
        {
            EditProjectPage.SelectTenantMatchGroup();

            if (isFinished)
                ClickNext();

            return GetWorkflow();
        }

        public T AddADGroup(string groupName, bool isFinished = false)
        {
            EditProjectPage.AddADGroupName(groupName);

            if (isFinished)
                ClickNext();

            return GetWorkflow();
        }

        public T Submit()
        {
            ClickNext();

            return GetWorkflow();
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
