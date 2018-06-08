using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Workflows;
using BinaryTree.Power365.AutomationFramework.Enums;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.MailEngine
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class PublicFolderTest: MailEngineTestBase
    {
        private class RetryException : Exception { }
        
        private string _clientName;
        private string _signInUser;
        private string _signInPassword;
        private string _projectName;

        private string _sourceTenant;
        private string _targetTenant;

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
        
        private string _attachment;

        private string _url;

        public PublicFolderTest() 
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
            var project = client.GetByReference<Project>("project2");
            var sourceTenant = Automation.Settings.GetByReference<Tenant>(project.Source);
            var targetTenant = Automation.Settings.GetByReference<Tenant>(project.Target);

            var sourcePowershellUser = sourceTenant.GetByReference<Credential>("pf1");
            var targetPowershellUser = targetTenant.GetByReference<Credential>("pf1");

            var userMigration1 = project.GetByReference<UserMigration>("pfuser1");
            var userMigration2 = project.GetByReference<UserMigration>("pfuser2");
            var userMigration3 = project.GetByReference<UserMigration>("pfuser3");

            _clientName = client.Name;
            _signInUser = client.Administrator.Username;
            _signInPassword = client.Administrator.Password;
            _projectName = project.Name;

            _sourceAdminUser = sourcePowershellUser.Username;
            _sourceAdminPassword = sourcePowershellUser.Password;

            _targetAdminUser = targetPowershellUser.Username;
            _targetAdminPassword = targetPowershellUser.Password;

            _sourceTenant = sourceTenant.Name;
            _targetTenant = targetTenant.Name;

            _sourceMailbox = userMigration1.Source;
            _targetMailbox = userMigration1.Target;

            _sourceMailboxExtra1 = userMigration2.Source;
            _targetMailboxExtra1 = userMigration2.Target;

            _sourceMailboxExtra2 = userMigration3.Source;
            _targetMailboxExtra2 = userMigration3.Target;

            _url = "https://outlook.office365.com/EWS/Exchange.asmx";
            
            _attachment = "resources/attach.jpg";
        }
        
        #region Tests
        //This test script does not test what the test case says
        //Test case is to add permission to parent folder and ensure the permission is only copied to parent folder
        //This test adds a parent folder, syncs it, then creates a child folder with no permissions, test causes the child folder to be invisible to the validation.
        //Need refactor
        //[Test]
        //[Category("MailEngine")]
        //[Category("PublicFolder")]
        //[Category("Permissions")]
        //public void PublicFolder_Test27842()
        //{
        //    var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
        //    var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
        //    var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
        //    var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

        //    AssertPublicFolderSyncTestPasses("27842", PerformPublicFolderSync, source1, target1, source2, target2);
        //}
        
        
        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30070()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30070", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30072()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30072", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30075()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30075", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30076()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30076", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30077()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30077", PerformPublicFolderSync, source1, target1, source2, target2);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30078()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30078", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30079()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30079", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30080()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30080", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30091()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30091", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30095()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30095", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30096()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30096", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30097()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30097", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30098()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30098", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30099()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30099", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30110()
        {
            AssertPublicFolderSyncTestPasses("30110", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30111()
        {
            AssertPublicFolderSyncTestPasses("30111", PerformPublicFolderSync);
        }

        ////This test is generating a public folder smtp address tha tis being reused somewhere and causing a persistant failure.
        //[Ignore]
        //[Test]
        //[Category("MailEngine")]
        //[Category("PublicFolder")]
        //[Category("Provisioning")]
        //public void PublicFolder_Test30112()
        //{
        //    AssertPublicFolderSyncTestPasses("30112", PerformPublicFolderSync);
        //}


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30119()
        {
            //@@@
            var source1 = new KeyValuePair<string, object>("SourceForwardingAddress", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("TargetForwardingAddress", _targetMailboxExtra1);

            AssertPublicFolderSyncTestPasses("30119", PerformPublicFolderSync, source1, target1);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30122()
        {
            AssertPublicFolderSyncTestPasses("30122", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30134()
        {
            AssertPublicFolderSyncTestPasses("30134", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30135()
        {
            AssertPublicFolderSyncTestPasses("30135", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30142()
        {
            AssertPublicFolderSyncTestPasses("30142", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30143()
        {
            AssertPublicFolderSyncTestPasses("30143", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30146a()
        {
            AssertPublicFolderSyncTestPasses("30146a", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30146b()
        {
            AssertPublicFolderSyncTestPasses("30146b", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30336()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30336", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30337()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30337", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30338()
        {
            AssertPublicFolderSyncTestPasses("30338", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30373()
        {
            AssertPublicFolderSyncTestPasses("30373", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test30377()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("30377", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30378()
        {
            AssertPublicFolderSyncTestPasses("30378", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30379()
        {
            AssertPublicFolderSyncTestPasses("30379", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30380()
        {
            AssertPublicFolderSyncTestPasses("30380", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30381()
        {
            AssertPublicFolderSyncTestPasses("30381", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30382()
        {
            AssertPublicFolderSyncTestPasses("30382", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30383()
        {
            AssertPublicFolderSyncTestPasses("30383", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        public void PublicFolder_Test30384()
        {
            AssertPublicFolderSyncTestPasses("30384", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30385()
        {
            AssertPublicFolderSyncTestPasses("30385", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30387a()
        {
            AssertPublicFolderSyncTestPasses("30387", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30387b()
        {
            AssertPublicFolderSyncTestPasses("30387b", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30391()
        {
            AssertPublicFolderSyncTestPasses("30391", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30392()
        {
            AssertPublicFolderSyncTestPasses("30392", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30393()
        {
            AssertPublicFolderSyncTestPasses("30393", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30394()
        {
            AssertPublicFolderSyncTestPasses("30394", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30469()
        {
            AssertPublicFolderSyncTestPasses("30469", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30470()
        {
            AssertPublicFolderSyncTestPasses("30470", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30471()
        {
            AssertPublicFolderSyncTestPasses("30471", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30475()
        {
            AssertPublicFolderSyncTestPasses("30475", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30477()
        {
            AssertPublicFolderSyncTestPasses("30477", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30478()
        {
            AssertPublicFolderSyncTestPasses("30478", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30479()
        {
            AssertPublicFolderSyncTestPasses("30479", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30485()
        {
            AssertPublicFolderSyncTestPasses("30485", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30488()
        {
            AssertPublicFolderSyncTestPasses("30488", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30489()
        {
            AssertPublicFolderSyncTestPasses("30489", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30490()
        {
            AssertPublicFolderSyncTestPasses("30490", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30501()
        {
            AssertPublicFolderSyncTestPasses("30501", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30503()
        {
            AssertPublicFolderSyncTestPasses("30503", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30504()
        {
            AssertPublicFolderSyncTestPasses("30504", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30511()
        {
            AssertPublicFolderSyncTestPasses("30511", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30512()
        {
            AssertPublicFolderSyncTestPasses("30512", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30513()
        {
            AssertPublicFolderSyncTestPasses("30513", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30518()
        {
            AssertPublicFolderSyncTestPasses("30518", PerformPublicFolderSync);
        }
        
        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30519()
        {
            AssertPublicFolderSyncTestPasses("30519", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test30522()
        {
            AssertPublicFolderSyncTestPasses("30522", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Provisioning")]
        public void PublicFolder_Test30822()
        {
            var source1 = new KeyValuePair<string, object>("SourceProxyAddress", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("TargetProxyAddress", _targetMailboxExtra1);

            AssertPublicFolderSyncTestPasses("30822", PerformPublicFolderSync, source1, target1);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test32077()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("32077", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32267()
        {
            AssertPublicFolderSyncTestPasses("32267", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32268()
        {
            AssertPublicFolderSyncTestPasses("32268", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32282()
        {
            AssertPublicFolderSyncTestPasses("32282", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32341()
        {
            AssertPublicFolderSyncTestPasses("32341", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32342()
        {
            AssertPublicFolderSyncTestPasses("32342", PerformPublicFolderSync);
        }
        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32343()
        {
            AssertPublicFolderSyncTestPasses("32343", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32344()
        {
            AssertPublicFolderSyncTestPasses("32344", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32345()
        {
            AssertPublicFolderSyncTestPasses("32345", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32346()
        {
            AssertPublicFolderSyncTestPasses("32346", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32347()
        {
            AssertPublicFolderSyncTestPasses("32347", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32348()
        {
            var attachment = new KeyValuePair<string, object>("Attachment", _attachment);

            AssertPublicFolderSyncTestPasses("32348", PerformPublicFolderSync, attachment);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32349()
        {
            var attachment = new KeyValuePair<string, object>("Attachment", _attachment);

            AssertPublicFolderSyncTestPasses("32349", PerformPublicFolderSync, attachment);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32350()
        {
            AssertPublicFolderSyncTestPasses("32350", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32351()
        {
            AssertPublicFolderSyncTestPasses("32351", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32362()
        {
            AssertPublicFolderSyncTestPasses("32362", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32363()
        {
            AssertPublicFolderSyncTestPasses("32363", PerformPublicFolderSync);
        }


        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32364()
        {
            AssertPublicFolderSyncTestPasses("32364", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32366()
        {
            AssertPublicFolderSyncTestPasses("32366", PerformPublicFolderSync);
        }
        
        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32369()
        {
            AssertPublicFolderSyncTestPasses("32369", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test32370()
        {
            AssertPublicFolderSyncTestPasses("32370", PerformPublicFolderSync);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34504()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34504", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34505()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34505", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34508()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34508", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34510()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34510", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34519()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34519", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34520()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34520", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34521()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34521", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34522()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34522", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34523()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34523", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Permissions")]
        public void PublicFolder_Test34524()
        {
            var source1 = new KeyValuePair<string, object>("FirstSourcePermission", _sourceMailboxExtra1);
            var target1 = new KeyValuePair<string, object>("FirstTargetPermission", _targetMailboxExtra1);
            var source2 = new KeyValuePair<string, object>("SecondSourcePermission", _sourceMailboxExtra2);
            var target2 = new KeyValuePair<string, object>("SecondTargetPermission", _targetMailboxExtra2);

            AssertPublicFolderSyncTestPasses("34524", PerformPublicFolderSync, source1, target1, source2, target2);
        }

        [Test]
        [Category("MailEngine")]
        [Category("PublicFolder")]
        [Category("Sync")]
        public void PublicFolder_Test36315()
        {
            AssertPublicFolderSyncTestPasses("36315", PerformPublicFolderSync);
        }


        #endregion
            
        private void PerformPublicFolderSync(string sourceFolder)
        {
            var targetFolder = "\\Automation\\Tests";
            
            var addPublicFolderWorkflow = Automation.Common
                .SingIn(_signInUser, _signInPassword)
                .MigrateAndIntegrateSelect()
                .ClientSelect(_clientName)
                .ProjectSelect(_projectName)
                .PublicFoldersEdit()
                .GetPage<ManagePublicFoldersPage>()
                .AddPublicFolderMigration();

            ManagePublicFoldersPage managePublicFoldersPage = null;
            try
            {
                managePublicFoldersPage = addPublicFolderWorkflow
                    .ChooseFolders()
                    .TenantPair(_sourceTenant, _targetTenant)
                    .PathMapping(sourceFolder, targetFolder)
                    .SyncScope(false)
                    .OnDemand()
                    .Conflicts(false)
                    .Submit();

                if (!managePublicFoldersPage.PublicFolders.PageToRow(sourceFolder))
                    throw new Exception(string.Format("Could not find Public Folder: {0}", sourceFolder));

                if (!managePublicFoldersPage.IsPublicFolderState(sourceFolder, StateType.Ready, 10 * 60, 10))
                    throw new Exception(string.Format("Public Folder: '{0}' failed to reach Ready state.", sourceFolder));

                managePublicFoldersPage.PublicFolders.ClickRowByValue(sourceFolder);
                managePublicFoldersPage.PerformAction(ActionType.Sync);
                managePublicFoldersPage.ConfirmAction();

                if (!managePublicFoldersPage.IsPublicFolderState(sourceFolder, StateType.Syncing, 10 * 60, 10))
                    throw new Exception(string.Format("Public Folder: '{0}' failed to reach Syncing state.", sourceFolder));

                if (!managePublicFoldersPage.IsPublicFolderAnyState(sourceFolder, new[] { StateType.SyncError, StateType.Ready }, 15 * 60, 30))
                    throw new Exception(string.Format("Public Folder: '{0}' failed to complete Sync."));
            }
            finally
            {
                if(managePublicFoldersPage != null)
                {
                    Logger.Debug(string.Format("Archiving migration: {0}", sourceFolder));
                    managePublicFoldersPage.PublicFolders.ClickRowByValue(sourceFolder);
                    managePublicFoldersPage.PerformAction(ActionType.Archive);
                    managePublicFoldersPage.ConfirmAction();
                }
                Automation.ResetBrowser();
            }
        }
        
        private void AssertPublicFolderSyncTestPasses(string testId, Action<string> userInterfaceActions, params KeyValuePair<string, object>[] parameters)
        {
            var folderPath = string.Format("\\Automation\\Tests\\{0}", testId);
            RunScript("Resources/PowerShell/PublicFolder.PrepareFolderPair.ps1",
                   $" -sourceLogin {_sourceAdminUser}" +
                   $" -sourcePassword {_sourceAdminPassword}" +
                   $" -targetLogin {_targetAdminUser}" +
                   $" -targetPassword {_targetAdminPassword}" +
                   $" -folderPath {folderPath}", false);
            
            var sourceFolderPath = string.Format("\\Automation\\Tests\\{0}", testId);
            var sourceFolderPathParam = new KeyValuePair<string, object>("RootPath", sourceFolderPath);

            var targetFolderPath = string.Format("\\Automation\\Tests");
            var targetFolderPathParam = new KeyValuePair<string, object>("TargetRootPath", targetFolderPath);

            //create new paramerters list to append rootFolder
            List<KeyValuePair<string, object>> paramSet = parameters != null ? parameters.ToList() : new List<KeyValuePair<string, object>>();
            paramSet.Add(sourceFolderPathParam);
            paramSet.Add(targetFolderPathParam);

            var paramArray = paramSet.ToArray();

            var powerShell = GetPowerShellSession();

            powerShell.Streams.Information.DataAdded += (s, e) =>
            {
                if (Logger.IsInfoEnabled)
                    Logger.Info(powerShell.Streams.Information[e.Index]);
            };

            powerShell.Streams.Debug.DataAdded += (s, e) =>
            {
                if (Logger.IsDebugEnabled)
                    Logger.Debug(powerShell.Streams.Debug[e.Index]);
            };

            powerShell.Streams.Progress.DataAdded += (s, e) =>
            {
                if (Logger.IsDebugEnabled)
                    Logger.Debug(powerShell.Streams.Progress[e.Index]);
            };

            powerShell.Streams.Error.DataAdded += (s, e) =>
            {
                if(Logger.IsErrorEnabled)
                    Logger.Error(powerShell.Streams.Error[e.Index]);
            };
            
            try
            {
                ConnectToMailboxes(powerShell, _sourceAdminUser, _sourceAdminPassword, _targetAdminUser, _targetAdminPassword, _sourceMailbox, _targetMailbox, _url, _url);
                AssertTestPasses(powerShell, testId, folderPath, userInterfaceActions, paramArray);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to Assert Test Passes", e);
                throw;
            }
            finally
            {
                if (powerShell != null)
                    powerShell.Dispose();
            }
        }
    }
}
