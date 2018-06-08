using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace BinaryTree.Power365.Test.MailEngine
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MailEngineTest : MailEngineTestBase
    {
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

        public MailEngineTest()
            : base() { }

        [OneTimeSetUp]
        public void OneTimeTestSetUp()
        {
            InitialStateSetup();
        }

        [SetUp]
        public void TestSetUp()
        {
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
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22384()
        {
            AssertSyncTestPasses("22384", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22490()
        {
            AssertSyncTestPasses("22490", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22491()
        {
            AssertSyncTestPasses("22491", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22497()
        {
            AssertSyncTestPasses("22497", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22504()
        {
            AssertSyncTestPasses("22504", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22507()
        {
            AssertSyncTestPasses("22507", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22513()
        {
            AssertSyncTestPasses("22513", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22623()
        {
            AssertSyncTestPasses("22623", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22624()
        {
            AssertSyncTestPasses("22624", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test22625()
        {
            AssertSyncTestPasses("22625", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test26765()
        {
            AssertSyncTestPasses("26765", PerformSync);
        }

        [Test]
        [Category("SmokeTest")]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Rollback")]
        public void Mailbox_Test39541()
        {
            AssertSyncTestPasses("39541", PerformSyncWithRollback);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Rollback")]
        public void Mailbox_Test39544()
        {
            AssertSyncTestPasses("39544", PerformSyncWithRollback);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test39545()
        {
            AssertSyncTestPasses("39545", PerformSync);
        }

        [Test]
        [Category("SmokeTest")]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Rollback")]
        public void Mailbox_Test39550()
        {
            AssertSyncTestPasses("39550", PerformSyncWithRollback);
        }

        [Test]
        [Category("SmokeTest")]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test39551()
        {
            AssertSyncTestPasses("39551", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Rollback")]
        public void Mailbox_Test39570()
        {
            AssertSyncTestPasses("39570", PerformSyncWithRollback);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test48980()
        {
            AssertSyncTestPasses("48980", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test48983()
        {
            AssertSyncTestPasses("48983", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test22500()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("22500", PerformSync, source, target);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test22516()
        {
            var source = new KeyValuePair<string, object>("SourceAddress", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetAddress", _targetMailboxExtra1);

            AssertSyncTestPasses("22516", PerformSync, source, target);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test22537()
        {
            var source = new KeyValuePair<string, object>("SourceAddress", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetAddress", _targetMailboxExtra1);

            AssertSyncTestPasses("22537", PerformSync, source, target);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test22546()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("22546", PerformSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test22728()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("22728", PerformSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test37613()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613", PerformSync, source, target);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test37613b()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613b", PerformSync, source, target);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test37613c()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613c", PerformSync, source, target);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test37614()
        {
            var source1 = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SourcePermission2", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("TargetPermission2", _targetMailboxExtra2);

            AssertSyncTestPasses("37614", PerformSync, source1, target1, source2, target2);
        }


        [Test]
        [Category("SmokeTest")]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test39557()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("39557", PerformSync, source1, target1, source2, target2);
        }


        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Permissions")]
        public void Mailbox_Test48974()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("48974", PerformSync, source1, target1, source2, target2);
        }
        
        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test45249a()
        {
            AssertSyncTestPasses("45249a", PerformSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_Test45249b()
        {
            AssertSyncTestPasses("45249b", PerformSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void PublicFolder_Test46843()
        {
            AssertSyncTestPasses("46843", PerformSync);
        }

        private void PerformSync(string user)
        {
            Automation.Common
                .UsersPerformAction(user, ActionType.Sync)
                .UsersValidateState(user, StateType.Syncing)
                .UsersValidateState(user, StateType.Synced);
        }

        private void PerformSyncWithRollback(string user)
        {
            PerformSync(user);

            Automation.Common
                .UsersPerformRollback(user, false)
                .UsersValidateState(user, StateType.RollbackInProgress)
                .UsersValidateState(user, StateType.RollbackCompleted);
        }
        
        private void AssertSyncTestPasses(string testId, Action<string> userInterfaceActions, params KeyValuePair<string, object>[] parameters)
        {
            var manageUsersPage = Automation.Common
                .SingIn(_username, _password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(_client)
                .ProjectSelect(_project)
                .UsersEdit()
                .GetPage<ManageUsersPage>();

            var powerShell = GetPowerShellSession();

            ConnectToMailboxes(powerShell, _sourceAdminUser, _sourceAdminPassword, _targetAdminUser, _targetAdminPassword, _sourceMailbox, _targetMailbox, _url, _url);

            AssertTestPasses(powerShell, testId, _sourceMailbox, userInterfaceActions, parameters);
        }
    }
}
