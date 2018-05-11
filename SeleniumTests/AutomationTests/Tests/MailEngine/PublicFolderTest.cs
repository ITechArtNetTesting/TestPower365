using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Tests.MailEngine
{
    [TestClass]
    public class PublicFolderTest: MailEngineTestBase
    {
        private PowerShell _powerShell;

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

        private string _sourcePublicFolder;
        private string _attachment;

        private string _url;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
            InitialStateSetup();
            CleanPublicFolders();
        }

        private static void CleanPublicFolders()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            RunConfigurator.RunPath = "resources/run.xml";
            RunConfigurator.ResourcesPath = "resources/";

            var tenants = "T5->T6";
            var sourceAdminUser = RunConfigurator.GetTenantValue(tenants, "source", "user");
            var sourceAdminPassword = RunConfigurator.GetTenantValue(tenants, "source", "password");

            var targetAdminUser = RunConfigurator.GetTenantValue(tenants, "target", "user");
            var targetAdminPassword = RunConfigurator.GetTenantValue(tenants, "target", "password");
            
            using (var cleanupScript = new PsLauncher().LaunchPowerShellInstance("PF/PFMailEngineCleanup.ps1",
                    $" -sourceLogin {sourceAdminUser}" +
                    $" -sourcePassword {sourceAdminPassword}" +
                    $" -targetLogin {targetAdminUser}" +
                    $" -targetPassword {targetAdminPassword}",
                "x64"))
            {
                Task sourceStdOut = Task.Run(() =>
                {
                    while (!cleanupScript.StandardOutput.EndOfStream)
                    {
                        var line = cleanupScript.StandardOutput.ReadLine();
                        Log.Debug(line);
                    }
                });

                Task sourceStdError = Task.Run(() =>
                {
                    while (!cleanupScript.StandardError.EndOfStream)
                    {
                        var line = cleanupScript.StandardError.ReadLine();
                        Log.Error(line);
                    }
                });

                Task.WaitAll(sourceStdOut, sourceStdError);
                cleanupScript.WaitForExit(30000);

                Thread.Sleep(2 * 60 * 1000); //Sleep for 2 minutes, reduce change that the folders are not in sync yet.
            }
        }

        [TestInitialize]
        public void TestInit()
        {
            _powerShell = GetPowerShellSession();

            _client = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//name");
            _username = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//user");
            _password = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//password");
            _project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");

            var tenants = "T5->T6";

            _sourceAdminUser = RunConfigurator.GetTenantValue(tenants, "source", "user");
            _sourceAdminPassword = RunConfigurator.GetTenantValue(tenants, "source", "password");

            _targetAdminUser = RunConfigurator.GetTenantValue(tenants, "target", "user");
            _targetAdminPassword = RunConfigurator.GetTenantValue(tenants, "target", "password");

            //_sourceAdminUser = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//source");
            //_sourceAdminPassword = "Password1";

            //_targetAdminUser = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//target");
            //_targetAdminPassword = "Password1";

            _sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//source");
            _targetMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//target");

            _sourceMailboxExtra1 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry17']/..//source");
            _targetMailboxExtra1 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry17']/..//target");

            _sourceMailboxExtra2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry18']/..//source");
            _targetMailboxExtra2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry18']/..//target");

            _sourcePublicFolder = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry16']/..//source");

            _attachment = "resources/attach.jgp";

            _url = "https://outlook.office365.com/EWS/Exchange.asmx";

            ConnectToMailboxes(_powerShell, _sourceAdminUser, _sourceAdminPassword, _targetAdminUser, _targetAdminPassword, _sourceMailbox, _targetMailbox, _url, _url);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _powerShell.Dispose();
        }


        //This test script does not test what the test case says
        //Test case is to add permission to parent folder and ensure the permission is only copied to parent folder
        //This test adds a parent folder, syncs it, then creates a child folder with no permissions, test causes the child folder to be invisible to the validation.
        //Need refactor
        [TestMethod]
        [Ignore]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test27842()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("27842", PerformPublicFolderSync, source1, target1, source2, target2);
        }
        
        
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30070()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30070", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30072()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30072", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30075()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30075", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30076()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30076", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30077()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30077", PerformPublicFolderSync, source1, target1, source2, target2);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30078()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30078", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30079()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30079", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30080()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30080", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30091()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30091", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30095()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30095", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30096()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30096", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30097()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30097", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30098()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30098", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30099()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30099", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30110()
        {
            AssertPublicFolderSyncTestPasses("30110", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30111()
        {
            AssertPublicFolderSyncTestPasses("30111", PerformPublicFolderSync);
        }

        //This test is generating a public folder smtp address tha tis being reused somewhere and causing a persistant failure.
        [Ignore]
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30112()
        {
            AssertPublicFolderSyncTestPasses("30112", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30119()
        {
            //@@@
            var source1 = new KeyValuePair<string, object>("SourceForwardingAddress", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("TargetForwardingAddress", _targetMailboxExtra1);

            AssertPublicFolderSyncTestPasses("30119", PerformPublicFolderSync, source1, target1);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30122()
        {
            AssertPublicFolderSyncTestPasses("30122", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30134()
        {
            AssertPublicFolderSyncTestPasses("30134", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30135()
        {
            AssertPublicFolderSyncTestPasses("30135", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30142()
        {
            AssertPublicFolderSyncTestPasses("30142", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30143()
        {
            AssertPublicFolderSyncTestPasses("30143", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30146a()
        {
            AssertPublicFolderSyncTestPasses("30146a", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30146b()
        {
            AssertPublicFolderSyncTestPasses("30146b", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30336()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30336", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30337()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30337", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        public void PublicFolder_PS_Test30338()
        {
            AssertPublicFolderSyncTestPasses("30338", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        public void PublicFolder_PS_Test30373()
        {
            AssertPublicFolderSyncTestPasses("30373", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test30377()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30377", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30378()
        {
            AssertPublicFolderSyncTestPasses("30378", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30379()
        {
            AssertPublicFolderSyncTestPasses("30379", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30380()
        {
            AssertPublicFolderSyncTestPasses("30380", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30381()
        {
            AssertPublicFolderSyncTestPasses("30381", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30382()
        {
            AssertPublicFolderSyncTestPasses("30382", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30383()
        {
            AssertPublicFolderSyncTestPasses("30383", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        public void PublicFolder_PS_Test30384()
        {
            AssertPublicFolderSyncTestPasses("30384", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30385()
        {
            AssertPublicFolderSyncTestPasses("30385", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30387a()
        {
            AssertPublicFolderSyncTestPasses("30387", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30387b()
        {
            AssertPublicFolderSyncTestPasses("30387b", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30391()
        {
            AssertPublicFolderSyncTestPasses("30391", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30392()
        {
            AssertPublicFolderSyncTestPasses("30392", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30393()
        {
            AssertPublicFolderSyncTestPasses("30393", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30394()
        {
            AssertPublicFolderSyncTestPasses("30394", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30469()
        {
            AssertPublicFolderSyncTestPasses("30469", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30470()
        {
            AssertPublicFolderSyncTestPasses("30470", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30471()
        {
            AssertPublicFolderSyncTestPasses("30471", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30475()
        {
            AssertPublicFolderSyncTestPasses("30475", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30477()
        {
            AssertPublicFolderSyncTestPasses("30477", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30478()
        {
            AssertPublicFolderSyncTestPasses("30478", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30479()
        {
            AssertPublicFolderSyncTestPasses("30479", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30485()
        {
            AssertPublicFolderSyncTestPasses("30485", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30488()
        {
            AssertPublicFolderSyncTestPasses("30488", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30489()
        {
            AssertPublicFolderSyncTestPasses("30489", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30490()
        {
            AssertPublicFolderSyncTestPasses("30490", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30501()
        {
            AssertPublicFolderSyncTestPasses("30501", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30503()
        {
            AssertPublicFolderSyncTestPasses("30503", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30504()
        {
            AssertPublicFolderSyncTestPasses("30504", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30511()
        {
            AssertPublicFolderSyncTestPasses("30511", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30512()
        {
            AssertPublicFolderSyncTestPasses("30512", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30513()
        {
            AssertPublicFolderSyncTestPasses("30513", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30518()
        {
            AssertPublicFolderSyncTestPasses("30518", PerformPublicFolderSync);
        }
        
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30519()
        {
            AssertPublicFolderSyncTestPasses("30519", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test30522()
        {
            AssertPublicFolderSyncTestPasses("30522", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Provisioning")]
        public void PublicFolder_PS_Test30822()
        {
            var source1 = new KeyValuePair<string, object>("SourceProxyAddress", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("TargetProxyAddress", _targetMailboxExtra1);

            AssertPublicFolderSyncTestPasses("30822", PerformPublicFolderSync, source1, target1);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test32077()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("32077", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32267()
        {
            AssertPublicFolderSyncTestPasses("32267", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32268()
        {
            AssertPublicFolderSyncTestPasses("32268", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32282()
        {
            AssertPublicFolderSyncTestPasses("32282", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32341()
        {
            AssertPublicFolderSyncTestPasses("32341", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32342()
        {
            AssertPublicFolderSyncTestPasses("32342", PerformPublicFolderSync);
        }
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32343()
        {
            AssertPublicFolderSyncTestPasses("32343", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32344()
        {
            AssertPublicFolderSyncTestPasses("32344", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32345()
        {
            AssertPublicFolderSyncTestPasses("32345", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32346()
        {
            AssertPublicFolderSyncTestPasses("32346", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32347()
        {
            AssertPublicFolderSyncTestPasses("32347", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32348()
        {
            var attachment = new KeyValuePair<string, object>("Attachment", _attachment);

            AssertPublicFolderSyncTestPasses("32348", PerformPublicFolderSync, attachment);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32349()
        {
            var attachment = new KeyValuePair<string, object>("Attachment", _attachment);

            AssertPublicFolderSyncTestPasses("32349", PerformPublicFolderSync, attachment);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32350()
        {
            AssertPublicFolderSyncTestPasses("32350", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32351()
        {
            AssertPublicFolderSyncTestPasses("32351", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32362()
        {
            AssertPublicFolderSyncTestPasses("32362", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32363()
        {
            AssertPublicFolderSyncTestPasses("32363", PerformPublicFolderSync);
        }


        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32364()
        {
            AssertPublicFolderSyncTestPasses("32364", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32366()
        {
            AssertPublicFolderSyncTestPasses("32366", PerformPublicFolderSync);
        }
        
        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32369()
        {
            AssertPublicFolderSyncTestPasses("32369", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test32370()
        {
            AssertPublicFolderSyncTestPasses("32370", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34504()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34504", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34505()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34505", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34507()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34507", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34508()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34508", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34510()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34510", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34519()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34519", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34520()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34520", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34521()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34521", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34522()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34522", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34523()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34523", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test34524()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34524", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test36315()
        {
            AssertPublicFolderSyncTestPasses("36315", PerformPublicFolderSync);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Permissions")]
        public void PublicFolder_PS_Test39557()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("39557", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [TestMethod]
        [TestCategory("MailEngine")]
        [TestCategory("PublicFolder")]
        [TestCategory("Sync")]
        public void PublicFolder_PS_Test46843()
        {
            AssertPublicFolderSyncTestPasses("46843", PerformPublicFolderSync);
        }

        private class RetryException : Exception
        {
        }


        private void PerformPublicFolderSync(string folder)
        {
            LoginAndSelectRole(_username, _password, _client);
            SelectProject(_project);
            User.AtProjectOverviewForm().OpenPublicFolders();
            User.AtPublicFolderMigrationViewForm().SyncUserByLocator(folder);
            User.AtPublicFolderMigrationViewForm().ConfirmSync();
            User.AtPublicFolderMigrationViewForm().WaitForSyncingState(folder);
            User.AtPublicFolderMigrationViewForm().WaitForAnyState(folder, new[] { Framework.Enums.State.SyncError, Framework.Enums.State.Ready }, 15 * 60 * 1000, 30);

            var state = User.AtPublicFolderMigrationViewForm().GetJobState(folder);

            if (state == State.Error)
            {
                Log.Warn("Public Folder Sync Job is in an Error state, cleaning up public folders");
                CleanPublicFolders();
                throw new RetryException();
            }
        }
        
        private void AssertPublicFolderSyncTestPasses(string testId, Action<string> userInterfaceActions, params KeyValuePair<string, object>[] parameters)
        {
            try
            {
                AssertTestPasses(_powerShell, testId, _sourcePublicFolder, userInterfaceActions, parameters);
            }
            catch (RetryException)
            {
                //Try a second time.. exception here will not be caught
                AssertTestPasses(_powerShell, testId, _sourcePublicFolder, userInterfaceActions, parameters);
            }
        }
    }
}
