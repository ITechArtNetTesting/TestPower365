using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.LandingPageTests
{
	[TestClass]
	public class ContentExistsTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}

        [TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_ContentExistsTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().AssertAllContentBlocksArePresent();
		        User.AtProjectOverviewForm().AssertConnectionsStatusesExist(1);
            }
		    catch (Exception e)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
		}
	}
}