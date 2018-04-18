using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.PowerShellTests
{
	[TestClass]
	public class CompleteDeltaDiscoveryTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}

		[TestMethod]
		public void PS_MD_CompleteDeltaDiscoveryTest()
		{
		    string login = configurator.GetValueByXpath("//metaname[text()='client2']/..//user");
		    string password = configurator.GetValueByXpath("//metaname[text()='client2']/..//password");
		    string client = configurator.GetValueByXpath("//metaname[text()='client2']/../name");
		    string projectName = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
		    string sourceTenant = configurator.GetTenantValue("T1->T2", "source", "name");
		    string sourceMailbox1 = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry1']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().EditTenants();
		        User.AtTenantsConfigurationForm().OpenDiscoveryTab();
		        User.AtTenantsConfigurationForm().DownloadLogs(sourceTenant);
                configurator.CheckDiscoveryFileIsDownloaded();
		        Assert.IsTrue(configurator.AssertLineExistsInCsv(store.TenantLog, $"Updated mailbox properties for {sourceMailbox1}"), "Line does not exist in log");
            }
		    catch (Exception e)
		    {
		        LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
		}
	}
}
