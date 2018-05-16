﻿using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BinaryTree.Power365.Test.CommonTests.MigrationWaves
{
    [TestClass]
    public class MigrationWave_Sync_TC30919 : TestBase
    {
        public MigrationWave_Sync_TC30919()
                   : base(LogManager.GetLogger(typeof(MigrationWave_Sync_TC30919))) { }

        private string _client;
        private string _username;
        private string _project;
        private string _password;
        private ManageUsersPage _manageUsersPage;

       [TestInitialize]
        public  void ClassInit()
        {
            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project1");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            var sourceTenant = Automation.Settings.GetByReference<Tenant>(project.Source);
            var targetTenant = Automation.Settings.GetByReference<Tenant>(project.Target);

            //var userMigration1 = project.GetByReference<UserMigration>("entry3");
            //var userMigration2 = project.GetByReference<UserMigration>("entryps1");

            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _project = project.Name;

            //_sourceAdminUser = sourcePowershellUser.Username;
            //_sourceAdminPassword = sourcePowershellUser.Password;

            //_targetAdminUser = targetPowershellUser.Username;
            //_targetAdminPassword = targetPowershellUser.Password;

            //_sourceMailbox = userMigration1.Source;
            //_targetMailbox = userMigration1.Target;
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void PerformSync()
        {
            _manageUsersPage = Automation.Common
                                        .SingIn(_username, _password)
                                        .ClientSelect(_client)
                                        .ProjectSelect(_project)
                                        .UsersEdit()
                                        //.UsersPerformAction(user, ActionType.Sync)
                                        //.UsersValidateState(user, StateType.Syncing)
                                        //.UsersValidateState(user, StateType.Synced)
                                        .GetPage<ManageUsersPage>();
        }

    }
}
