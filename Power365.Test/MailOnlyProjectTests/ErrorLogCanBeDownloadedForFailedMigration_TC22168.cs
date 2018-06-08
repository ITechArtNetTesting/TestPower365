using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.MailOnlyProjectTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    class ErrorLogCanBeDownloadedForFailedMigration_TC22168 : TestBase
    {
        private readonly static string LOGS_FILE_NAME = "*Logs.csv";

        [Test]
        [Category("Log")]
        [Category("MailOnly")]
        [Category("UI")]
        [TestResource("client1", "project1", "entry13")]
        public void ErrorLogCanBeDownloadedForFailedMigration_22168()
        {
            var client = Automation.Settings.GetByReference<Client>("client1");
            var project = client.GetByReference<Project>("project1");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            string clientName = client.Name;
            string projectName = project.Name;
            string _userMigration = project.GetByReference<UserMigration>("entry13").Source;
            string _downloadPath = Automation.Settings.DownloadsPath;
            
            //test steps
            var _manageUsersPage = Automation.Common
                                       .SingIn(username, password)
                                       .MigrateAndIntegrateSelect()
                                       .ClientSelect(clientName)
                                       .ProjectSelect(projectName)
                                       .UsersEdit()
                                       .UsersFindAndPerformAction(_userMigration,ActionType.Sync)                                       
                                       .GetPage<ManageUsersPage>();

            var _userDetailsPage = _manageUsersPage.OpenUserDetails(_userMigration);
           
            _userDetailsPage.UsersValidateState(_userMigration, StateType.SyncError, WaitDefaults.STATE_SYNCED_TIMEOUT_SEC, 5);

            _userDetailsPage.JobsTable.ClickRowLinkByValue("Start Error", UserDetailsDialog.DOWNLOAD_LOGS);

            var isFileDownloaded = Automation.Browser.IsFileDownloaded(LOGS_FILE_NAME, WaitDefaults.FILE_DOWNLOAD_TIMEOUT_SEC, 3);

            Assert.IsTrue(isFileDownloaded, "Error log can not be downloaded ");          
        }
    }
}
