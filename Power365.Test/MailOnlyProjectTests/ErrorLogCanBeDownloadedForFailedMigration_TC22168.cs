using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.MailOnlyProjectTests
{
    [TestFixture]
    class ErrorLogCanBeDownloadedForFailedMigration_TC22168 : TestBase
    {

        public ErrorLogCanBeDownloadedForFailedMigration_TC22168()
                  : base() { }

        private readonly static string LOGS_FILE_NAME = "*Logs.csv";

        [Test]
        [Category("Log")]
        [Category("MailOnly")]
        [Category("UI")]
        public void ErrorLogCanBeDownloadedForFailedMigration_22168()
        {
            //init
            var client = Automation.Settings.GetByReference<Client>("client1");
            var project = client.GetByReference<Project>("project1");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _project = project.Name;
            string _userMigration = project.GetByReference<UserMigration>("entry13").Source;
            string _downloadPath = Automation.Settings.DownloadsPath;


            //test steps
            var _manageUsersPage = Automation.Common
                                       .SingIn(_username, _password)
                                       .MigrateAndIntegrateSelect()
                                       .ClientSelect(_client)
                                       .ProjectSelect(_project)
                                       .UsersEdit()
                                       .UsersFindAndPerformAction(_userMigration,ActionType.Sync)                                       
                                       .GetPage<ManageUsersPage>();

            var _userDetailsPage = _manageUsersPage.OpenUserDetails(_userMigration);
            _userDetailsPage.PerformAction<ConfirmationDialog>(ActionType.Sync).Yes();
            _userDetailsPage.UsersValidateState(_userMigration, StateType.SyncError);
            _userDetailsPage.JobsTable.ClickLogsByValue("Start Error");

            Assert.IsTrue(_userDetailsPage.IsLogsDownload(_downloadPath, LOGS_FILE_NAME, 15), "Error log can not be downloaded ");          
        }
    }
}
