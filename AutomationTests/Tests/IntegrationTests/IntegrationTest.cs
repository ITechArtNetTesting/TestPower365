using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System.Threading;

namespace Product.Tests.IntegrationTests
{
	[TestClass]
	public class IntegrationTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        
		[TestMethod] 
		public void Automation_IN_PS_PrepareTest()
		{
		    string targetEntry = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//target");
            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string sourceMailbox1 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//source");
            string sourceMailbox2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry2']/..//source");
            string sourceMailbox3 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry3']/..//source");
            string sourceMailbox6 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry6']/..//source");
            string sourceMailbox8 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry8']/..//source");
            string sourceMailbox9 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry9']/..//source");
            string sourceMailbox11 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry11']/..//source");
            string sourceMailbox11Upn = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry11']/..//upn");
            string sourceMailbox12 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry12']/..//source");
            string sourceMailbox13 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//source");
            string sourceMailbox14 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry14']/..//source");
            string sourceMailbox15 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry15']/..//source");
		    string targetLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "user");
		    string targetPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "password");
            string sourceMailbox1Smtp = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//smtp");
		    string targetOnPremLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "aduser");
		    string targetOnPremPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "adpassword");

            try
            {
                LoginAndSelectRole(userName, password, client);
                SelectProject(project);
                User.AtProjectOverviewForm().OpenUsersList();

                PerformActionAndWaitForState(sourceMailbox1, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox2, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox3, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox6, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox8, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox9, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox11Upn, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox12, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox13, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox14, ActionType.Prepare, State.Preparing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox15, ActionType.Prepare, State.Preparing, 60000, 10);

                WaitForState(sourceMailbox1, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox2, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox3, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox6, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox8, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox9, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox11Upn, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox12, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox13, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox14, State.Prepared, 2400000, 30);
                WaitForState(sourceMailbox15, State.Prepared, 2400000, 30);

                PerformActionAndWaitForState(sourceMailbox1, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox2, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox3, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox6, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox8, ActionType.Sync, State.Provisioning, 60000, 10);//This requires azure ad sync
                PerformActionAndWaitForState(sourceMailbox9, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox11Upn, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox12, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox13, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox14, ActionType.Sync, State.Provisioning, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox15, ActionType.Sync, State.Provisioning, 60000, 10);

                //@@@ These are broken 
                WaitForAnyState(sourceMailbox1, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox2, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox3, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox6, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox8, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox9, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox11Upn, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox12, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox13, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox14, new[] { State.Synced, State.SyncError }, 2600000, 60);
                WaitForAnyState(sourceMailbox15, new[] { State.Synced, State.SyncError }, 2600000, 60);

                Thread.Sleep(60 * 60 * 1000);//Sleep for an hour because the cloud is slow and early evaluations cause problems.

                bool tc32195Group = false;
                bool tc32188 = false;
                bool tc32180Group = false;
                bool tc32181Group = false;
                bool tc32196 = false;
                bool tc28066Group = false;
                bool tc32390 = false;
                bool tc39398 = false;
                bool tc32179 = false;
                bool tc28078 = false;
                bool tc32623 = false;
                bool tc32624 = false;
                using (
                    var process = new PsLauncher().LaunchPowerShellInstance("IntegrationUsers-Prepare.ps1",
                        $" -slogin {targetLogin}" +
                        $" -spassword {targetPassword}" +
                        $" -mailbox {targetEntry.Substring(0, targetEntry.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -smtp {sourceMailbox1Smtp}" +
                        $" -remoteshare {sourceMailbox3.Substring(0, sourceMailbox3.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -remoteroom {sourceMailbox2.Substring(0, sourceMailbox2.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -mailboxremote2 {sourceMailbox6.Substring(0, sourceMailbox6.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -mailboxremote2contact {sourceMailbox6}" +
                        $" -mailboxremote4 {sourceMailbox8.Substring(0, sourceMailbox8.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -remoteequip1 {sourceMailbox9.Substring(0, sourceMailbox9.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -sam1 {sourceMailbox11.Substring(0, sourceMailbox11.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -sam1upn {sourceMailbox11Upn}" +
                        $" -sam2 {sourceMailbox12.Substring(0, sourceMailbox12.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -adlogin {targetOnPremLogin}" +
                        $" -adpassword {targetOnPremPassword}",
                        "x64"))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Log.Info(line);
                        if (line.Contains("TC32195, 32189, 32395 Passed"))
                        {
                            tc32195Group = true;
                        }
                        if (line.Contains("TC32188 Passed"))
                        {
                            tc32188 = true;
                        }
                        if (line.Contains("TC32180, 32396 Passed"))
                        {
                            tc32180Group = true;
                        }
                        if (line.Contains("TC32181, 32396 Passed"))
                        {
                            tc32181Group = true;
                        }
                        if (line.Contains("TC32196 Passed"))
                        {
                            tc32196 = true;
                        }
                        if (line.Contains("TC28066, 39398 Passed"))
                        {
                            tc28066Group = true;
                        }
                        if (line.Contains("TC32390 Passed"))
                        {
                            tc32390 = true;
                        }
                        if (line.Contains("TC39398 Passed"))
                        {
                            tc39398 = true;
                        }
                        if (line.Contains("TC32179 Passed"))
                        {
                            tc32179 = true;
                        }
                        if (line.Contains("TC28078 Passed"))
                        {
                            tc28078 = true;
                        }
                        if (line.Contains("TC32623 Passed"))
                        {
                            tc32623 = true;
                        }
                        if (line.Contains("TC32624 Passed"))
                        {
                            tc32624 = true;
                        }
                    }
                    process.WaitForExit(600000);
                }
                Assert.IsTrue(tc32195Group, "TC32195 group validation failed");
                Assert.IsTrue(tc32188, "TC32188 validation failed");
                Assert.IsTrue(tc32180Group, "TC32180 group validation failed");
                Assert.IsTrue(tc32181Group, "TC32181 group validation failed");
                Assert.IsTrue(tc32196, "TC32196 validation failed");
                Assert.IsTrue(tc28066Group, "TC28066 group validation failed");
                Assert.IsTrue(tc32390, "TC32390 validation failed");
                Assert.IsTrue(tc39398, "TC39398 validation failed");
                Assert.IsTrue(tc32179, "TC32179 validation failed");
                Assert.IsTrue(tc28078, "TC28078 validation failed");
                Assert.IsTrue(tc32623, "TC32623 validation failed");
                Assert.IsTrue(tc32624, "TC32624 validation failed");
            }
            catch (Exception e)
            {
                Log.Error("PrepareTest Failed", e);
                LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}
