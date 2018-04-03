using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.PowerShellTests
{
	[TestClass]
	public class DeleteFoldersAndItemsTest : LoginAndConfigureTest
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
		public void Automation_MO_PS_DeleteFoldersAndItemsTest()
		{
            string stopFolder = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//stopfolder");
		    string sourceLogin = RunConfigurator.GetTenantValue("T1->T2", "source", "psuser");
		    string sourcePassword = RunConfigurator.GetTenantValue("T1->T2", "source", "pspassword");
		    string targetLogin = RunConfigurator.GetTenantValue("T1->T2", "target", "psuser");
		    string targetPassword = RunConfigurator.GetTenantValue("T1->T2", "target", "pspassword");
		    string sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps1']/..//source");
		    string targetMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps1']/..//target");
		    string stopFile1 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='stopfile3']/..//path");
		    string stopFile2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='stopfile4']/..//path");
            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
		    string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
            try
		    {
		        bool success = true;
		        var launcher = new PsLauncher();
		        RunConfigurator.CreateFlagFolder(stopFolder);
		        using (var process = launcher.LaunchPowerShellInstance("DeleteFoldersAndItems.ps1",
		            $" -slogin {sourceLogin}" +
		            $" -spassword {sourcePassword}" +
		            $" -tlogin {targetLogin}" +
		            $" -tpassword {targetPassword}" +
		            $" -smailbox {sourceMailbox}" +
		            $" -tmailbox {targetMailbox}" +
		            $" -StopFilePath1 {stopFile1}" +
		            $" -StopFilePath2 {stopFile2}"))
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
		                    User.AtUsersForm().WaitForState(sourceMailbox, State.Synced, 60000);
		                    RunConfigurator.CreateEmptyFile(stopFile1);
		                }
		                if (line == "Powershell will pause until Migration is complete - 2")
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
		                        Browser.GetDriver().Navigate().Refresh();
		                        User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
		                        User.AtUsersForm().SelectAction(ActionType.Sync);
		                        User.AtUsersForm().Apply();
		                    }
		                    User.AtUsersForm().ConfirmSync();
		                    Browser.GetDriver().Navigate().Refresh();
		                    User.AtUsersForm().WaitForState(sourceMailbox, State.Synced, 60000);
		                    RunConfigurator.CreateEmptyFile(stopFile2);
		                }
		            }
		            process.WaitForExit();
		        }
		        Assert.IsTrue(success, "Test failed");
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}
