using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.IntegrationTests
{
    [TestClass]
    public class CutoverTest : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("Integration_test")]
        public void Automation_IN_PS_CutoverTest()
        {
            string userName = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string project = RunConfigurator.GetProjectName("client2", "project2");
            string sourceMailbox13 = RunConfigurator.GetSourceMailbox("client2", "project2", "entry13");
            string sourceMailbox14 = RunConfigurator.GetSourceMailbox("client2", "project2", "entry14");
            string sourceMailbox15 = RunConfigurator.GetSourceMailbox("client2", "project2", "entry15");
            string targetLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "user");
            string targetPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "password");
            string sourceLogin = RunConfigurator.GetTenantValue("T5->T6", "source", "user");
            string sourcePassword = RunConfigurator.GetTenantValue("T5->T6", "source", "password");
            string targetMailbox13 = RunConfigurator.GetTargetMailbox("client2", "project2", "entry13");
            string targetMailbox13Smtp = RunConfigurator.GetTargetSmtpMailbox("client2", "project2", "entry13");
            string targetMailbox13X500 = RunConfigurator.GetTargetX500Mailbox("client2", "project2", "entry13");
            string targetOnPremLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "aduser");
            string targetOnPremPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "adpassword");
            string targetOnPremUri = RunConfigurator.GetTenantValue("T5->T6", "target", "uri");

            try
            {
                LoginAndSelectRole(userName, password, client);
                SelectProject(project);
                User.AtProjectOverviewForm().OpenUsersList();

                //PerformActionAndWaitForState(sourceMailbox13, ActionType.Sync, State.Syncing, 60000, 10);
                //PerformActionAndWaitForState(sourceMailbox14, ActionType.Sync, State.Syncing, 60000, 10);
                //PerformActionAndWaitForState(sourceMailbox15, ActionType.Sync, State.Syncing, 60000, 10);

                //WaitForState(sourceMailbox13, State.Synced, 2400000, 60);
                //WaitForState(sourceMailbox14, State.Synced, 2400000, 60);
                //WaitForState(sourceMailbox15, State.Synced, 2400000, 60);

                PerformActionAndWaitForState(sourceMailbox13, ActionType.Cutover, State.Finalizing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox14, ActionType.Cutover, State.Finalizing, 60000, 10);
                PerformActionAndWaitForState(sourceMailbox15, ActionType.Cutover, State.Finalizing, 60000, 10);

                User.AtUsersForm().PerformSearch(sourceMailbox13);
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox13);
                User.AtUsersForm().WaitForSyncJobAppear(3);
                User.AtUsersForm().CloseUserDetails();

                WaitForState(sourceMailbox13, State.Complete, 3600000, 60);
                WaitForState(sourceMailbox14, State.Complete, 3600000, 60);
                WaitForState(sourceMailbox15, State.Complete, 3600000, 60);

                Thread.Sleep(45 * 60 * 1000);
                //NOTE: Run PS script
                bool tc32208 = false;
                bool tc28545 = false;
                bool tc27857 = false;
                bool tc27856 = false;
                bool tc27859 = false;
                bool tc32394 = false;
                bool tc32203 = false;

                using (
                    var validationProcess = new PsLauncher().LaunchPowerShellInstance("Integration-Cutover.ps1",
                        $" -tlogin {targetLogin}" +
                        $" -tpassword {targetPassword}" +
                        $" -slogin {sourceLogin}" +
                        $" -spassword {sourcePassword}" +
                        $" -mailboxremote5 {sourceMailbox13.Substring(0, sourceMailbox13.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -mailboxremote5SMTP {targetMailbox13Smtp}" +
                        $" -Equipremote2 {sourceMailbox14.Substring(0, sourceMailbox14.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -Roomremote2 {sourceMailbox15.Substring(0, sourceMailbox15.LastIndexOf("@", StringComparison.Ordinal))}" +
                        $" -mailboxremote5X500 \"{targetMailbox13X500}\"" +
                        $" -adlogin {targetOnPremLogin}" +
                        $" -adpassword {targetOnPremPassword}" +
                        $" -uri {targetOnPremUri}"+
                        $" -ForwardingSmtpAddressSource {targetMailbox13Smtp}",
                        "x64"))
                {
                    while (!validationProcess.StandardOutput.EndOfStream)
                    {
                        var line = validationProcess.StandardOutput.ReadLine();
                        Log.Debug(line);
                        if (line.Contains("TC32208 Passed"))
                        {
                            tc32208 = true;
                        }
                        if (line.Contains("TC28545 Passed"))
                        {
                            tc28545 = true;
                        }
                        if (line.Contains("TC27857 Passed"))
                        {
                            tc27857 = true;
                        }
                        if (line.Contains("RoomMailbox Cutover Successful"))
                        {
                            tc27856 = true;
                        }
                        if (line.Contains("TC27859 and 32404 Passed"))
                        {
                            tc27859 = true;
                        }
                        if (line.Contains("TC32394 and 32404 Passed"))
                        {
                            tc32394 = true;
                        }
                        if (line.Contains("TC32203 Passed"))
                        {
                            tc32203 = true;
                        }
                    }
                    validationProcess.WaitForExit(600000);
                }

                var success = tc32208 & tc28545 & tc27857 & tc27856 & tc27859 & tc32394 & tc32203;

                Assert.IsTrue(success, "Not all tests were successful.");
            }
            catch (Exception e)
            {
                Log.Error("Failed to run CutoverTest.", e);
                LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
        }
    }
}
