using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.IntegrationProjectTests
{
    [TestFixture]
    public class VerifyTenantPageWillProvideDirSyncInformationForSourceTenant_TC31098 : TestBase
    {   
        [Test]
        [Category("Integration")]
        [Category("UI")]
        public void VerifyTenantPageWillProvideDirSyncInformationForSourceTenant_Integration_31098()
        {
            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project2");
            string clientName = client.Name;
            string username = client.Administrator.Username;
            string password = client.Administrator.Password;
            string projectName = project.Name;
            string sourceTenant = Automation.Settings.GetByReference<Tenant>(project.Source).Name;
            string targetTenant = Automation.Settings.GetByReference<Tenant>(project.Target).Name;

            var editTenantPage = Automation.Common
                                    .SingIn(username, password)
                                    .MigrateAndIntegrateSelect()
                                    .ClientSelect(clientName)                                            
                                    .ProjectSelect(projectName)
                                    .TenantsEdit()
                                    .GetPage<EditTenantsPage>();

            editTenantPage.ClickDiscoveryTab();

            Assert.IsTrue(editTenantPage.IsTenantDirSyncStatusSuccesfull(sourceTenant), "Source tenants DirSync status is not sucessesfull");
            Assert.IsTrue(editTenantPage.IsTenantDirSyncStatusSuccesfull(targetTenant), "Target tenants DirSync status is not sucessesfull");
        }
    
    }
}
