using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Workflows;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.CommonTests.Help
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    class HelpVideoExist_TC : TestBase
    {


        [Test]
        [Category("UI")]
        [Category("Integration")]
        [TestResource("client2", "project2", "entry11")]
        public void MigrationWave_Sync_Integration_TC30919_33610()
        {
            var settings = Automation.Settings;

            var client = settings.GetByReference<Client>("client2");
            var clientName = client.Name;

            var signInUser = client.Administrator.Username;
            var signInPassword = client.Administrator.Password;

            var project = client.GetByReference<Project>("project2");
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


            // ManageUsersPage manageUsersPage;
            ProjectDetailsPage projectDetailPage = Automation.Common
                                        .SingIn(signInUser, signInPassword)
                                        .MigrateAndIntegrateSelect()
                                        .ClientSelect(clientName)
                                        .ProjectSelect(projectName)
                                        .GetPage<ProjectDetailsPage>();

            //var editProjectWorkflow = projectDetailPage.ClickProjectEdit<IntegrationProjectWorkflow>();
            //editProjectWorkflow
            //    .ProjectName(projectName)
            //    .ProjectDescription(projectDescription)
            //    .AddTenant()  
            //    .ReviewTenant()
            //    .SelectDomain()
            //    .
            //    .UploadUserList(uploadFilePath)
            //    .SyncSchedule(false)


            ////var projectListPage = Automation.ProjectWo
            //// .SingIn(signInUser, signInPassword)
            //// .ClientSelect(clientName)
            //// .GetPage<ProjectListPage>();

            //manageUsersPage = new IntegrationProjectWorkflow(projectListPage, webDriver);

            //var editProjectWorkflow = projectListPage.ClickNewProject<EmailWithFileProjectWorkflow>();
            //var projectDetailsPage = editProjectWorkflow
            //       .ProjectType(ProjectType.EmailByFile)
            //       .ProjectName(projectName)
            //       .ProjectDescription(projectDescription)
            //       .AddTenant(sourceTenantUser, sourceTenantPassword)
            //       .AddTenant(targetTenantUser, targetTenantPassword, true)
            //       .UploadUserList(uploadFilePath)
            //       .SyncSchedule(false)
            //       .Submit()
            //       .GetPage<ProjectDetailsPage>();

        }


    }
}
