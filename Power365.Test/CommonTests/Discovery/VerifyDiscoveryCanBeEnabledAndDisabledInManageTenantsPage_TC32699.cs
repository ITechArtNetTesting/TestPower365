using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryTree.Power365.Test.CommonTests.Discovery
{
    [TestClass]
    public class VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage_TC32699:TestBase
    {
        public VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage_TC32699() : base(LogManager.GetLogger(typeof(VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage_TC32699))) { }

        private string _client;
        private string _username;
        private string _projectName;
        private string _password;
        private string _tenant;

        private EditTenantsPage tenantsEditPage;

        private void SetTestCaseParams(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _projectName = _project.Name;
            _tenant = Automation.Settings.GetByReference<Tenant>(_project.Source).Name;
        }

        private void VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage(string client, string project)
        {
            SetTestCaseParams(client, project);
            tenantsEditPage = Automation.Common
                .SingIn(_username, _password)
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();
            tenantsEditPage.ClickDiscoveryTab();
            Assert.IsTrue(discoveryCanBeDisabledAndEnabled());
        }

        private bool discoveryCanBeDisabledAndEnabled()
        {            
            tenantsEditPage.DisableTenant(_tenant);
            bool EnabledResult= tenantsEditPage.CheckTenantCanBeDisabled(_tenant);
            tenantsEditPage.EnableTenant(_tenant);
            bool DisabledResult = tenantsEditPage.CheckTenantCanBeEnabled(_tenant);
            return DisabledResult&&EnabledResult;
        }        

        //[TestMethod]
        //[TestCategory("MailWithDiscovery")]
        //public void VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage_MD_32699()
        //{
        //    VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage("client2", "project1");
        //}

        [TestMethod]
        [TestCategory("Integration")]
        public void VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage_Integration_32699()
        {
            VerifyDiscoveryCanBeEnabledAndDisabledInManageTenantsPage("client2", "project2");
        }
    }
}
