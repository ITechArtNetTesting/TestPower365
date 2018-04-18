using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.MigrationTests
{
    [TestClass]
    public class RollbackTest : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailOnly")]
        public void Automation_MO_RollbackTest()
        {
            string login = configurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = configurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = configurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string projectName = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
            string sourceMailbox = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps4']/..//source");
            string targetMailbox = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps4']/..//target");
            string sourceLogin = configurator.GetTenantValue("T1->T2", "source", "psuser2");
            string sourcePassword = configurator.GetTenantValue("T1->T2", "source", "pspassword2");
            string targetLogin = configurator.GetTenantValue("T1->T2", "target", "psuser2");
            string targetPassword = configurator.GetTenantValue("T1->T2", "target", "pspassword2");
            string stopFile1 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='stopfile9']/..//path");
            string stopFile2 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='stopfile7']/..//path");
            string stopFile3 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='stopfile10']/..//path");
            string stopFile4 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='stopfile8']/..//path");
            string permSourceMailbox = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry2']/..//source");
            string permTargetMailbox = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry2']/..//target");
            string stopFolder = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//stopfolder");

            try
            {
                using (
                    var sourcePreparation = new PsLauncher().LaunchPowerShellInstance("Cleanup_Rollback.ps1", $" -slogin {sourceLogin}" +
                                                                                                              $" -spassword {sourcePassword}" +
                                                                                                              $" -tlogin {targetLogin}" +
                                                                                                              $" -tpassword {targetPassword}" +
                                                                                                              $" -smailbox {sourceMailbox}" +
                                                                                                              $" -SourceMailbox {permSourceMailbox}", "x64"))
                {
                    while (!sourcePreparation.StandardOutput.EndOfStream)
                    {
                        string line = sourcePreparation.StandardOutput.ReadLine();
                        Log.Info(line);
                    }
                    sourcePreparation.WaitForExit(30000);
                }

                bool success = true;
                var launcher = new PsLauncher();
                configurator.CreateFlagFolder(stopFolder);
                using (var process = launcher.LaunchPowerShellInstance("Rollback1.ps1", $" -slogin {sourceLogin}" +
                                                                                               $" -spassword {sourcePassword}" +
                                                                                               $" -tlogin {targetLogin}" +
                                                                                               $" -tpassword {targetPassword}" +
                                                                                               $" -smailbox {sourceMailbox}" +
                                                                                               $" -tmailbox {targetMailbox}" +
                                                                                               $" -StopFilePath1 {stopFile1}" +
                                                                                               $" -SourceMailbox1 {permSourceMailbox}" +
                                                                                               $" -TargetMailbox1 {permTargetMailbox}" +
                                                                                               $" -StopFilePathRollback1 {stopFile2}" +
                                                                                               $" -StopFilePath3 {stopFile3}" +
                                                                                               $" -StopFilePathRollback4 {stopFile4}"))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        Log.Info(line);
                        if (line.Contains("failed"))
                        {
                            Log.Fatal(line);
                            success = false;
                        }
                        if (line == "Powershell will pause until Migration is complete - 1")
                        {
                            LoginAndSelectRole(login, password, client);
                            SelectProject(projectName);
                            User.AtProjectOverviewForm().OpenUsersList();
                            User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                            User.AtUsersForm().SelectAction(ActionType.Rollback);
                            User.AtUsersForm().AssertApplyIsDisabled();
                            User.AtUsersForm().SelectAction(ActionType.Sync);
                            try
                            {
                                User.AtUsersForm().Apply();
                            }
                            catch (Exception)
                            {
                                Log.Info("Apply button is disabled");
                                Driver.GetDriver(driver.GetDriverKey()).Navigate().Refresh();
                                User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                                User.AtUsersForm().SelectAction(ActionType.Sync);
                                User.AtUsersForm().Apply();
                            }
                            User.AtUsersForm().ConfirmSync();
                            User.AtUsersForm().WaitForState(sourceMailbox, State.Syncing, 10000);
                            User.AtUsersForm().WaitForState(sourceMailbox, State.Synced, 60000);
                            configurator.CreateEmptyFile(stopFile1);
                        }
                        if (line == "Powershell will pause until Migration is complete - 2")
                        {
                            User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                            User.AtUsersForm().SelectAction(ActionType.Rollback);
                            User.AtUsersForm().Apply();
                            User.AtUsersForm().SetDontResetPermissions();
                            User.AtUsersForm().SetSureCheckbox();
                            User.AtUsersForm().Rollback();
                            User.AtUsersForm().WaitForState(sourceMailbox, State.RollbackInProgress, 10000);
                            User.AtUsersForm().WaitForState(sourceMailbox, State.RollbackCompleted, 60000);
                            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox);
                            User.AtUsersForm().DownloadRollbackLogs();
                            User.AtUsersForm().CloseUserDetails();
                            configurator.CheckRollbackLogsFileIsDownloaded();
                            configurator.CreateEmptyFile(stopFile2);
                        }

                        if (line == "Powershell will pause until Migration is complete - 3")
                        {
                            User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                            User.AtUsersForm().SelectAction(ActionType.Sync);
                            try
                            {
                                User.AtUsersForm().Apply();
                            }
                            catch (Exception)
                            {
                                Log.Info("Apply button is disabled");
                                Driver.GetDriver(driver.GetDriverKey()).Navigate().Refresh();
                                User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                                User.AtUsersForm().SelectAction(ActionType.Sync);
                                User.AtUsersForm().Apply();
                            }
                            User.AtUsersForm().ConfirmSync();
                            User.AtUsersForm().WaitForState(sourceMailbox, State.Syncing, 10000);
                            User.AtUsersForm().WaitForState(sourceMailbox, State.Synced, 60000);
                            configurator.CreateEmptyFile(stopFile3);
                        }

                        if (line == "Powershell will pause until Migration is complete - 4")
                        {
                            User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                            User.AtUsersForm().SelectAction(ActionType.Rollback);
                            User.AtUsersForm().Apply();
                            User.AtUsersForm().SetResetPermissions();
                            User.AtUsersForm().SetSureCheckbox();
                            User.AtUsersForm().Rollback();
                            User.AtUsersForm().WaitForState(sourceMailbox, State.RollbackInProgress, 10000);
                            User.AtUsersForm().WaitForState(sourceMailbox, State.RollbackCompleted, 60000);
                            configurator.CreateEmptyFile(stopFile4);
                        }

                    }
                    process.WaitForExit();
                }
                Assert.IsTrue(success, "Test failed");
            }
            catch (Exception)
            {
                LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
        }
    }
}
