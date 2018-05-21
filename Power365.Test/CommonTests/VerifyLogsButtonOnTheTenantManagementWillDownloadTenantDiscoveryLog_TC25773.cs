using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryTree.Power365.Test.CommonTests
{
    [TestClass]
    class VerifyLogsButtonOnTheTenantManagementWillDownloadTenantDiscoveryLog_TC25773: TestBase
    {
        public VerifyLogsButtonOnTheTenantManagementWillDownloadTenantDiscoveryLog_TC25773() : base(LogManager.GetLogger(typeof(VerifyLogsButtonOnTheTenantManagementWillDownloadTenantDiscoveryLog_TC25773))) { }

        private string _client;
        private string _username;
        private string _projectName;
        private string _password;

        private EditTenantsPage tenantsEditPage;

        private void SetTestCaseParams(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _projectName = _project.Name;            
        }

        private void VerifyLogsButtonOnTheTenantManagementWillDownloadTenantDiscoveryLog(string client, string project)
        {
            SetTestCaseParams(client, project);
            tenantsEditPage= Automation.Common
                .SingIn(_username, _password)
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();
            tenantsEditPage.ClickDiscoveryTab();
            tenantsEditPage.DownloadLogs();
            Assert.IsTrue(tenantsEditPage.CheckDiscoveryFileIsDownloaded());
        }
    }
}
