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

namespace BinaryTree.Power365.Test.CommonTests.MigrationWaves
{
    [TestClass]
    public class UsersViewHasTheTabToSwitchToMigrationWaveView_TC30917 :
        TestBase
    {
        public UsersViewHasTheTabToSwitchToMigrationWaveView_TC30917(): base() { }

        private string _client;
        private string _username;
        private string _project;
        private string _password;
        private ManageUsersPage _manageUsersPage;

        //[TestInitialize]
        //public void ClassInit()
        //{
        //    var client = Automation.Settings.GetByReference<Client>("client2");
        //    var project = client.GetByReference<Project>("project1");
        //    var username = client.Administrator.Username;
        //    var password = client.Administrator.Password;
                       
        //    _client = client.Name;
        //    _username = client.Administrator.Username;
        //    _password = client.Administrator.Password;
        //    _project = project.Name;         
        //}
         


        [TestMethod]
        [TestCategory("MailOnly")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_MO_30917()
        {
            CallTest("client1", "project1");
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_MD_30917()
        {           
            CallTest("client2", "project1");
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_Integration_30917()
        {                 
            CallTest("client2", "project2");
        }

        private void CallTest(string init_client, string init_project)
        {
            var client = Automation.Settings.GetByReference<Client>(init_client);
            var project = client.GetByReference<Project>(init_project);
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _project = project.Name;
            UsersViewHasTheTabToSwitchToMigrationWaveView(_username, _password, _client, _project);

        }


        private void UsersViewHasTheTabToSwitchToMigrationWaveView(string username, string password, string client, string project)
        {
            _manageUsersPage = Automation.Common
                                        .SingIn(username, password)
                                        .ClientSelect(client)
                                        .ProjectSelect(project)
                                        .UsersEdit() 
                                        .GetPage<ManageUsersPage>();
           
         
            //Verify migration Waves Tab is visible
            Assert.IsTrue(_manageUsersPage.MigrationWaveTabIsVisible(), "Migration Waves Tab is not visible");
            _manageUsersPage.SwichToTab("Migration Waves");

            //Verify migration Waves Tab were opened
            Assert.IsTrue(_manageUsersPage.CheckMigrationWavesTabOpen(), "Migration Waves Tab was not opened");
            
           
        }
    }

}
