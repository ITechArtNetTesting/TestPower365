﻿using BinaryTree.Power365.AutomationFramework;
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
        private string _project;
        private string _password;
        private string _entry;
        private ManageUsersPage _manageUsersPage;

        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserFor_MO_23091()
        {            
            var client= Automation.Settings.GetByReference<Client>("client1");            

            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _project = client.GetByReference<Project>("project2").Name;
            _entry = (client.GetByReference<Project>("project2").UserMigrations.Single(a=>a.Reference=="entry6")).Source;

            VerifySubmittingDeltaSyncJobForASyncedUser(
                _username,
                _password,
                _client,
                _project,
                _entry
                );
        }        

        private void VerifySubmittingDeltaSyncJobForASyncedUser(string login, string password, string client, string projectName, string entry)
        {
            _manageUsersPage = Automation.Common
                                       .SingIn(login, password)
                                       .ClientSelect(client)
                                       .ProjectSelect(projectName)
                                       .UsersEdit()
                                       .GetPage<ManageUsersPage>();
            _manageUsersPage.PerformSearch(entry);
            _manageUsersPage.OpenDetailsOf(entry);
            _manageUsersPage.PerformSyncFromDetails();
            _manageUsersPage.ConfirmAction();
            _manageUsersPage.WaitForState_DetailPage(entry, StateType.Syncing, 2700000, 5);
            _manageUsersPage.WaitForState_DetailPage(entry, StateType.Synced, 2700000, 5);           
            _manageUsersPage.PerformSyncFromDetails();
            _manageUsersPage.ConfirmAction();
            _manageUsersPage.WaitForState_DetailPage(entry, StateType.Syncing, 2700000, 5);
            _manageUsersPage.WaitForState_DetailPage(entry, StateType.Synced, 2700000, 5);
            _manageUsersPage.CloseDetailsWindow();
        }
    }
}
