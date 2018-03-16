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
        public void Automation_IN_PS_CutoverTest()
        {
            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string sourceMailbox13 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//source");
            string sourceMailbox14 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry14']/..//source");
            string sourceMailbox15 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry15']/..//source");
            string targetLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "user");
            string targetPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "password");
            string sourceLogin = RunConfigurator.GetTenantValue("T5->T6", "source", "user");
            string sourcePassword = RunConfigurator.GetTenantValue("T5->T6", "source", "password");
            string targetMailbox13 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//target");
            string targetMailbox13Smtp = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//targetsmtp");
            string targetMailbox13X500 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//targetx500");
            string targetOnPremLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "aduser");
            string targetOnPremPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "adpassword");
            string targetOnPremUri = RunConfigurator.GetTenantValue("T5->T6", "target", "uri");

            try
            {
                LoginAndSelectRole(userName, password, client);
                SelectProject(project);
                User.AtProjectOverviewForm().OpenUsersList();
                //NOTE: Sync entry13
                User.AtUsersForm().PerformSearch(sourceMailbox13);
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox13);
                User.AtUsersForm().SelectAction(ActionType.Sync);
                try
                {
                    User.AtUsersForm().Apply();
                }
                catch (Exception)
                {
                    Log.Info("Apply button is not enabled");
                    Browser.GetDriver().Navigate().Refresh();
                    User.AtUsersForm().SelectEntryBylocator(sourceMailbox13);
                    User.AtUsersForm().SelectAction(ActionType.Sync);
                    User.AtUsersForm().Apply();
                }
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().WaitForState(sourceMailbox13, State.Syncing, 30000);
                //NOTE: Sync entry14
                User.AtUsersForm().PerformSearch(sourceMailbox14);
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox14);
                User.AtUsersForm().SelectAction(ActionType.Sync);
                try
                {
                    User.AtUsersForm().Apply();
                }
                catch (Exception)
                {
                    Log.Info("Apply button is not enabled");
                    Browser.GetDriver().Navigate().Refresh();
                    User.AtUsersForm().SelectEntryBylocator(sourceMailbox14);
                    User.AtUsersForm().SelectAction(ActionType.Sync);
                    User.AtUsersForm().Apply();
                }
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().WaitForState(sourceMailbox14, State.Syncing, 30000);
                //NOTE: Sync entry15
                User.AtUsersForm().PerformSearch(sourceMailbox15);
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox15);
                User.AtUsersForm().SelectAction(ActionType.Sync);
                try
                {
                    User.AtUsersForm().Apply();
                }
                catch (Exception)
                {
                    Log.Info("Apply button is not enabled");
                    Browser.GetDriver().Navigate().Refresh();
                    User.AtUsersForm().SelectEntryBylocator(sourceMailbox15);
                    User.AtUsersForm().SelectAction(ActionType.Sync);
                    User.AtUsersForm().Apply();
                }
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().WaitForState(sourceMailbox15, State.Syncing, 30000);

                //NOTE: Wait for entry13 is synced
                User.AtUsersForm().PerformSearch(sourceMailbox13);
                User.AtUsersForm().WaitForState(sourceMailbox13, State.Synced, 60000);
                //NOTE: Wait for entry14 is synced
                User.AtUsersForm().PerformSearch(sourceMailbox14);
                User.AtUsersForm().WaitForState(sourceMailbox14, State.Synced, 60000);
                //NOTE: Wait for entry15 is synced
                User.AtUsersForm().PerformSearch(sourceMailbox15);
                User.AtUsersForm().WaitForState(sourceMailbox15, State.Synced, 60000);
                //NOTE: Cutover entry15
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox15);
                User.AtUsersForm().SelectAction(ActionType.Cutover);
                try
                {
                    User.AtUsersForm().Apply();
                }
                catch (Exception)
                {
                    Log.Info("Apply button is not enabled");
                    Browser.GetDriver().Navigate().Refresh();
                    User.AtUsersForm().SelectEntryBylocator(sourceMailbox15);
                    User.AtUsersForm().SelectAction(ActionType.Cutover);
                    User.AtUsersForm().Apply();
                }
                User.AtUsersForm().ConfirmCutover();
                User.AtUsersForm().WaitForState(sourceMailbox15, State.Finalizing, 30000);
                //NOTE: Cutover entry14
                User.AtUsersForm().PerformSearch(sourceMailbox14);
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox14);
                User.AtUsersForm().SelectAction(ActionType.Cutover);
                try
                {
                    User.AtUsersForm().Apply();
                }
                catch (Exception)
                {
                    Log.Info("Apply button is not enabled");
                    Browser.GetDriver().Navigate().Refresh();
                    User.AtUsersForm().SelectEntryBylocator(sourceMailbox14);
                    User.AtUsersForm().SelectAction(ActionType.Cutover);
                    User.AtUsersForm().Apply();
                }
                User.AtUsersForm().ConfirmCutover();
                User.AtUsersForm().WaitForState(sourceMailbox14, State.Finalizing, 30000);
                //NOTE: Cutover entry13
                User.AtUsersForm().PerformSearch(sourceMailbox13);
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox13);
                User.AtUsersForm().SelectAction(ActionType.Cutover);
                try
                {
                    User.AtUsersForm().Apply();
                }
                catch (Exception)
                {
                    Log.Info("Apply button is not enabled");
                    Browser.GetDriver().Navigate().Refresh();
                    User.AtUsersForm().SelectEntryBylocator(sourceMailbox13);
                    User.AtUsersForm().SelectAction(ActionType.Cutover);
                    User.AtUsersForm().Apply();
                }
                User.AtUsersForm().ConfirmCutover();
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox13);
                User.AtUsersForm().WaitForSyncJobAppear(2);
                User.AtUsersForm().CloseUserDetails();

                //NOTE: Wait till entry13 cutover completes
                User.AtUsersForm().WaitForState(sourceMailbox13, State.Complete, 60000);
                //NOTE: Wait till entry14 cutover completes
                User.AtUsersForm().PerformSearch(sourceMailbox14);
                User.AtUsersForm().WaitForState(sourceMailbox14, State.Complete, 60000);
                //NOTE: Wait till entry15 cutover completes
                User.AtUsersForm().PerformSearch(sourceMailbox15);
                User.AtUsersForm().WaitForState(sourceMailbox15, State.Complete, 60000);
                Thread.Sleep(2700000);
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
                        Log.Info(line);
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

                Assert.IsTrue(tc32208, "Test failed");
                Assert.IsTrue(tc28545, "Test failed");
                Assert.IsTrue(tc27857, "Test failed");
                Assert.IsTrue(tc27856, "Test failed");
                Assert.IsTrue(tc27859, "Test failed");
                Assert.IsTrue(tc32394, "Test failed");
                Assert.IsTrue(tc32203, "Test failed");
            }
            catch (Exception)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
        }
    }
}
