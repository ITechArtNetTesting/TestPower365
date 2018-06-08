using AutomationServices.SqlDatabase;
using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Utilities;
using NUnit.Framework;


namespace BinaryTree.Power365.Test.CommonTests.Rollback
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class TheRollbackCountIsDisplayed_TC43335 : TestBase
    {

        [Test]
        [Category("UI")]
        [Category("Integration")]
      //  [TestResource("client2", "project2", "entry7")]
        public void TheRollbackCountIsDisplayed_Integration_43335()
        {
           
            TestRun("client2", "project2","entry7");
        }

        //[Test]
        //[Category("UI")]
        //[Category("MailOnly")]
        //public void TheRollbackCountIsDisplayed_MO_34718()
        //{
        //    //TestRun("client1", "project1", "entry12", true);
        //    TestRun("client1", "project1", "entry11");
        //}

        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
     //   [TestResource("client2", "project1", "entry7")]
        public void TheRollbackCountIsDisplayed_MD_43335()
        {
            TestRun("client2", "project1", "entry7");
        }

        private void TestRun(string clientName, string projectName, string entry)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = _project.Name;
           
            string _userMigration = _project.GetByReference<UserMigration>(entry).Source;

            var database = Automation.Settings.GetByReference<Database>("t2t");
            string _connectionString = database.GetAzureSqlConnectionString();
            TheRollbackCountIsDisplayed(_username, _password, _client, _projectName, _userMigration,  _connectionString);
        }

        private void TheRollbackCountIsDisplayed(string _username, string _password, string _client, string _projectName, string _userMigration, string _connectionString)
        {
           var _projectDetailsPage = Automation.Common
                                        .SingIn(_username,  _password)
                                        .MigrateAndIntegrateSelect()
                                        .ClientSelect(_client)
                                        .ProjectSelect(_projectName)                                        
                                        .GetPage<ProjectDetailsPage>();

            int rollbackNumber= _projectDetailsPage.GetRollbackUsersNumber();
            int errorNumber = _projectDetailsPage.GetErrorUsersNumber();

            var databaseService = Automation.GetService<DatabaseService>();

            //Changing stateId
            databaseService.SetUserMigrationState(_client, _projectName, _userMigration, StateType.RollbackError);
            _projectDetailsPage.Refresh();
            //get the new value of rollback and error mail
            int new_rollbackNumber = _projectDetailsPage.GetRollbackUsersNumber();
            int new_errorNumber = _projectDetailsPage.GetErrorUsersNumber();

            // reset DB
            databaseService.ResetUser(_client, _projectName, _userMigration, StateType.Matched);
            
            //Verify
            Assert.AreEqual(new_errorNumber, errorNumber,  "The error count is not correct");
            Assert.Greater(new_rollbackNumber, rollbackNumber, "The rollback count is not correct");        
              

        } 

    }
}








