using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Workflows;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BinaryTree.Power365.Test.CommonTests.MigrationWaves
{
    [TestClass]
    public class MigrationWave_Sync_TC30919_33610 : UITestBase
    {
        public MigrationWave_Sync_TC30919_33610()
                   : base(LogManager.GetLogger(typeof(MigrationWave_Sync_TC30919_33610))) { }

        private string _client;
        private string _username;
        private string _project;
        private string _password;
        private string _userMigration;
        private ManageUsersPage _manageUsersPage;
        private static readonly string WAVE_NAME = "TC_30919_33610";

        [TestMethod]
        [TestCategory("MailOnly")]
        public void MigrationWave_Sync_MO_30919()
        {
          RunTest("client1", "project1", "entry10",true, false);
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void MigrationWave_Sync_MD_30919_33610()
        {
            RunTest("client2", "project1", "entry6", false, false,"c7toc9smlgrp2");
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void MigrationWave_Sync_Integration_TC30919_33610()
        {
            RunTest("client2", "project2", "entry11",false, true, "P365AutoGrp1");
        }


        public void RunTest(string init_client, string init_project, string init_entry, bool isMailOnly, bool isIntegrate,  string groupName = null )
        {
            var client = Automation.Settings.GetByReference<Client>(init_client);
            var project = client.GetByReference<Project>(init_project);
            var username = client.Administrator.Username;
           
            var sourceTenant = Automation.Settings.GetByReference<Tenant>(project.Source);
            var targetTenant = Automation.Settings.GetByReference<Tenant>(project.Target);

            
            _client = client.Name;
            _username = client.Administrator.Username;         
            _project = project.Name;
            _password = client.Administrator.Password;
            _userMigration = project.GetByReference<UserMigration>(init_entry).Source;

            MigrationWave_Sync(isMailOnly, isIntegrate, groupName);
        }   

                
        public void MigrationWave_Sync( bool isMailOnly,bool isIntegrate, string groupName )
        {
            _manageUsersPage = Automation.Common
                                        .SingIn(_username, _password)
                                        .ClientSelect(_client)
                                        .ProjectSelect(_project)
                                        .UsersEdit()                                        
                                        .GetPage<ManageUsersPage>();

            _manageUsersPage.SwichToTab("Migration Waves");
         
            var editWavePage = _manageUsersPage.clickNewWaveButton();
            if (isMailOnly)
            {
                //mailOnly
                editWavePage.AddMigrationWave(WAVE_NAME);
                              
            }
            else
            {
                //Integration+MailWithDiscovery
                _manageUsersPage = editWavePage.AddMigrationWave(WAVE_NAME)
                               .SelectTenantMatchGroup()
                               .AddADGroupName(groupName);
            }

            _manageUsersPage.SwichToTab("Users");
            _manageUsersPage.Refresh();
            _manageUsersPage = Automation.Common
                            .UsersPerformAddToWave(_userMigration, WAVE_NAME)
                            .GetPage<ManageUsersPage>();
            _manageUsersPage.SwichToTab("Migration Waves");
            _manageUsersPage.Waves.ClickRowByValue(WAVE_NAME);

            //Verify
            if (isIntegrate)
            {
                Assert.IsTrue(_manageUsersPage.isActionEnabled(ActionType.Prepare), "Prepare action isn't enabled");
                Assert.IsFalse(_manageUsersPage.isActionEnabled(ActionType.Sync), "Sync action is enabled");
            }
            else {
                Assert.IsTrue(_manageUsersPage.isActionEnabled(ActionType.Sync), "Sync action isn't enabled");
            }
                       
            Assert.IsTrue(_manageUsersPage.isActionEnabled(ActionType.Archive), "Archive action isn't enabled");
            Assert.IsFalse(_manageUsersPage.isActionEnabled(ActionType.Cutover), "Cutover action is enabled");
            Assert.IsFalse(_manageUsersPage.isActionEnabled(ActionType.Stop), "Stop action is enabled");
            Assert.IsTrue(_manageUsersPage.isActionEnabled(ActionType.AddToProfile), "AddToProfile action is enabled");       

        }

    }
}
