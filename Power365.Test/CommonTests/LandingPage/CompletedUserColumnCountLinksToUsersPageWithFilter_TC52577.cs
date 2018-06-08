using AutomationServices.SqlDatabase;
using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Utilities;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.LandingPage
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class CompletedUserColumnCountLinksToUsersPageWithFilter_TC52577 : TestBase
    {
        [Test]
        [Category("UI")]
        [Category("Integration")]
      //  [TestResource("client2", "project2", "entry13")]
      //  [TestResource("client2", "project2", "entry6")]
        public void CompletedUserColumnCountLinksToUsersPageWithFilter_Integration_52577()
        {
            // have to change entry
            TestRun("client2", "project2", "entry13", "entry6");
        }
        
        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
     //   [TestResource("client2", "project1", "entry9")]
       // [TestResource("client2", "project1", "entry7")]
        public void CompletedUserColumnCountLinksToUsersPageWithFilter_MD_52577()
        {
            // have to change entry
            TestRun("client2", "project1", "entry7", "entry9");
        }

        private void TestRun(string clientName, string projectName, string entry1, string entry2)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            string username = client.Administrator.Username;
            string password = client.Administrator.Password;            
            string userMigration1 = _project.GetByReference<UserMigration>(entry1).Source;
            string userMigration2 = _project.GetByReference<UserMigration>(entry2).Source;

            var database = Automation.Settings.GetByReference<Database>("t2t");
            string _connectionString = database.GetAzureSqlConnectionString();

            CompletedUserColumnCountLinksToUsersPageWithFilter(username, password, client.Name, _project.Name, userMigration1, userMigration2);
        }

        private void CompletedUserColumnCountLinksToUsersPageWithFilter(string username, string password, string client, string projectName, string userMigration1, string userMigration2)
        {
            var projectDetailsPage = Automation.Common
                                         .SingIn(username, password)
                                         .MigrateAndIntegrateSelect()
                                         .ClientSelect(client)
                                         .ProjectSelect(projectName)
                                         .GetPage<ProjectDetailsPage>();

            int initialCompleted = projectDetailsPage.GetCompletedNumber();

            var databaseService = Automation.GetService<DatabaseService>();

            databaseService.SetUserMigrationState(client, projectName, userMigration1, StateType.Complete);
            databaseService.SetUserMigrationState(client, projectName, userMigration2, StateType.Moved);

            int actualCompleted = projectDetailsPage.GetCompletedNumber();

            //Verify
            Assert.GreaterOrEqual(actualCompleted, initialCompleted, "The completed count is not correct");
            
            var manageUsersPage = projectDetailsPage.clickCompletedCountUsers();
            var rowHasValue = manageUsersPage.UsersTable.RowHasValue(userMigration1, "Moved");

            databaseService.SetUserMigrationState(client, projectName, userMigration1, StateType.Matched);
            databaseService.SetUserMigrationState(client, projectName, userMigration2, StateType.Matched);

            Assert.IsTrue(rowHasValue, "The rollback count is not correct");

         
        }
    }
}
