using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System.Collections.Generic;
using System.Text;

namespace Product.Tests.PowerShellTests
{
	[TestClass]
	public class PermissionsPrepareTest: LoginAndConfigureTest
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
		public void Automation_PS_MO_PermissionsPrepareTest()
		{           
		    string sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entryps4']/..//target");
		    string sourceLogin = RunConfigurator.GetTenantValue("T1->T2", "target", "user");
		    string sourcePassword = RunConfigurator.GetTenantValue("T1->T2", "target", "password");
            string permSourceMailbox = RunConfigurator.GetTenantValue("T1->T2", "target", "psuser2"); ;

            List<string> trustees = new List<string>();
            trustees.Add(permSourceMailbox);
            var trusteeString = string.Join(",", trustees);
            
            using (var process = new PsLauncher().LaunchPowerShellInstance("AddPermission.ps1",
                                $" -login {sourceLogin}" +
                                $" -password {sourcePassword}" +
                                $" -mailbox {sourceMailbox}" +
                                $" -trustees {trusteeString}" +
                                $" -accessRights FullAccess"
                                , "x64"))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    Log.Info(line);

                    if (line.Contains("wasn't found"))
                    {
                        Log.Fatal(line);
                        Assert.Fail(line);
                    }
                }
                process.WaitForExit();
            }
        }
	}
}