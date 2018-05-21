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
    public class UsersViewHasTheTabToSwitchToMigrationWaveView_TC30917 : TestBase
    {
        public UsersViewHasTheTabToSwitchToMigrationWaveView_TC30917()
                  : base(LogManager.GetLogger(typeof(UsersViewHasTheTabToSwitchToMigrationWaveView_TC30917))) { }

        private string _client;
        private string _username;
        private string _project;
        private string _password;
        private ManageUsersPage _manageUsersPage;

        [TestInitialize]
        public void ClassInit()
        {
            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project1");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;
                       
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
        [TestCategory("MailOnly")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_MO_30917()
        {
            var client = Automation.Settings.GetByReference<Client>("client1");
            var project = client.GetByReference<Project>("project1");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _project = project.Name;

      
            UsersViewHasTheTabToSwitchToMigrationWaveView(_username, _password, _client, _project);
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_MD_30917()
        {           
            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project1");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _project = project.Name;


            UsersViewHasTheTabToSwitchToMigrationWaveView(_username, _password, _client, _project);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_Integration_30917()
        {
          
            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project2");
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

            
            Assert.IsTrue(_manageUsersPage.MigrationWavesLinkIsVisible(), "Migration Waves Tab is not visible");
            _manageUsersPage.OpenMigrationWavesTab();
            Assert.IsTrue(_manageUsersPage.IsMigrationWavesTabOpen(), "Migration Waves Tab was not opened");
         
        }
    }

}
