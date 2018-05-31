using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.IntegrationProjectTests
{
    [TestFixture]
    public class VerifyTenantPageWillProvideDirSyncInformationForSourceTenant_TC31098 : TestBase
    {
        public VerifyTenantPageWillProvideDirSyncInformationForSourceTenant_TC31098() : base() { }

        EditTenantsPage _editTenantsPage;

        private void runTest(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var project = client.GetByReference<Project>(projectName);
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = project.Name;
            string _sourceTenant = Automation.Settings.GetByReference<Tenant>(project.Source).Name;
            string _targetTenant = Automation.Settings.GetByReference<Tenant>(project.Target).Name;

            verifyTenantPageWillProvideDirSyncInformationForSourceTenant(_username, _password, _client, _projectName, _sourceTenant, _targetTenant);
        }

        private void verifyTenantPageWillProvideDirSyncInformationForSourceTenant(string username, string password, string client, string projectName, string sourceTenant, string targetTenant)
        {
            _editTenantsPage = Automation.Common
                .SingIn(username, password)
                .ClientSelect(client)
                .SelectMigrateAndIntegrate()                             
                .ProjectSelect(projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();
            _editTenantsPage.ClickDiscoveryTab();
            Assert.IsTrue(_editTenantsPage.TenantHasOkStateAtDiscoveryTab(sourceTenant), "Source tenants status is not ok");
            Assert.IsTrue(_editTenantsPage.TenantHasOkStateAtDiscoveryTab(targetTenant), "Target tenants status is not ok");
        }

        [Test]
        [Category("Integration")]
        public void VerifyTenantPageWillProvideDirSyncInformationForSourceTenant_Integration_31098()
        {
            runTest("client2", "project2");
        }
    }
}
