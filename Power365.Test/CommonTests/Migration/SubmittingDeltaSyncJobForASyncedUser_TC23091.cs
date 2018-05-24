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
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.Migration
{
    [TestFixture]
    public class SubmittingDeltaSyncJobForASyncedUser_TC23091 : TestBase
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

        

        [Test]
        [Category("UI")]
        [Category("MailOnly")]
        [Category("Sync")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MO_23091()
        {
            RunTest("client1", "project1", "entry11");           
        }

        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        [Category("Sync")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MD_23091()
        {
            RunTest("client2", "project1", "entry6");            
        }

        [Test]
        [Category("UI")]
        [Category("Integration")]
        [Category("Sync")]
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
                    //_usersDetailsPage.PerformAction<UserDetailsDialog>(ActionType.Prepare); 
                    _usersDetailsPage.PerformAction(ActionType.Prepare);
                    _usersDetailsPage.ConfirmAction(true);
                    _usersDetailsPage.IsUserState(entry, StateType.Preparing, 2700000, 5);
                    _usersDetailsPage.IsUserState(entry, StateType.Prepared, 2700000, 5);

                }
               _usersDetailsPage.PerformAction(ActionType.Sync);
                _usersDetailsPage.ConfirmAction(true);
               _usersDetailsPage.IsUserState(entry, StateType.Syncing, 2700000, 5);
                _usersDetailsPage.IsUserState(entry, StateType.Synced1, 2700000, 5);
               _usersDetailsPage.PerformAction(ActionType.Sync);
               _usersDetailsPage.ConfirmAction(true);
               _usersDetailsPage.IsUserState(entry, StateType.Syncing, 2700000, 5);
               _usersDetailsPage.IsUserState(entry, StateType.Synced2, 2700000, 5);
        //    _usersDetailsPage.Close(); 
        }


    }
}
