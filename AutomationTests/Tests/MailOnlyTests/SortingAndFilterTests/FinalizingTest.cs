using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.SortingAndFilterTests
{
	[TestClass]
	public class FinalizingTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
			[TestCategory("MailOnly")]
		public void Automation_MO_FinalizingTest()
		{
		    string login = configurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = configurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = configurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox5 = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry7']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox5);
		        User.AtUsersForm().SelectAction(ActionType.Sync);
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().ConfirmSync();
		        User.AtUsersForm().WaitForState(sourceMailbox5, State.Syncing, 100000,1);
		        User.AtUsersForm().WaitForState(sourceMailbox5, State.Synced, 600000,1);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox5);
		        User.AtUsersForm().SelectAction(ActionType.Cutover);
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().ConfirmCutover();
		        User.AtUsersForm().WaitForState(sourceMailbox5, State.Complete, 300000,1);
		        User.AtUsersForm().OpenProjectOverview();
		        User.AtProjectOverviewForm().AssertFinalizingCount(2);
		        User.AtProjectOverviewForm().OpenFinalizingUsers();
		        User.AtUsersForm().VerifyLineisExist(sourceMailbox5);
		        User.AtUsersForm().VerifyLinesCount(2);
            }
		    catch (Exception)
		    {
		        LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
		}
	}
}