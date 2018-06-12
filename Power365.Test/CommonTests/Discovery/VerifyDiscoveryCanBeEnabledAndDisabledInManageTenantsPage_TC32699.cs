using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.Discovery
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    class VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage_TC32699:TestBase
    {
        public VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage_TC32699() : base() { }
     
        private void VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage(string _client, string _username, string _projectName, string _password, string _tenant)
        {

            var tenantsEditPage = Automation.Common
                .SingIn(_username, _password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();
            tenantsEditPage.ClickDiscoveryTab();
     
            Assert.IsTrue(tenantsEditPage.CheckTenantCanBeEnabledOrDisabled(_tenant, false), "Discovery tenant can not be deasbled");
            Assert.IsTrue(tenantsEditPage.CheckTenantCanBeEnabledOrDisabled(_tenant, true),"Discovery tenant can not be enabled");
      
        }

        public void TestRun(string clientName, string project)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(project);
            string _client = client.Name;
            string  _username = client.Administrator.Username;
            string  _password = client.Administrator.Password;
            string _projectName = _project.Name;
            string _tenant = Automation.Settings.GetByReference<Tenant>(_project.Source).Name;

            VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage(_client,_username,_projectName,_password, _tenant);
        }


        [Test]
        [Category("SmokeTest")]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        [Category("Discovery")]
        public void DiscoveryCanBeEnabledAndDisabledInManageTenantsPage_MD_32699()
        {            
            TestRun("client2", "project1");
        }

        [Test]
        [Category("SmokeTest")]
        [Category("UI")]
        [Category("Integration")]
        [Category("Discovery")]
        public void DiscoveryCanBeEnabledAndDisabledInManageTenantsPage_Integration_32699()
        {
            TestRun("client2", "project2");
        }
    }
}
