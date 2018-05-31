using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.ExportAndImport
{
    [TestFixture]
    public class TheExportButtonExportsUserMigrationRecords_TC45618:TestBase
    {
        public TheExportButtonExportsUserMigrationRecords_TC45618() : base() { }
        private readonly static string USER_MIGRATIONS_FILE_NAME = "user-migrations-*.csv";

        private void TheExportButtonExportsUserMigrationRecords(string username, string password, string client, string projectName,string downloadPath)
        {
            var _manageUsersPage = Automation.Common
                                       .SingIn(username, password)
                                       .MigrateAndIntegrateSelect()
                                       .ClientSelect(client)                                       
                                       .ProjectSelect(projectName)
                                       .UsersEdit()
                                       .GetPage<ManageUsersPage>();
            _manageUsersPage.SelectAllUsers();
            _manageUsersPage.DeleteLogs(downloadPath, USER_MIGRATIONS_FILE_NAME);
            _manageUsersPage.PerformAction<ConfirmationDialog>(ActionType.Export).Yes();
          
            Assert.IsTrue(_manageUsersPage.IsLogsDownload(downloadPath, USER_MIGRATIONS_FILE_NAME, 15), "Error with downloading logs");
        }
    

        private void runTest(string clientName, string projectName)
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

        [Test]
        [Category("Integration")]
        [Category("UI")]
        public void TheExportButtonExportsUserMigrationRecords_Integration_45618()
        {
            runTest("client2", "project2");
        }

        [Test]
        [Category("MailOnly")]
        [Category("UI")]
        public void TheExportButtonExportsUserMigrationRecords_MO_45618()
        {
            runTest("client1", "project1");
        }

        [Test]
        [Category("MailWithDiscovery")]
        [Category("UI")]
        public void TheExportButtonExportsUserMigrationRecords_MD_45618()
        {
            runTest("client2", "project1");
        }

    }
}
