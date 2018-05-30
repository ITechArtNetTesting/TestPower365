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
            TestRun("client2", "project2", true);
        }
        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        public void TheRollbackCountIsDisplayed_MD_34718()
        {
            TestRun("client2", "project1", false);
        }

        private void TestRun(string clientName, string projectName, bool isIntegration = false)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = _project.Name;
            string _userMigration = _project.GetByReference<UserMigration>("entry2").Source;

            var database = Automation.Settings.GetByReference<Database>("sqlt2t01");
            string _connectionString = database.GetConnectionString();
            TheRollbackCountIsDisplayed(_username, _password, _client, _projectName, _userMigration,  _connectionString,isIntegration);
        }

        private void TheRollbackCountIsDisplayed(string _username, string _password, string _client, string _projectName, string _userMigration, string _connectionString, bool isIntegration)
        {
           var _projectDetailsPage = Automation.Common
                                        .SingIn(_username,  _password)
                                        .ClientSelect(_client)
                                        .ProjectSelect(_projectName)                                        
                                        .GetPage<ProjectDetailsPage>();
            _projectDetailsPage.GetRollbackNumber();
           // UserMigrationQuery queryExecuter = new UserMigrationQuery();
           // queryExecuter.SetMigrtationStateId(_userMigration, _projectName, _connectionString);

                       
            //Assert.IsFalse(migrationProfilePage.IsDeleteLinkVisible("Default"), "Delete link is visible for Default profile");
            //Assert.IsTrue(migrationProfilePage.IsDeleteLinkVisible("TestProfile1"), "Delete link is not visible for not Default profile");

            //var usersPage = migrationProfilePage.Menu.
            //                   ClickUsers();
            //usersPage.Search(_userMigration);
            //usersPage.UsersTable.ClickRowByValue(_userMigration);
            //var migrationProfileDialog = usersPage.PerformAction<SelectMigrationProfileDialog>(ActionType.AddToProfile);
            //Assert.IsFalse(migrationProfileDialog.IsProfileActionVisible("Default", "Delete"), "Delete link is visible for Default profile in the User Page");
            //Assert.IsTrue(migrationProfileDialog.IsProfileActionVisible("TestProfile1", "Delete"), "Delete link is not visible for not Default profile in the User Page");

        }
    }
}








