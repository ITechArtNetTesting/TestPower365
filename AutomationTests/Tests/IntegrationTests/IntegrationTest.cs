using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

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

        public void PerformAction(string mailbox, ActionType action)
        {
            if (action == ActionType.Rollback)
                throw new NotImplementedException("Rollback requires special confirmation");

            User.AtUsersForm().PerformSearch(mailbox);
            User.AtUsersForm().SelectEntryBylocator(mailbox);
            User.AtUsersForm().SelectAction(action);
            User.AtUsersForm().Apply();
            User.AtUsersForm().ConfirmAction();
        }

        public void PerformActionAndWaitForState(string mailbox, ActionType action, State state, int timeout = 5000, int pollIntervalSec = 0)
        {
            PerformAction(mailbox, action);
            WaitForState(mailbox, state, timeout, pollIntervalSec);
        }

        public void WaitForState(string mailbox, State state, int timeout = 5000, int pollIntervalSec = 0)
        {
            User.AtUsersForm().PerformSearch(mailbox);
            User.AtUsersForm().WaitForState(mailbox, state, timeout, pollIntervalSec);
        }

		[TestMethod] 
		public void Automation_IN_PS_PrepareTest()
		{
		    string targetEntry = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//target");
            string userName = configurator.GetValueByXpath("//metaname[text()='client2']/..//user");
		    string password = configurator.GetValueByXpath("//metaname[text()='client2']/..//password");
		    string client = configurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string sourceMailbox1 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//source");
            string sourceMailbox2 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry2']/..//source");
            string sourceMailbox3 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry3']/..//source");
            string sourceMailbox6 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry6']/..//source");
            string sourceMailbox8 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry8']/..//source");
            string sourceMailbox9 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry9']/..//source");
            string sourceMailbox11 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry11']/..//source");
            string sourceMailbox11Upn = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry11']/..//upn");
            string sourceMailbox12 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry12']/..//source");
            string sourceMailbox13 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//source");
            string sourceMailbox14 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry14']/..//source");
            string sourceMailbox15 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry15']/..//source");
		    string targetLogin = configurator.GetTenantValue("T5->T6", "target", "user");
		    string targetPassword = configurator.GetTenantValue("T5->T6", "target", "password");
            string sourceMailbox1Smtp = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//smtp");
		    string targetOnPremLogin = configurator.GetTenantValue("T5->T6", "target", "aduser");
		    string targetOnPremPassword = configurator.GetTenantValue("T5->T6", "target", "adpassword");

            try
            {
                LoginAndSelectRole(userName, password, client);
                SelectProject(project);
                User.AtProjectOverviewForm().OpenUsersList();

                PerformActionAndWaitForState(sourceMailbox1, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox2, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox3, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox6, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox8, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox9, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox11Upn, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox12, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox13, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox14, ActionType.Prepare, State.Preparing, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox15, ActionType.Prepare, State.Preparing, 20000, 5);

                WaitForState(sourceMailbox1, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox2, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox3, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox6, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox8, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox9, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox11Upn, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox12, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox13, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox14, State.Prepared, 1200000, 15);
                WaitForState(sourceMailbox15, State.Prepared, 1200000, 15);

                PerformActionAndWaitForState(sourceMailbox1, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox2, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox3, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox6, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox8, ActionType.Sync, State.Provisioning, 20000, 5);//This requires azure ad sync
                PerformActionAndWaitForState(sourceMailbox9, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox11Upn, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox12, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox13, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox14, ActionType.Sync, State.Provisioning, 20000, 5);
                PerformActionAndWaitForState(sourceMailbox15, ActionType.Sync, State.Provisioning, 20000, 5);

                WaitForState(sourceMailbox1, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox2, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox3, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox6, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox8, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox9, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox11Upn, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox12, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox13, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox14, State.Synced, 1200000, 60);
                WaitForState(sourceMailbox15, State.Synced, 1200000, 60);


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
            catch (Exception)
            {
                LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
		}
	}
}
