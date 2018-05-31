using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using log4net;
using System.Runtime.InteropServices;
using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using IO = System.IO;

namespace BinaryTree.Power365.Test.MailEngine
{
    [TestClass]
    public class AddTest : PowershellTestBase
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

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

        private string _stopFile;

        private bool _is32Bit;

        public AddTest() 
            : base() { }

        [TestInitialize]
        public void Initialize()
        {
            var settings = Automation.Settings;

            _is32Bit = settings.Bitness == "x86";

            var client = settings.GetByReference<Client>("client2");
            _clientName = client.Name;

            var project = client.GetByReference<Project>("project1");
            _projectName = project.Name;

            var sourceTenant = settings.GetByReference<Tenant>(project.Source);
            var targetTenant = settings.GetByReference<Tenant>(project.Target);

            var sourceCredential = sourceTenant.GetByReference<Credential>("psuser");
            var targetCredential = targetTenant.GetByReference<Credential>("psuser");

            _sourceAdminUser = sourceCredential.Username;
            _sourceAdminPassword = sourceCredential.Password;

            _targetAdminUser = targetCredential.Username;
            _targetAdminPassword = targetCredential.Password;

            var userMigration1 = project.GetByReference<UserMigration>("entryps1");

            _sourceMailbox = userMigration1.Source;
            _targetMailbox = userMigration1.Target;

            _signInUser = client.Administrator.Username;
            _signInPassword = client.Administrator.Password;

            _stopFile = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetRandomFileName());
        }


        [TestMethod]
        [TestCategory("Powershell")]
        public void Automation_PS_MO_AddTest()
        {
            RunScript("AddTest.ps1", $" -slogin {_sourceAdminUser}" +
                                    $" -spassword {_sourceAdminPassword}" +
                                    $" -tlogin {_targetAdminUser}" +
                                    $" -tpassword {_targetAdminPassword}" +
                                    $" -smailbox {_sourceMailbox}" +
                                    $" -tmailbox {_targetMailbox}" +
                                    $" -StopFilePath1 {_stopFile}",
                                    _is32Bit);
        }

        protected override void ScriptOutputHandler(string line, bool isError = false)
        {
            if (line.Contains("failed"))
                throw new Exception(string.Format("Script run failed: '{0}'", line));

            if (line == "Powershell will pause until Migration is complete - 1")
            {
                Automation.Common
                        .SingIn(_signInUser, _signInPassword)
                        .MigrateAndIntegrateSelect()
                        .ClientSelect(_clientName)
                        .ProjectSelect(_projectName)
                        .UsersEdit()
                        .UsersPerformAction(_sourceMailbox, ActionType.Sync)
                        .UsersValidateState(_sourceMailbox, StateType.Syncing)
                        .UsersValidateState(_sourceMailbox, StateType.Synced);

                IO.File.Create(_stopFile).Dispose();
            }
        }
    }
}
