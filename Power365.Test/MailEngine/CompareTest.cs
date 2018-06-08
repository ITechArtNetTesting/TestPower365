using BinaryTree.Power365.AutomationFramework;
using NUnit.Framework;
using IO = System.IO;
using BinaryTree.Power365.AutomationFramework.Enums;

namespace BinaryTree.Power365.Test.MailEngine
{
    [TestFixture]
    public class CompareTest : PowerShellTestBase
    {
        private string _clientName;
        private string _projectName;

        private string _signInUser;
        private string _signInPassword;

        private string _sourceAdminUser;
        private string _sourceAdminPassword;

        private string _targetAdminUser;
        private string _targetAdminPassword;

        private string _sourceMailbox;
        private string _targetMailbox;
        
        private bool _is32Bit;

        bool _isFirstTestSuccess = false;
        bool _isSecondTestSuccess = false;
        
        [SetUp]
        public void TestSetUp()
        {
            var settings = Automation.Settings;

            _is32Bit = settings.Bitness == "x86";

            var client = settings.GetByReference<Client>("client2");
            _clientName = client.Name;

            var project = client.GetByReference<Project>("project1");
            _projectName = project.Name;

            var sourceTenant = settings.GetByReference<Tenant>(project.Source);
            var targetTenant = settings.GetByReference<Tenant>(project.Target);

            var sourceCredential = sourceTenant.GetByReference<Credential>("ps2");
            var targetCredential = targetTenant.GetByReference<Credential>("ps2");

            _sourceAdminUser = sourceCredential.Username;
            _sourceAdminPassword = sourceCredential.Password;

            _targetAdminUser = targetCredential.Username;
            _targetAdminPassword = targetCredential.Password;

            var userMigration1 = project.GetByReference<UserMigration>("entryps4");

            _sourceMailbox = userMigration1.Source;
            _targetMailbox = userMigration1.Target;

            _signInUser = client.Administrator.Username;
            _signInPassword = client.Administrator.Password;
        }

        [Test]
        [Category("MailOnly")]
        [Category("MailEngine")]
        [Category("Mailbox")]
        [Category("Sync")]
        public void Mailbox_CompareTest()
        {
            Automation.Common
                    .SingIn(_signInUser, _signInPassword)
                    .ClientSelect(_clientName)
                    .ProjectSelect(_projectName)
                    .UsersEdit()
                    .UsersPerformAction(_sourceMailbox, ActionType.Sync)
                    .UsersValidateState(_sourceMailbox, StateType.Syncing)
                    .UsersValidateState(_sourceMailbox, StateType.Synced);

            Automation.ResetBrowser();

            RunScript("Resources/PowerShell/MailEngine.CompareTest.ps1",
                $" -slogin {_sourceAdminUser}" +
                $" -spassword {_sourceAdminPassword}" +
                $" -tlogin {_targetAdminUser}" +
                $" -tpassword {_targetAdminPassword}" +
                $" -smailbox {_sourceMailbox}" +
                $" -tmailbox {_targetMailbox}",
                _is32Bit);

            Assert.IsTrue(_isFirstTestSuccess, "Folder existance Test failed");
            Assert.IsTrue(_isSecondTestSuccess, "Source Target Item existance failed");
        }

        protected override void ScriptOutputHandler(string line, bool isError = false)
        {
            if (line.Contains("Folder existance Check succeeded"))
                _isFirstTestSuccess = true;
            if (line.Contains("Source Target Item existance Check succeeded"))
                _isSecondTestSuccess = true;
        }
    }
}
