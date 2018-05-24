using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;


namespace BinaryTree.Power365.Test.MailEngine
{
    [TestClass]
    public class MailEngineTest : MailEngineTestBase
    {
        private static Runspace _runspace;
        private static PowerShell _powerShell;
        
        private string _client;
        private string _username;
        private string _password;
        private string _project;

        private string _sourceAdminUser;
        private string _sourceAdminPassword;
        private string _targetAdminUser;
        private string _targetAdminPassword;

        private string _sourceMailbox;
        private string _targetMailbox;

        private string _sourceMailboxExtra1;
        private string _targetMailboxExtra1;
        private string _sourceMailboxExtra2;
        private string _targetMailboxExtra2;

        private string _url;
        
        private static bool _connectedToMailboxes;

        private ManageUsersPage _manageUsersPage;

        public MailEngineTest()
            : base() { }

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitialStateSetup();
        }

        [TestInitialize]
        public void Initialize()
        {
            _powerShell = GetPowerShellSession();

            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project1");
            var sourceTenant = Automation.Settings.GetByReference<Tenant>(project.Source);
            var targetTenant = Automation.Settings.GetByReference<Tenant>(project.Target);

            var sourcePowershellUser = sourceTenant.GetByReference<Credential>("ps1");
            var targetPowershellUser = targetTenant.GetByReference<Credential>("ps1");

            var userMigration1 = project.GetByReference<UserMigration>("entry3");
            var userMigration2 = project.GetByReference<UserMigration>("entryps1");
            var userMigration3 = project.GetByReference<UserMigration>("entryps3");

            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _project = project.Name;

            _sourceAdminUser = sourcePowershellUser.Username;
            _sourceAdminPassword = sourcePowershellUser.Password;

            _targetAdminUser = targetPowershellUser.Username;
            _targetAdminPassword = targetPowershellUser.Password;
            
            _sourceMailbox = userMigration1.Source;
            _targetMailbox = userMigration1.Target;

            _sourceMailboxExtra1 = userMigration2.Source;
            _targetMailboxExtra1 = userMigration2.Target;

            _sourceMailboxExtra2 = userMigration3.Source;
            _targetMailboxExtra2 = userMigration3.Target;
            
            _url = "https://outlook.office365.com/EWS/Exchange.asmx";

            ConnectToMailboxes(_powerShell, _sourceAdminUser, _sourceAdminPassword, _targetAdminUser, _targetAdminPassword, _sourceMailbox, _targetMailbox, _url, _url);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _powerShell.Dispose();
        }
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22384()
        {
            AssertSyncTestPasses("22384", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22490()
        {
            AssertSyncTestPasses("22490", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22491()
        {
            AssertSyncTestPasses("22491", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22497()
        {
            AssertSyncTestPasses("22497", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22504()
        {
            AssertSyncTestPasses("22504", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22507()
        {
            AssertSyncTestPasses("22507", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22513()
        {
            AssertSyncTestPasses("22513", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22623()
        {
            AssertSyncTestPasses("22623", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22624()
        {
            AssertSyncTestPasses("22624", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test22625()
        {
            AssertSyncTestPasses("22625", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test26765()
        {
            AssertSyncTestPasses("26765", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void MailEngine_PS_Test39541()
        {
            AssertSyncTestPasses("39541", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void MailEngine_PS_Test39544()
        {
            AssertSyncTestPasses("39544", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test39545()
        {
            AssertSyncTestPasses("39545", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void MailEngine_PS_Test39550()
        {
            AssertSyncTestPasses("39550", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test39551()
        {
            AssertSyncTestPasses("39551", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void MailEngine_PS_Test39570()
        {
            AssertSyncTestPasses("39570", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test48980()
        {
            AssertSyncTestPasses("48980", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test48983()
        {
            AssertSyncTestPasses("48983", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test22500()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("22500", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test22516()
        {
            var source = new KeyValuePair<string, object>("SourceAddress", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetAddress", _targetMailboxExtra1);

            AssertSyncTestPasses("22516", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test22537()
        {
            var source = new KeyValuePair<string, object>("SourceAddress", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetAddress", _targetMailboxExtra1);

            AssertSyncTestPasses("22537", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test22546()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("22546", PerformSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test22728()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("22728", PerformSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test37613()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test37613b()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613b", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test37613c()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613c", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test37614()
        {
            var source1 = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SourcePermission2", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("TargetPermission2", _targetMailboxExtra2);

            AssertSyncTestPasses("37614", PerformSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void MailEngine_PS_Test48974()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("48974", PerformSync, source1, target1, source2, target2);
        }

        //New
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test45249a()
        {
            AssertSyncTestPasses("45249a", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void MailEngine_PS_Test45249b()
        {
            AssertSyncTestPasses("45249b", PerformSync);
        }

        private void PerformSync(string user, ManageUsersPage manageUsersPage)
        {
            Automation.Common
                .UsersPerformAction(user, ActionType.Sync)
                .UsersValidateState(user, StateType.Syncing)
                .UsersValidateState(user, StateType.Synced);
        }

        private void PerformSyncWithRollback(string user, ManageUsersPage manageUsersPage)
        {
            PerformSync(user, manageUsersPage);

            Automation.Common
                    .UsersPerformRollback(user, false)
                    .UsersValidateState(user, StateType.RollbackInProgress)
                    .UsersValidateState(user, StateType.RollbackCompleted);
        }
        
        private void AssertSyncTestPasses(string testId, Action<string, ManageUsersPage> userInterfaceActions, params KeyValuePair<string, object>[] parameters)
        {
            var manageUsersPage = Automation.Common
                .SingIn(_username, _password)
                .ClientSelect(_client)
                .ProjectSelect(_project)
                .UsersEdit()
                .GetPage<ManageUsersPage>();

            AssertTestPasses(_powerShell, testId, _sourceMailbox, manageUsersPage, userInterfaceActions, parameters);
        }
    }
}
