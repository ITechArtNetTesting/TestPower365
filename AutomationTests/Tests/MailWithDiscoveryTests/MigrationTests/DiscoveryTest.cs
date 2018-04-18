using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.MailWithDiscoveryTests.MigrationTests
{
	[TestClass]
	public class DiscoveryTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("MailWithDiscovery")]
		public void Automation_MD_DiscoveryProcessTest()
		{
		    string login = configurator.GetValueByXpath("//metaname[text()='client2']/..//user");
		    string password = configurator.GetValueByXpath("//metaname[text()='client2']/..//password");
		    string client = configurator.GetValueByXpath("//metaname[text()='client2']/../name");
		    string projectName = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
		    string userAmount = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='adgroup1']/../amount");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().WaitTillDiscoveryIsComplete(10);
		        User.AtProjectOverviewForm().GetUsersCount();
		        User.AtProjectOverviewForm().AssertDiscoveredUserCount(userAmount);
		        User.AtProjectOverviewForm().AssertMigrationUserCount();
            }
		    catch (Exception)
		    {
		        LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
		}
	}
}