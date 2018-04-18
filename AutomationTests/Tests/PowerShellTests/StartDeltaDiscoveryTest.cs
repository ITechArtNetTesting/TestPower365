using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.PowerShellTests
{
	[TestClass]
	public class StartDeltaDiscoveryTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

		[TestMethod]
		public void PS_MD_StartDeltaDiscoveryTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
		    string sourceTenant = RunConfigurator.GetTenantValue("T1->T2", "source", "name");
            string sourceAdminLogin = RunConfigurator.GetTenantValue("T1->T2", "source", "user");
		    string sourceAdminPassword = RunConfigurator.GetTenantValue("T1->T2", "source", "password");
		    string sourceMailbox1 =RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry1']/..//source");

		    try
		    {
		        using (var process = new PsLauncher().LaunchPowerShellInstance("DeltaDiscoveryModify.ps1",
		            $" -slogin {sourceAdminLogin}" +
		            $" -spassword {sourceAdminPassword}" +
		            $" -mailbox {sourceMailbox1}",
		            "x64"))
		        {
		            while (!process.StandardOutput.EndOfStream)
		            {
		                var line = process.StandardOutput.ReadLine();
		                Log.Info(line);
		            }
		            process.WaitForExit();
		            LoginAndSelectRole(login, password, client);
		            SelectProject(projectName);
		            try
		            {
		                User.AtProjectOverviewForm().EditTenants();
		            }
		            catch (Exception)
		            {
		                User.AtProjectOverviewForm().EditTenants();
		            }
		            User.AtTenantsConfigurationForm().OpenDiscoveryTab();
		            User.AtTenantsConfigurationForm().RunDiscovery(sourceTenant);
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