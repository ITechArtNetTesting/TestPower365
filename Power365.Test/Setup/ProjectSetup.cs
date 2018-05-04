using System;
using log4net;
using NUnit.Framework;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Workflows;
using BinaryTree.Power365.AutomationFramework;

namespace BinaryTree.Power365.Test.Setup
{
    [TestFixture]
    public class ProjectSetup: TestBase
    {
        public ProjectSetup() 
            : base(LogManager.GetLogger(typeof(ProjectSetup))) { }
        
        [Test]
        [Ignore("Incomplete")]
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
           
            var sourceCredential = sourceTenant.GetByReference<Credential>("psuser");
            var targetCredential = targetTenant.GetByReference<Credential>("psuser");

            var sourceTenantUser = sourceCredential.Username;
            var sourceTenantPassword = sourceCredential.Password;

            var targetTenantUser = targetCredential.Username;
            var targetTenantPassword = targetCredential.Password;
            
            var projectListPage = Automation.Common
                .SingIn(signInUser, signInPassword)
                .ClientSelect(clientName)
                .GetPage<ProjectListPage>();

            var editProjectPage = projectListPage.ClickNewProject();

            var editProjectWorkflow = Automation.Browser
                .CreateWorkflow<EmailWithFileProjectWorkflow, EditProjectPage>(editProjectPage);
            

            var projectDetailsPage = editProjectWorkflow
                .ProjectName(projectName)
                .ProjectDescription(projectDescription)
                .AddTenant(sourceTenantUser, sourceTenantPassword)
                .AddTenant(targetTenantUser, targetTenantPassword, true)
                //.UploadUserList("")
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
