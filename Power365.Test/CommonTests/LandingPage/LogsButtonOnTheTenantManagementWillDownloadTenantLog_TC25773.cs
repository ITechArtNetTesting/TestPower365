using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.LandingPage
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    class LogsButtonOnTheTenantManagementWillDownloadTenantLog_TC25773: TestBase
    {   
        private readonly static string EXPORT_FILE_NAME="Tenant*.csv";

        private void SetTestCaseParams(string clientName, string projectName)
        {
           var client = Automation.Settings.GetByReference<Client>(clientName);
           var _project = client.GetByReference<Project>(projectName);
           string  _client = client.Name;
           string _username = client.Administrator.Username;
           string _password = client.Administrator.Password;
           string _projectName = _project.Name;
           string _tenant = Automation.Settings.GetByReference<Tenant>(_project.Source).Name;
           string _downloadsPath = Automation.Settings.DownloadsPath;
           LogsButtonOnTheTenantManagementWillDownloadTenantLog(_client, _projectName, _username, _password, _downloadsPath, _tenant);
        }

        private void LogsButtonOnTheTenantManagementWillDownloadTenantLog(string _client, string _projectName,string _username, string _password, string _downloadsPath, string _tenant)
        {
           var  tenantsEditPage = Automation.Common
                .SingIn(_username, _password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();
            tenantsEditPage.ClickDiscoveryTab();
            tenantsEditPage.DownloadLogs(_tenant);

            var isFileDownloaded = Automation.Browser.IsFileDownloaded(EXPORT_FILE_NAME, WaitDefaults.FILE_DOWNLOAD_TIMEOUT_SEC, 3);

            Assert.IsTrue(isFileDownloaded, "Tenant log file was not found in the downloads directory");
        }

        [Test]
        [Category("MailWithDiscovery")]
        [Category("UI")]
        public void LogsButtonOnTheTenantManagementWillDownloadTenantLog_MD_25773()
        {
            SetTestCaseParams("client2", "project1");
        }

        [Test]
        [Category("Integration")]
        [Category("UI")]
        public void LogsButtonOnTheTenantManagementWillDownloadTenantLog_Integration_25773()
        {
            SetTestCaseParams("client2", "project2");
        }
    }
}
