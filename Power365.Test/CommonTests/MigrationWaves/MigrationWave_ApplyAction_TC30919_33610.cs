using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.MigrationWaves
{
      [TestFixture]    
    public class MigrationWave_ApplyAction_TC30919_33610 : TestBase
    {
        public MigrationWave_ApplyAction_TC30919_33610()
                   : base() { }
                    
        private static readonly string WAVE_NAME = "TC_30919_33610_4";


        [Category("MailOnly")]
        [Category("UI")]
        [Test]
        public void MigrationWave_Sync_MO_30919()
        {
          RunTest("client1", "project1", "entry10",true, false);
        }


        [Category("MailWithDiscovery")]
        [Category("UI")]
        [Test]
        public void MigrationWave_Sync_MD_30919_33610()
        {
            RunTest("client2", "project1", "entry6", false, false,"c7toc9smlgrp2");
        }


        [Category("Integration")]
        [Category("UI")]
        [Test]       
        public void MigrationWave_Sync_Integration_TC30919_33610()
        {
            RunTest("client2", "project2", "entry11",false, true, "P365AutoGrp1");
        }


       private void RunTest(string init_client, string init_project, string init_entry, bool isMailOnly, bool isIntegrate,  string groupName = null )
        {
            var client = Automation.Settings.GetByReference<Client>(init_client);
            var project = client.GetByReference<Project>(init_project);
            var username = client.Administrator.Username;
           
            var sourceTenant = Automation.Settings.GetByReference<Tenant>(project.Source);
            var targetTenant = Automation.Settings.GetByReference<Tenant>(project.Target);

            
           string _client = client.Name;
           string _username = client.Administrator.Username;         
           string _project = project.Name;
           string _password = client.Administrator.Password;
           string _userMigration = project.GetByReference<UserMigration>(init_entry).Source;

            MigrationWave_Sync(_client, _password , _project,  _username, _userMigration ,  isMailOnly, isIntegrate, groupName);
        }   

                
        private void MigrationWave_Sync( string _client, string _password , string _project ,  string _username, string _userMigration, bool isMailOnly,bool isIntegrate, string groupName )
        {
             ManageUsersPage _manageUsersPage;
           _manageUsersPage = Automation.Common
                                        .SingIn(_username, _password)
                                        .MigrateAndIntegrateSelect()
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
            _manageUsersPage.WavesTable.ClickRowByValue(WAVE_NAME);

            //Verify
            if (isIntegrate)
            {
                Assert.IsTrue(_manageUsersPage.IsActionEnabled(ActionType.Prepare), "Prepare action isn't enabled");
                Assert.IsFalse(_manageUsersPage.IsActionEnabled(ActionType.Sync), "Sync action is enabled");
            }
            else {
                Assert.IsTrue(_manageUsersPage.IsActionEnabled(ActionType.Sync), "Sync action isn't enabled");
            }

            Assert.IsTrue(_manageUsersPage.IsActionEnabled(ActionType.Archive), "Archive action isn't enabled");
            Assert.IsFalse(_manageUsersPage.IsActionEnabled(ActionType.Cutover), "Cutover action is enabled");
            Assert.IsFalse(_manageUsersPage.IsActionEnabled(ActionType.Stop), "Stop action is enabled");
            Assert.IsTrue(_manageUsersPage.IsActionEnabled(ActionType.AddToProfile), "AddToProfile action is enabled");
                    


        }

    }
}
