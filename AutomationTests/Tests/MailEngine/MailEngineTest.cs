using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using Product.Tests.MailEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;


namespace Product.Tests.PowerShellTests
{
    [TestClass]
    public class MailEngineTest : MailEngineTestBase
    {
        private enum TestType
        {
            Sync,
            Rollback
        }

        private PowerShell _powerShell;
        
        private string _client;
        private string _username;
        private string _password;
        private string _project;

        private string _sourceAdminUser;
        private string _sourceAdminPassword;
        private string _targetAdminUser;
        private string _targetAdminPassword;

        private string _sourcePublicFolder;

        private string _sourceMailbox;
        private string _targetMailbox;

        private string _sourceMailboxExtra1;
        private string _targetMailboxExtra1;
        private string _sourceMailboxExtra2;
        private string _targetMailboxExtra2;

        private string _url;
        
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
            InitialStateSetup();
        }

        [TestInitialize]
        public void TestInit()
        {
            _powerShell = GetPowerShellSession();

            _client = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//name");
            _username = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//user");
            _password = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//password");
            _project = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//metaname[text()='project1']/..//name");

            var tenants = "T1->T2";

            _sourceAdminUser = RunConfigurator.GetTenantValue(tenants, "source", "psuser");
            _sourceAdminPassword = RunConfigurator.GetTenantValue(tenants, "source", "pspassword");

            _targetAdminUser = RunConfigurator.GetTenantValue(tenants, "target", "psuser");
            _targetAdminPassword = RunConfigurator.GetTenantValue(tenants, "target", "pspassword");

            _sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry3']/..//source");
            _targetMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry3']/..//target");

            _sourceMailboxExtra1 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps1']/..//source");
            _targetMailboxExtra1 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps1']/..//target");

            _sourceMailboxExtra2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps3']/..//source");
            _targetMailboxExtra2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps3']/..//target");

            _sourcePublicFolder = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry16']/..//source");

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
        public void Mailbox_PS_Test22384()
        {
            AssertSyncTestPasses("22384", PerformSync);
        }
        
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22490()
        {
            AssertSyncTestPasses("22490", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22491()
        {
            AssertSyncTestPasses("22491", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22497()
        {
            AssertSyncTestPasses("22497", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22504()
        {
            AssertSyncTestPasses("22504", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22507()
        {
            AssertSyncTestPasses("22507", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22513()
        {
            AssertSyncTestPasses("22513", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22623()
        {
            AssertSyncTestPasses("22623", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22624()
        {
            AssertSyncTestPasses("22624", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test22625()
        {
            AssertSyncTestPasses("22625", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test26765()
        {
            AssertSyncTestPasses("26765", PerformSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void Mailbox_PS_Test34507()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("34507", PerformSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void Mailbox_PS_Test39541()
        {
            AssertSyncTestPasses("39541", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void Mailbox_PS_Test39544()
        {
            AssertSyncTestPasses("39544", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test39545()
        {
            AssertSyncTestPasses("39545", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void Mailbox_PS_Test39550()
        {
            AssertSyncTestPasses("39550", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test39551()
        {
            AssertSyncTestPasses("39551", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void Mailbox_PS_Test39557()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("39557", PerformSyncWithRollback, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Rollback")]
        public void Mailbox_PS_Test39570()
        {
            AssertSyncTestPasses("39570", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test48980()
        {
            AssertSyncTestPasses("48980", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test48983()
        {
            AssertSyncTestPasses("48983", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void Mailbox_PS_Test22500()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("22500", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void Mailbox_PS_Test22516()
        {
            var source = new KeyValuePair<string, object>("SourceAddress", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetAddress", _targetMailboxExtra1);

            AssertSyncTestPasses("22516", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void Mailbox_PS_Test22537()
        {
            var source = new KeyValuePair<string, object>("SourceAddress", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetAddress", _targetMailboxExtra1);

            AssertSyncTestPasses("22537", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void Mailbox_PS_Test22546()
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
        public void Mailbox_PS_Test22728()
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
        public void Mailbox_PS_Test37613()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void Mailbox_PS_Test37613b()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613b", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void Mailbox_PS_Test37613c()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613c", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Permissions")]
        public void Mailbox_PS_Test37614()
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
        public void Mailbox_PS_Test48974()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("48974", PerformSync, source1, target1, source2, target2);
        }
        
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test45249a()
        {
            AssertSyncTestPasses("45249a", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test45249b()
        {
            AssertSyncTestPasses("45249b", PerformSync);
        }
        
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("Mailbox")]
        [TestCategory("Sync")]
        public void Mailbox_PS_Test46843()
        {
            AssertSyncTestPasses("46843", PerformSync);
        }


        private void PerformSync(string user)
        {
            LoginAndSelectRole(_username, _password, _client);
            SelectProject(_project);
            User.AtProjectOverviewForm().OpenUsersList();

            User.AtUsersForm().PerfomActionForUser(user, ActionType.Sync);
            User.AtUsersForm().ConfirmSync();
            User.AtUsersForm().AssertUserHaveSyncingState(user);

            User.AtUsersForm().WaitForState(user, State.Synced, 1200000, 60);

        }
        
        private void PerformSyncWithRollback(string user)
        {
            PerformSync(user);

            User.AtUsersForm().PerfomActionForUser(user, ActionType.Rollback);
            //Do not reset permissions it will remove access for the admin user.
            User.AtUsersForm().ConfirmRollback(false);
            User.AtUsersForm().AssertUserHasState(user, "Rollback In Progress");

            User.AtUsersForm().WaitForState(user, State.RollbackCompleted, 2400000, 600);
        }
        
        private void AssertSyncTestPasses(string testId, Action<string> userInterfaceActions, params KeyValuePair<string, object>[] parameters)
        {
            AssertTestPasses(_powerShell, testId, _sourceMailbox, userInterfaceActions, parameters);
        }
    }
}
