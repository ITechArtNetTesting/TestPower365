using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.MailEngine
{
    [TestClass]
    public class CompareTest : PowershellTestBase
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

        private string _stopFile;

        private bool _is32Bit;

        [TestMethod]
        [TestCategory("Powershell")]
        public void Automation_PS_MO_CompareTest()
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

            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
            string sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps4']/..//source");
            string sourceLogin = RunConfigurator.GetTenantValue("T1->T2", "source", "psuser2");
            string sourcePassword = RunConfigurator.GetTenantValue("T1->T2", "source", "pspassword2");
            string targetLogin = RunConfigurator.GetTenantValue("T1->T2", "target", "psuser2");
            string targetPassword = RunConfigurator.GetTenantValue("T1->T2", "target", "pspassword2");
            string targetMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps4']/..//target");

            try
            {
                var IsFirstTestSuccess = false;
                var IsSecondTestSuccess = false;
                var IsExpectedFailed = false;
                LoginAndSelectRole(userName, password, client);
                SelectProject(project);
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                User.AtUsersForm().SelectAction(ActionType.Sync);
                try
                {
                    User.AtUsersForm().Apply();
                }
                catch (Exception)
                {
                    Log.Info("Apply button is disabled");
                    Browser.GetDriver().Navigate().Refresh();
                    User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                    User.AtUsersForm().SelectAction(ActionType.Sync);
                    User.AtUsersForm().Apply();
                }
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().WaitForState(sourceMailbox, State.Syncing, 10000);
                User.AtUsersForm().WaitForState(sourceMailbox, State.Synced, 600000, 30);
                using (var process = new PsLauncher().LaunchPowerShellInstance("Compare.ps1",
                    $" -slogin {sourceLogin}" +
                    $" -spassword {sourcePassword}" +
                    $" -tlogin {targetLogin}" +
                    $" -tpassword {targetPassword}" +
                    $" -smailbox {sourceMailbox}" +
                    $" -tmailbox {targetMailbox}"))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Log.Info(line);
                        if (line.Contains("failed as expected") || line.Contains("Test 6 (Calendar with multiple attachments)") || line.Contains("Test 5 (Calendar with 1 Attachment)") || line.Contains("Test Case 3 (Calendar item with HTML Body)") || line.Contains("Test 15 Contact with 2 Attachments") || line.Contains("Test 14 Contact with 1 Attachment") || line.Contains("Testt 13 Contact with HTML Body") || line.Contains("Test Case 3 (Calendar item with HTML Body)") || line.Contains("Test 49, Task with two attachements") || line.Contains("Test 48, Task with one attachment") || line.Contains("Test 47, Task with HTML In body"))
                        {

                            IsExpectedFailed = true;
                        }
                        if (line.Contains("Folder existance Check succeeded"))
                        {
                            IsFirstTestSuccess = true;
                        }
                        if (line.Contains("Source Target Item existance Check succeeded"))
                        {
                            IsSecondTestSuccess = true;
                        }
                    }
                    process.WaitForExit();
                }
                Assert.IsTrue(IsFirstTestSuccess, "Folder existance Test failed");
                if (!IsExpectedFailed)
                {
                    Assert.IsTrue(IsSecondTestSuccess, "Source Target Item existance failed");
                }
                else
                {
                    Log.Fatal("Source Target Item existance test failed as expected");
                }
            }
            catch (Exception)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
        }
    }
}
