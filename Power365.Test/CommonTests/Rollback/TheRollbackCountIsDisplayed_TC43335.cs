using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Utilities;
using NUnit.Framework;


namespace BinaryTree.Power365.Test.CommonTests.Rollback
{
    class TheRollbackCountIsDisplayed_TC43335 : TestBase
    {

        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void TheRollbackCountIsDisplayed_Integration_34718()
        {
            // have to change entry
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
        public void TheRollbackCountIsDisplayed_MD_34718()
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
            string _userMigration = _project.GetByReference<UserMigration>(entry).Target;

            var database = Automation.Settings.GetByReference<Database>("sqlt2t01");
            string _connectionString = database.GetConnectionString();
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

            //Changing stateId
            UserMigrationQuery queryExecuter = new UserMigrationQuery();
            queryExecuter.SetMigrtationStateToRollbackError(_userMigration, _projectName, _connectionString);
           
            //Verify
            Assert.AreEqual(_projectDetailsPage.GetErrorUsersNumber(), errorNumber,  "The error count is not correct");
            Assert.Greater(_projectDetailsPage.GetRollbackUsersNumber(), rollbackNumber, "The rollback count is not correct");
                       
            queryExecuter.SetMigrtationStateToMached(_userMigration, _projectName, _connectionString);


        }
    }
}








