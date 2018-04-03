using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.SortingAndFilterTests
{
	[TestClass]
	public class PartSearchTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_PartSearchTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox2 =RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry2']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().PerformSearch(sourceMailbox2.Substring(0, sourceMailbox2.IndexOf("@", StringComparison.Ordinal)));
		        User.AtUsersForm().VerifyLinesCount(1);
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}