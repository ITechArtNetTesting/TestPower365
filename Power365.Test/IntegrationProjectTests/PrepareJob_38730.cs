using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.Test.CommonTests;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.IntegrationProjectTests
{
    public class PrepareJob_38730: UITestBase
    {
        public PrepareJob_38730()
                  : base(LogManager.GetLogger(typeof(PrepareJob_38730))) { }

        private string _client;
        private string _username;
        private string _project;
        private string _password;
        private string _userMigration;
        private ManageUsersPage _manageUsersPage;

        [TestInitialize]
        public void ClassInit()
        {
            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project2");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _project = project.Name;
            _userMigration = project.GetByReference<UserMigration>("entry12").Source;
        }


        [TestMethod]
        [TestCategory("Integration")]
        public void PrepareJob_Integration_38730()
        {
            _manageUsersPage = Automation.Common
                                       .SingIn(_username, _password)
                                       .ClientSelect(_client)
                                       .ProjectSelect(_project)
                                       .UsersEdit()
                                       .UsersFindAndPerformAction(_userMigration,ActionType.Prepare)
                                       .UsersValidateState(_userMigration, StateType.Preparing)
                                       .GetPage<ManageUsersPage>();
           
            PerformActionAndWaitForState(sourceMailbox1, ActionType.Prepare, State.Preparing, 60000, 10);



        }

    }


}
