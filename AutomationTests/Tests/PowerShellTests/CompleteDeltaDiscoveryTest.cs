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
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
		    string sourceTenant = RunConfigurator.GetTenantValue("T1->T2", "source", "name");
		    string sourceMailbox1 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry1']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().EditTenants();
		        User.AtTenantsConfigurationForm().OpenDiscoveryTab();
		        User.AtTenantsConfigurationForm().DownloadLogs(sourceTenant);
		        RunConfigurator.CheckDiscoveryFileIsDownloaded();
		        Assert.IsTrue(RunConfigurator.AssertLineExistsInCsv(Store.TenantLog, $"Updated mailbox properties for {sourceMailbox1}"), "Line does not exist in log");
            }
		    catch (Exception e)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}
