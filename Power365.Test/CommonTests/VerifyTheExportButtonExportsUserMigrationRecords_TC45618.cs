using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.CommonTests
{
    [TestFixture]
    public class VerifyTheExportButtonExportsUserMigrationRecords_TC45618:TestBase
    {
        public VerifyTheExportButtonExportsUserMigrationRecords_TC45618() : base() { }

        ManageUsersPage _manageUsersPage;

        private void verifyTheExportButtonExportsUserMigrationRecords(string username, string password, string client, string projectName,string downloadPath)
        {
            _manageUsersPage = Automation.Common
                                       .SingIn(username, password)
                                       .ClientSelect(client)
                                       .SelectMigrateAndIntegrate()
                                       .ProjectSelect(projectName)
                                       .UsersEdit()
                                       .GetPage<ManageUsersPage>();
            _manageUsersPage.SelectAllUsers();
            _manageUsersPage.DeleteUserMigrationsJobsLogs(downloadPath);
            _manageUsersPage.PerformAction(ActionType.Export);
            _manageUsersPage.ConfirmAction();
            Assert.IsTrue(_manageUsersPage.CheckUserMigrationLogs(downloadPath, 15), "Error with downloading logs");            
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

            verifyTheExportButtonExportsUserMigrationRecords(_username, _password, _client, _projectName, _downloadPath);
        }        

        [Test]
        [Category("Integration")]
        public void VerifyTheExportButtonExportsUserMigrationRecords_Integration_45618()
        {
            runTest("client2", "project2");
        }

        [Test]
        [Category("MailOnly")]
        public void VerifyTheExportButtonExportsUserMigrationRecords_MO_45618()
        {
            runTest("client1", "project1");
        }

        [Test]
        [Category("MailWithDiscovery")]
        public void VerifyTheExportButtonExportsUserMigrationRecords_MD_45618()
        {
            runTest("client2", "project1");
        }

    }
}
