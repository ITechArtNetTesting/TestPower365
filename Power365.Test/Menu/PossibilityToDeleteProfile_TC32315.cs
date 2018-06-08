using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.Menu
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class PossibilityToDeleteProfile_TC32315: TestBase
    {
        readonly string PROFILE_NAME = "TestProfile1";

        [Test]
        [Category("UI")]
        [Category("MailOnly")]
        public void PossibilityToDeleteProfile_MO_32315()
        {
            TestRun("client1", "project1");
        }
        
        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void PossibilityToDeleteProfile_Integration_32315()
        {
            TestRun("client2", "project2", true);
        }
        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        public void PossibilityToDeleteProfile_MD_32315()
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
            PossibilityToDeleteProfile(_username, _password, _client, _projectName, _userMigration, isIntegration);
            
        }

        private void PossibilityToDeleteProfile(string _username, string _password, string _client, string _projectName, string _userMigration, bool isIntegration)
        {
            var migrationProfilePage = Automation.Common
                 .SingIn(_username, _password)
                 .MigrateAndIntegrateSelect()
                 .ClientSelect(_client)
                 .ProjectSelect(_projectName)
                 .GetPage<ProjectDetailsPage>()
                 .Menu
                 .ClickMigrationProfiles();

            
            //I started to add a MigrationProfileWorkflow.
            //The migration profile needs to be create then deleted in a try/finally to ensure we do not collide.
            if (!migrationProfilePage.ProfileTable.RowExists(PROFILE_NAME, 0))
            {          
             
               var createProfile= migrationProfilePage.ClickAddNewProfile();
                migrationProfilePage= createProfile.ProfileName(PROFILE_NAME)
                    .UpdateOutlookProfiles()
                    .CopyLitigationHoldSettingsAndData()
                    .TranslateEmailAddress()
                    .MigrateMailboxContent()
                    .IncludeLargeItems()
                    .BadItemLimit(0)
                    .BadFolderLimit(0)
                    .Submit();
                       
            }

           Assert.IsFalse(migrationProfilePage.IsDeleteLinkVisible("Default"), "Delete link is visible for Default profile");
           Assert.IsTrue(migrationProfilePage.IsDeleteLinkVisible("TestProfile1"), "Delete link is not visible for not Default profile");

            var usersPage =  migrationProfilePage
                   .Menu
                   .ClickUsers();

            usersPage.Search(_userMigration);
            usersPage.UsersTable.ClickRowByValue(_userMigration);

            var migrationProfileDialog = usersPage.PerformAction<SelectMigrationProfileDialog>(ActionType.AddToProfile);
            Assert.IsFalse(migrationProfileDialog.IsProfileActionVisible("Default", "Delete"), "Delete link is visible for Default profile in the User Page");
            Assert.IsTrue(migrationProfileDialog.IsProfileActionVisible("TestProfile1", "Delete"), "Delete link is not visible for not Default profile in the User Page");

        }
    }
}
