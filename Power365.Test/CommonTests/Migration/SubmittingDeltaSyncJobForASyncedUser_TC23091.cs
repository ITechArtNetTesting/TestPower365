using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.Migration
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    class SubmittingDeltaSyncJobForASyncedUser_TC23091 : TestBase
    {
        public SubmittingDeltaSyncJobForASyncedUser_TC23091()
                   : base() { }    
        
        [Test]
        [Category("SmokeTest")]
        [Category("UI")]
        [Category("MailOnly")]
        [Category("Sync")]
      //  [TestResource("client1", "project1", "entry11")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MO_23091()
        {
            RunTest("client1", "project1", "entry11");           
        }

        [Test]
        [Category("SmokeTest")]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        [Category("Sync")]
      //  [TestResource("client2", "project1", "entry8")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MD_23091()
        {
            RunTest("client2", "project1", "entry8");            
        }

        [Test]
        [Category("SmokeTest")]
        [Category("UI")]
        [Category("Integration")]
        [Category("Sync")]
       // [TestResource("client2", "project2", "entry10")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_Integration_23091()
        {
            RunTest("client2", "project2", "entry10", true);
        }

        private void RunTest(string init_client, string init_project, string entry, bool isInetgrat=false)
        {
            var  client = Automation.Settings.GetByReference<Client>(init_client);
            var project = client.GetByReference<Project>(init_project);
            string username = client.Administrator.Username;
            string  password = client.Administrator.Password;
            string _client = client.Name;
            var _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = project.Name;            
            string _entry = project.GetByReference<UserMigration>(entry).Source;

            VerifySubmittingDeltaSyncJobForASyncedUser(
                _username,
                _password,
                _client,
                _projectName,
                _entry, isInetgrat
                );
        }

        private void VerifySubmittingDeltaSyncJobForASyncedUser(string login, string password, string client, string projectName, string entry, bool isIntegrat)
        {
            ManageUsersPage _manageUsersPage;
            _manageUsersPage = Automation.Common
                                        .SingIn(login, password)
                                        .MigrateAndIntegrateSelect()
                                        .ClientSelect(client)
                                        .ProjectSelect(projectName)
                                        .UsersEdit()
                                        .GetPage<ManageUsersPage>();

            _manageUsersPage.Search(entry);
            var _usersDetailsPage = _manageUsersPage.OpenUserDetails(entry);

            if (isIntegrat)
            {
                // Integration project
                _usersDetailsPage
                    .PerformAction<ConfirmationDialog>(ActionType.Prepare)
                    .Yes();

                _usersDetailsPage.UsersValidateState(entry, StateType.Preparing, WaitDefaults.STATE_PREPARING_TIMEOUT_SEC, 5);
                _usersDetailsPage.UsersValidateState(entry, StateType.Prepared, WaitDefaults.STATE_PREPARED_TIMEOUT_SEC, 60);
            }

            _usersDetailsPage
                .PerformAction<ConfirmationDialog>(ActionType.Sync)
                .Yes();

            //verify Sync1
            _usersDetailsPage.UsersValidateState(entry, StateType.Syncing, WaitDefaults.STATE_SYNCING_TIMEOUT_SEC, 5);
            _usersDetailsPage.UsersValidateState(entry, StateType.Synced1, WaitDefaults.STATE_SYNCED_TIMEOUT_SEC, 60);

            //sync job is ran again
            _usersDetailsPage
                .PerformAction<ConfirmationDialog>(ActionType.Sync)
                .Yes();

            //verify Sync2
            _usersDetailsPage.UsersValidateState(entry, StateType.Syncing, WaitDefaults.STATE_SYNCING_TIMEOUT_SEC, 5);
            _usersDetailsPage.UsersValidateState(entry, StateType.Synced2, WaitDefaults.STATE_SYNCED_TIMEOUT_SEC, 60);

        }




    }
}
