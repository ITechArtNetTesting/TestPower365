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
		        //NOTE: entry1 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox1);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox1);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox1);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox1, State.Preparing, 30000);

		        //NOTE: entry2 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox2);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox2);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox2);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox2, State.Preparing, 30000);
		        //NOTE: entry3 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox3);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox3);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox3);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        try
		        {
		            User.AtUsersForm().ConfirmPrepare();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply did not work");
		            User.AtUsersForm().Apply();
		            User.AtUsersForm().ConfirmPrepare();
		        }
		        User.AtUsersForm().WaitForState(sourceMailbox3, State.Preparing, 30000);
		        //NOTE: entry6 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox6);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox6);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox6);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox6, State.Preparing, 30000);
		        //NOTE: entry8 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox8);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox8);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox8);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox8, State.Preparing, 30000);
		        //NOTE: entry9 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox9);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox9);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox9);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);

		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox9, State.Preparing, 30000);
		        //NOTE: enrty11 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox11Upn);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox11Upn);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox11Upn);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox11Upn, State.Preparing, 30000);
		        //NOTE: entry12 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox12);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox12);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox12);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox12, State.Preparing, 30000);
		        //NOTE: entry13 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox13);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox13);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox13);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox13, State.Preparing, 30000);
		        //NOTE: entry14 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox14);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox14);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox14);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox14, State.Preparing, 30000);
		        //NOTE: entry15 prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox15);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox15);
		        User.AtUsersForm().SelectAction(ActionType.Prepare);
		        try
		        {
		            User.AtUsersForm().Apply();
		        }
		        catch (Exception)
		        {
		            Log.Info("Apply button is not enabled");
		            Browser.GetDriver().Navigate().Refresh();
		            User.AtUsersForm().SelectEntryBylocator(sourceMailbox15);
		            User.AtUsersForm().SelectAction(ActionType.Prepare);
		            User.AtUsersForm().Apply();
		        }
		        User.AtUsersForm().ConfirmPrepare();
		        User.AtUsersForm().WaitForState(sourceMailbox15, State.Preparing, 30000);

		        //NOTE: entry1 wait for prepared
		        User.AtUsersForm().PerformSearch(sourceMailbox1);
		        User.AtUsersForm().WaitForState(sourceMailbox1, State.Prepared, 90000);
		        //NOTE: entry2 wait for prepared
		        User.AtUsersForm().PerformSearch(sourceMailbox2);
		        User.AtUsersForm().WaitForState(sourceMailbox2, State.Prepared, 90000);
		        //NOTE: entry3 wait for prepared
		        User.AtUsersForm().PerformSearch(sourceMailbox3);
		        User.AtUsersForm().WaitForState(sourceMailbox3, State.Prepared, 90000);
		        //NOTE: entry6 wait for prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox6);
		        User.AtUsersForm().WaitForState(sourceMailbox6, State.Prepared, 90000);
		        //NOTE: entry8 wait for prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox8);
		        User.AtUsersForm().WaitForState(sourceMailbox8, State.Prepared, 90000);
		        //NOTE: entry9 wait for prepare
		        User.AtUsersForm().PerformSearch(sourceMailbox9);
		        User.AtUsersForm().WaitForState(sourceMailbox9, State.Prepared, 90000);
		        //NOTE: entry11 wait for prepared
		        User.AtUsersForm().PerformSearch(sourceMailbox11Upn);
		        User.AtUsersForm().WaitForState(sourceMailbox11Upn, State.Prepared, 90000);
		        //NOTE: entry12 wait for prepared
		        User.AtUsersForm().PerformSearch(sourceMailbox12);
		        User.AtUsersForm().WaitForState(sourceMailbox12, State.Prepared, 90000);
		        //NOTE: entry13 wait for prepared
		        User.AtUsersForm().PerformSearch(sourceMailbox13);
		        User.AtUsersForm().WaitForState(sourceMailbox13, State.Prepared, 90000);
		        //NOTE: entry14 wait for prepared
		        User.AtUsersForm().PerformSearch(sourceMailbox14);
		        User.AtUsersForm().WaitForState(sourceMailbox14, State.Prepared, 90000);
		        //NOTE: entry15 wait for prepared
		        User.AtUsersForm().PerformSearch(sourceMailbox15);
		        User.AtUsersForm().WaitForState(sourceMailbox15, State.Prepared, 90000);

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
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}
