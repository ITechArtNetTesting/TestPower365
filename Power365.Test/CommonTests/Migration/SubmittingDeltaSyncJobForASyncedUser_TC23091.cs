using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryTree.Power365.Test.CommonTests.Migration
{
    [TestClass]
    public class SubmittingDeltaSyncJobForASyncedUser_TC23091 : UITestBase
    {
        public SubmittingDeltaSyncJobForASyncedUser_TC23091()
                   : base() { }


        private string _client;
        private string _username;
        private string _projectName;
        private string _password;
        private string _entry;
        private ManageUsersPage _manageUsersPage;
        private UserDetailsDialog _usersDetailsPage;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext){}

        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("MailOnly")]
        [TestCategory("Sync")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MO_23091()
        {
            RunTest("client1", "project1", "entry11");           
        }

        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("MailWithDiscovery")]
        [TestCategory("Sync")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MD_23091()
        {
            RunTest("client2", "project1", "entry6");            
        }

        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Integration")]
        [TestCategory("Sync")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_Integration_23091()
        {
            RunTest("client2", "project2", "entry10",true);            
        }

        private void RunTest(string init_client, string init_project, string entry, bool isInetgrat=false)
        {
            var client = Automation.Settings.GetByReference<Client>(init_client);
            var project = client.GetByReference<Project>(init_project);
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _projectName = project.Name;            
            _entry = project.GetByReference<UserMigration>(entry).Source;

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
            _manageUsersPage = Automation.Common
                                       .SingIn(login, password)
                                       .ClientSelect(client)
                                       .ProjectSelect(projectName)
                                       .UsersEdit()
                                       .GetPage<ManageUsersPage>();
            _manageUsersPage.Search(entry);
            _usersDetailsPage = _manageUsersPage.OpenUserDetails(entry);
            if (isIntegrat)
            {
                _usersDetailsPage.PerformAction<UserDetailsDialog>(ActionType.Prepare); 
               // _usersDetailsPage.ConfirmAction();
                _usersDetailsPage.IsUserState(entry, StateType.Preparing, 2700000, 5);
                _usersDetailsPage.IsUserState(entry, StateType.Prepared, 2700000, 5);

            }
            _usersDetailsPage.PerformAction<UserDetailsDialog>(ActionType.Sync);           
            _usersDetailsPage.IsUserState(entry, StateType.Syncing, 2700000, 5);
            _usersDetailsPage.IsUserState(entry, StateType.Synced1, 2700000, 5);
            _usersDetailsPage.PerformAction<UserDetailsDialog>(ActionType.Sync);          
            _usersDetailsPage.IsUserState(entry, StateType.Syncing, 2700000, 5);
            _usersDetailsPage.IsUserState(entry, StateType.Synced2, 2700000, 5);
            _usersDetailsPage.Close(); 
        }


    }
}
