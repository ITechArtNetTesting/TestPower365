using BinaryTree.Power365.AutomationFramework;
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

        private ErrorsPage atErrorsPage;

        private void verifyTheExportButtonExportsUserMigrationRecords(string username, string password, string client, string projectName,string downloadPath)
        {
            atErrorsPage = Automation.Common
                .SingIn(username, password)
                .ClientSelect(client)
                .ProjectSelect(projectName)
                .GetPage<ProjectDetailsPage>()
                .Menu
                .ClickErrors();
            Assert.IsTrue(atErrorsPage.CheckExportAreDisplayed());
            atErrorsPage.SelectAllUsers();
            atErrorsPage.ExportSelected();
            atErrorsPage.DeleteUserMigrationsJobsLogs(downloadPath);
            atErrorsPage.ClickYesButton();
            Assert.IsTrue(atErrorsPage.CheckUserMigrationJobsLogs(downloadPath, 15), "Error with downloading logs");
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

    }
}
