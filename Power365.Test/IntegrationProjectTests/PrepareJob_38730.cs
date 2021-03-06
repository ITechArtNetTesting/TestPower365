﻿using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Utilities;
using NUnit.Framework;


namespace BinaryTree.Power365.Test.IntegrationProjectTests
{  
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class PrepareJob_38730: TestBase
    {
        [Test]
        [Category("Integration")]
        [Category("UI")]
       // [TestResource("client2", "project2", "entry12")]
        [Category("Prepare")]
        public void PrepareJob_Integration_38730()
        {
            //init
            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project2");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _project = project.Name;
            string _userMigration_source = project.GetByReference<UserMigration>("entry12").Source;
            string _userMigration_target = project.GetByReference<UserMigration>("entry12").Target;

            var database = Automation.Settings.GetByReference<Database>("t2t");
            string _connectionString = database.GetAzureSqlConnectionString();


            //test steps
            var _manageUsersPage = Automation.Common
                                       .SingIn(_username, _password)
                                       .MigrateAndIntegrateSelect()
                                       .ClientSelect(_client)
                                       .ProjectSelect(_project)
                                       .UsersEdit()                                       
                                       .GetPage<ManageUsersPage>();

            _manageUsersPage.Search(_userMigration_source);
            _manageUsersPage.UsersTable.ClickRowByValue(_userMigration_source);
            _manageUsersPage.PerformAction<ConfirmationDialog>(ActionType.Prepare)
                                      .Yes();
            _manageUsersPage.IsUserState(_userMigration_source, StateType.Preparing, WaitDefaults.STATE_PREPARING_TIMEOUT_SEC, 5);

            //Verify
            UserMigrationQuery queryExecuter = new UserMigrationQuery();
            var  resultInDB= queryExecuter.SelectIsLockedByUsermail(_userMigration_target, _project, _connectionString);
            Assert.AreEqual(resultInDB, true , "The  UserMigration record IsLocked column wasn't be set to True ");
        }

    }
}
