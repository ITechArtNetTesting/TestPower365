using System;
using log4net;
using NUnit.Framework;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Workflows;
using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using AutomationServices.SqlDatabase;

namespace BinaryTree.Power365.Test.Setup
{
    [TestFixture]
    public class ProjectSetup: TestBase
    {
        public ProjectSetup() 
            : base() { }
        
        [Test]
        public void DBAccesstest()
        {
            var databaseService =Automation.GetService<DatabaseService>();
            databaseService.SetDirSyncLiteTenantId(null, 0);
        }

        [Test]
        public void EmailWithFileProjectSetup()
        {
            var settings = Automation.Settings;

            var client = settings.GetByReference<Client>("client1");
            var clientName = client.Name;

            var signInUser = client.Administrator.Username;
            var signInPassword = client.Administrator.Password;

            var project = client.GetByReference<Project>("project1");
            var projectName = project.Name;
            var projectDescription = project.Description;

            var sourceTenant = settings.GetByReference<Tenant>(project.Source);
            var targetTenant = settings.GetByReference<Tenant>(project.Target);
           
            var sourceCredential = sourceTenant.GetByReference<Credential>("ps1");
            var targetCredential = targetTenant.GetByReference<Credential>("ps1");

            var sourceTenantUser = sourceCredential.Username;
            var sourceTenantPassword = sourceCredential.Password;

            var targetTenantUser = targetCredential.Username;
            var targetTenantPassword = targetCredential.Password;

            var userList = project.GetByReference<File>("file1");

            var uploadFilePath = userList.Path;

            var projectListPage = Automation.Common
                .SingIn(signInUser, signInPassword)
                .MigrateAndIntegrateSelect()
                .ClientSelect(clientName)
                .GetPage<ProjectListPage>();

            var editProjectWorkflow = projectListPage.ClickNewProject<EmailWithFileProjectWorkflow>();

            var projectDetailsPage = editProjectWorkflow
                .ProjectType(ProjectType.EmailByFile)
                .ProjectName(projectName)
                .ProjectDescription(projectDescription)
                .AddTenant(sourceTenantUser, sourceTenantPassword)
                .AddTenant(targetTenantUser, targetTenantPassword, true)
                .UploadUserList(uploadFilePath)
                .SyncSchedule(false)
                .Submit()
                .GetPage<ProjectDetailsPage>();
        }

        [Test]
        public void MailWithDiscoveryProjectSetup()
        {

        }

        [Test]
        public void IntegrationProjectSetup()
        {

        }

        [Test]
        public void IntegrationProProjectSetup()
        {

        }

        [Test]
        public void OnPremProjectSetup()
        {

        }
    }
}
