using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.CommonTests.Migration_Tests
{
    [TestClass]
    public class VerifySubmittingDeltaSyncJobForASyncedUser_TC23091:TestBase
    {
        public VerifySubmittingDeltaSyncJobForASyncedUser_TC23091() : base(LogManager.GetLogger(typeof(VerifySubmittingDeltaSyncJobForASyncedUser_TC23091))) { }

        private string _client;
        private string _username;
        private string _projectName;
        private string _password;
        private string _entry;
        private ManageUsersPage _manageUsersPage;
        private UserDetailsPage _usersDetailsPage;

        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MO_23091()
        {                                    
            VerifySubmittingDeltaSyncJobForASyncedUser("client1", "project2", "entry6");
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MD_23091()
        {                        
            VerifySubmittingDeltaSyncJobForASyncedUser("client2", "project1", "entry5");
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_Integration_23091()
        {                        
            VerifySubmittingDeltaSyncJobForASyncedUser("client2", "project2", "entry10");
        }

        private void SetTestCaseParams(string clientName,string projectName,string entry)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _projectName = _project.Name;
            _entry = _project.UserMigrations.Single(a => a.Reference == entry).Source;
        }

        private void VerifySubmittingDeltaSyncJobForASyncedUser(string client, string project, string entry)
        {
            SetTestCaseParams(client,project,entry);
            _manageUsersPage = Automation.Common
                                       .SingIn(_username, _password)
                                       .ClientSelect(_client)
                                       .ProjectSelect(_projectName)
                                       .UsersEdit()
                                       .GetPage<ManageUsersPage>();
            _manageUsersPage.PerformSearch(_entry);
            _usersDetailsPage= _manageUsersPage.OpenDetailsOf(_entry);
            _usersDetailsPage.PerformAction(ActionType.Sync);
            _usersDetailsPage.ConfirmAction();
            _usersDetailsPage.WaitForState_DetailPage(_entry, StateType.Syncing, 2700000, 5);
            _usersDetailsPage.WaitForState_DetailPage(_entry, StateType.Synced1, 2700000, 5);
            _usersDetailsPage.PerformAction(ActionType.Sync);
            _usersDetailsPage.ConfirmAction();
            _usersDetailsPage.WaitForState_DetailPage(_entry, StateType.Syncing, 2700000, 5);
            _usersDetailsPage.WaitForState_DetailPage(_entry, StateType.Synced2, 2700000, 5);
            _usersDetailsPage.CloseDetailsWindow();
        }
    }
}
