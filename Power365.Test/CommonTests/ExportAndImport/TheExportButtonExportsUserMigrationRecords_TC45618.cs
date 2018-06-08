using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.ExportAndImport
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    class TheExportButtonExportsUserMigrationRecords_TC45618:TestBase
    {
        private readonly static string USER_MIGRATIONS_FILE_NAME = "user*";

        [Test]
        [Category("Integration")]
        [Category("UI")]
        public void TheExportButtonExportsUserMigrationRecords_Integration_45618()
        {
            RunTest("client2", "project2");
        }

        [Test]
        [Category("MailOnly")]
        [Category("UI")]
        public void TheExportButtonExportsUserMigrationRecords_MO_45618()
        {
            RunTest("client1", "project1");
        }

        [Test]
        [Category("MailWithDiscovery")]
        [Category("UI")]
        public void TheExportButtonExportsUserMigrationRecords_MD_45618()
        {
            RunTest("client2", "project1");
        }
        
        private void RunTest(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var project = client.GetByReference<Project>(projectName);
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = project.Name;
            string _downloadPath = Automation.Settings.DownloadsPath;

            TheExportButtonExportsUserMigrationRecords(_username, _password, _client, _projectName, _downloadPath);
        }

        private void TheExportButtonExportsUserMigrationRecords(string username, string password, string client, string projectName, string downloadPath)
        {
            var _manageUsersPage = Automation.Common
                                    .SingIn(username, password)
                                    .MigrateAndIntegrateSelect()
                                    .ClientSelect(client)
                                    .ProjectSelect(projectName)
                                    .UsersEdit()
                                    .GetPage<ManageUsersPage>();

            _manageUsersPage.SelectAllUsers();
            _manageUsersPage
                .PerformAction<ConfirmationDialog>(ActionType.Export)
                .Yes();

            var isFileDownloaded = Automation.Browser.IsFileDownloaded(USER_MIGRATIONS_FILE_NAME, WaitDefaults.FILE_DOWNLOAD_TIMEOUT_SEC, 3);
        
            Assert.IsTrue(isFileDownloaded, "Error with downloading logs");
        }

    }
}
