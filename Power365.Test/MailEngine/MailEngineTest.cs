using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;


namespace BinaryTree.Power365.Test.MailEngine
{
    [TestClass]
    public class MailEngineTest : TestBase
    {
        private class TestResults
        {
            public string TestCase { get; private set; }
            public string Description { get; private set; }
            
            public string TestPrepareResult { get; private set; }
            public DateTime TestPrepareLastRun { get; private set; }
            public string ValidationResult { get; private set; }
            public DateTime VaildationLastRun { get; private set; }

            public string OverallResult { get; private set; }

            public TestResults(PSObject psObject)
            {
                TestCase = psObject.Properties["TestCase"].Value?.ToString();
                Description = psObject.Properties["Description"].Value?.ToString();
                TestPrepareResult = psObject.Properties["TestResult"].Value?.ToString();
                TestPrepareLastRun = psObject.Properties["TestLastRun"] != null ? (DateTime)((PSObject)psObject.Properties["TestLastRun"].Value).BaseObject : DateTime.MinValue;
                ValidationResult = psObject.Properties["ValidationResult"].Value?.ToString();
                VaildationLastRun = psObject.Properties["VaildationLastRun"] != null ? (DateTime)((PSObject)psObject.Properties["VaildationLastRun"].Value).BaseObject : DateTime.MinValue;
                OverallResult = psObject.Properties["OverAllResult"].Value?.ToString();
            }
        }

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
            : base(LogManager.GetLogger(typeof(MailEngineTest))) { }

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            var assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(MailEngineTest)).Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            string modulePath = Path.Combine(assemblyDirectory, "resources", "MailEngine\\Power365-Test.psd1");
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { modulePath });
            initial.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Bypass;
            
            _runspace = RunspaceFactory.CreateRunspace(initial);
            _runspace.Open();
        }

        [TestInitialize]
        public void Initialize()
        {
            _powerShell = PowerShell.Create();
            _powerShell.Runspace = _runspace;

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

            if(!_connectedToMailboxes)
                ConnectToMailboxes(_sourceAdminUser, _sourceAdminPassword, _targetAdminUser, _targetAdminPassword, _sourceMailbox, _targetMailbox, _url, _url);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _powerShell.Dispose();
        }
        
        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22384()
        {
            AssertSyncTestPasses("22384", PerformSync);
        }
        
        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22490()
        {
            AssertSyncTestPasses("22490", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22491()
        {
            AssertSyncTestPasses("22491", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22497()
        {
            AssertSyncTestPasses("22497", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22504()
        {
            AssertSyncTestPasses("22504", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22507()
        {
            AssertSyncTestPasses("22507", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22513()
        {
            AssertSyncTestPasses("22513", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22623()
        {
            AssertSyncTestPasses("22623", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22624()
        {
            AssertSyncTestPasses("22624", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22625()
        {
            AssertSyncTestPasses("22625", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test26765()
        {
            AssertSyncTestPasses("26765", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test39541()
        {
            AssertSyncTestPasses("39541", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test39544()
        {
            AssertSyncTestPasses("39544", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test39545()
        {
            AssertSyncTestPasses("39545", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test39550()
        {
            AssertSyncTestPasses("39550", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test39551()
        {
            AssertSyncTestPasses("39551", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test39570()
        {
            AssertSyncTestPasses("39570", PerformSyncWithRollback);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test48980()
        {
            AssertSyncTestPasses("48980", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test48983()
        {
            AssertSyncTestPasses("48983", PerformSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22500()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("22500", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22516()
        {
            var source = new KeyValuePair<string, object>("SourceAddress", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetAddress", _targetMailboxExtra1);

            AssertSyncTestPasses("22516", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test22537()
        {
            var source = new KeyValuePair<string, object>("SourceAddress", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetAddress", _targetMailboxExtra1);

            AssertSyncTestPasses("22537", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
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
        public void MailEngine_PS_Test37613()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test37613b()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613b", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        public void MailEngine_PS_Test37613c()
        {
            var source = new KeyValuePair<string, object>("SourcePermission", _sourceMailboxExtra1);
            var target = new KeyValuePair<string, object>("TargetPermission", _targetMailboxExtra1);

            AssertSyncTestPasses("37613c", PerformSync, source, target);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
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
        public void MailEngine_PS_Test48974()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertSyncTestPasses("48974", PerformSync, source1, target1, source2, target2);
        }

        private static void ConnectToMailboxes(string sourceAdminUser, string sourceAdminPassword, string targetAdminUser, string targetAdminPassword, string sourceMailbox, string targetMailbox, string sourceUrl, string targetUrl)
        {
            var sourceAdminPasswordSecure = new NetworkCredential("", sourceAdminPassword).SecurePassword;
            PSCredential sourceCredential = new PSCredential(sourceAdminUser, sourceAdminPasswordSecure);

            var targetAdminPasswordSecure = new NetworkCredential("", targetAdminPassword).SecurePassword;
            PSCredential targetCredential = new PSCredential(targetAdminUser, targetAdminPasswordSecure);

            PSCommand command = new PSCommand();
            command.AddCommand("Connect-P365Mailboxes");
            command.AddParameter("SourceMailbox", sourceMailbox);
            command.AddParameter("TargetMailbox", targetMailbox);
            command.AddParameter("SourceCredential", sourceCredential);
            command.AddParameter("TargetCredential", targetCredential);
            command.AddParameter("SourceURL", sourceUrl);
            command.AddParameter("TargetURL", targetUrl);

            _powerShell.Commands = command;

            _powerShell.Invoke();

            if (_powerShell.HadErrors)
                throw new Exception("Failed to execute Connect-P365Mailboxes");

            _connectedToMailboxes = true;
        }

        private T Invoke<T>(string cmd, Func<Collection<PSObject>, T> convertReturn, params KeyValuePair<string, Object>[] parameters)
        {
            PSCommand testCommand = new PSCommand();
            testCommand.AddCommand(cmd);

            foreach (var p in parameters)
            {
                testCommand.AddParameter(p.Key, p.Value);
            }

            _powerShell.Commands = testCommand;

            return convertReturn(_powerShell.Invoke());
        }

        private T Invoke<T, PT>(string cmd, Func<Collection<PT>, T> convertReturn, params KeyValuePair<string, object>[] parameters)
        {
            PSCommand command = new PSCommand();
            command.AddCommand(cmd);

            foreach (var p in parameters)
            {
                command.AddParameter(p.Key, p.Value);
            }

            _powerShell.Commands = command;

            return convertReturn(_powerShell.Invoke<PT>());
        }

        private void Invoke(string cmd, params KeyValuePair<string, object>[] parameters)
        {
            PSCommand command = new PSCommand();
            command.AddCommand(cmd);

            foreach (var p in parameters)
            {
                command.AddParameter(p.Key, p.Value);
            }

            _powerShell.Commands = command;

            _powerShell.Invoke();

            if (_powerShell.HadErrors)
            {
                var exceptions = _powerShell.Streams.Error.Select(e => e.Exception).ToList();
                throw new AggregateException(string.Format("Failed to run {0}", cmd), exceptions);
            }
        }

        private void PerformSync(string user)
        {
            _manageUsersPage = Automation.Common
                                        .SingIn(_username, _password)
                                        .ClientSelect(_client)
                                        .ProjectSelect(_project)
                                        .UsersEdit()
                                        .UsersPerformAction(user, ActionType.Sync)
                                        .UsersValidateState(user, StateType.Syncing)
                                        .UsersValidateState(user, StateType.Synced)
                                        .GetPage<ManageUsersPage>();
        }

        private void PerformSyncWithRollback(string user)
        {
            PerformSync(user);

            Automation.Common
                    .UsersPerformRollback(user, false)
                    .UsersValidateState(user, StateType.RollbackInProgress)
                    .UsersValidateState(user, StateType.RollbackCompleted);
        }

        private TestResults GetTestResults(string testId)
        {
            return Invoke<TestResults, Dictionary<string, PSObject>>("Get-P365TestResults", (res) =>
            {
                var firstDictionary = res.FirstOrDefault();
                var tr = firstDictionary[string.Format("Test{0}", testId)];
                return new TestResults(tr);
            });
        }

        private void AssertSyncTestPasses(string testId, Action<string> userInterfaceActions, params KeyValuePair<string, object>[] parameters)
        {
            Invoke(string.Format("Invoke-Test{0}", testId), parameters);

            var testPrepareResults = GetTestResults(testId);

            Assert.AreEqual("Succeeded", testPrepareResults.TestPrepareResult);

            userInterfaceActions(_sourceMailbox);

            Invoke(string.Format("Invoke-Validate{0}", testId));
            var validateResults = GetTestResults(testId);

            Assert.AreEqual("Succeeded", validateResults.ValidationResult);
            Assert.AreEqual("Successful", validateResults.OverallResult);
        }
    }
}
