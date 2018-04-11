using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.PowerShellTests
{
	[TestClass]
	public class CompareTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
		[TestMethod]
		[TestCategory("Powershell")]
		public void Automation_PS_MO_CompareTest()
		{
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
		        User.AtUsersForm().WaitForState(sourceMailbox, State.Synced, 
		            60000);
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
		    catch (Exception e)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}