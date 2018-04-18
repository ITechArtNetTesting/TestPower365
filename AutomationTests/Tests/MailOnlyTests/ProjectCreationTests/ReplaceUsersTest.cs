using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.ProjectCreationTests
{
	[TestClass]
	public class ReplaceUsersTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}

        [TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_ReplaceUsersTest()
		{
		    string login = configurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = configurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = configurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string fileName = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='file1']/..//filename");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().GetUsersCount();
		        User.AtProjectOverviewForm().OpenProjectDetails();
		        ReplaceFile(fileName);
		        User.AtProjectOverviewForm().AssertNewUsersAreReplaced();
		        User.AtProjectOverviewForm().GetUsersCount();
            }
		    catch (Exception e)
		    {
		        LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
		}
	}
}