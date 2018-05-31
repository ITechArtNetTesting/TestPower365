using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.CommonTests.LandingPage
{
    public class CompletedUserColumnCountLinksToUsersPageWithFilter_TC52577 : TestBase
    {
        public CompletedUserColumnCountLinksToUsersPageWithFilter_TC52577() : base() { }

        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void CompletedUserColumnCountLinksToUsersPageWithFilter_Integration_52577()
        {
            // have to change entry
            TestRun("client2", "project2", "entry7", "entry6");
        }


        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        public void CompletedUserColumnCountLinksToUsersPageWithFilter_MD_52577()
        {
            // have to change entry
            TestRun("client2", "project1", "entry7", "entry6");
        }

        private void TestRun(string clientName, string projectName, string entry1, string entry2)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = _project.Name;
            string _userMigration1 = _project.GetByReference<UserMigration>(entry1).Target;
            string _userMigration2 = _project.GetByReference<UserMigration>(entry2).Target;

            var database = Automation.Settings.GetByReference<Database>("sqlt2t01");
            string _connectionString = database.GetConnectionString();
            CompletedUserColumnCountLinksToUsersPageWithFilter(_username, _password, _client, _projectName, _userMigration1, _userMigration2, _connectionString);
        }

        private void CompletedUserColumnCountLinksToUsersPageWithFilter(string _username, string _password, string _client, string _projectName, string _userMigration1, string _userMigration2, string _connectionString)
        {
            var _projectDetailsPage = Automation.Common
                                         .SingIn(_username, _password)
                                         .MigrateAndIntegrateSelect()
                                         .ClientSelect(_client)
                                         .ProjectSelect(_projectName)
                                         .GetPage<ProjectDetailsPage>();

            int completedNumber = _projectDetailsPage.GetCompletedNumber();

            //Changing stateId
            UserMigrationQuery queryExecuter = new UserMigrationQuery();
            queryExecuter.SetMigrtationStateToCompleted(_userMigration1, _projectName, _connectionString);
            queryExecuter.SetMigrtationStateToMoved(_userMigration2, _projectName, _connectionString);
          
            //Verify
            Assert.GreaterOrEqual(_projectDetailsPage.GetCompletedNumber(), completedNumber, "The completed count is not correct");


            var manageUsersPage=  _projectDetailsPage.clickCompletedCountUsers();
            var tt= manageUsersPage.UsersTable.RowHasValue(_userMigration1,"Moved");
            var pp= manageUsersPage.UsersTable.RowHasValue(_userMigration1, "Complete");

            Assert.IsTrue(manageUsersPage.UsersTable.RowHasValue(_userMigration1, "Moved"), "The rollback count is not correct");

            queryExecuter.SetMigrtationStateToMached(_userMigration1, _projectName, _connectionString);
            queryExecuter.SetMigrtationStateToMached(_userMigration2, _projectName, _connectionString);


        }


    }
}
