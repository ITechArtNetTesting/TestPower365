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
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox5 =RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry5']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox5);
		        User.AtUsersForm().SelectAction(ActionType.Sync);
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().ConfirmSync();
		        User.AtUsersForm().WaitForState(sourceMailbox5, State.Syncing, 10000);
		        User.AtUsersForm().WaitForState(sourceMailbox5, State.Synced, 60000);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox5);
		        User.AtUsersForm().SelectAction(ActionType.Cutover);
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().ConfirmCutover();
		        User.AtUsersForm().WaitForState(sourceMailbox5, State.Complete, 30000);
		        User.AtUsersForm().OpenProjectOverview();
		        User.AtProjectOverviewForm().AssertFinalizingCount(2);
		        User.AtProjectOverviewForm().OpenFinalizingUsers();
		        User.AtUsersForm().VerifyLineisExist(sourceMailbox5);
		        User.AtUsersForm().VerifyLinesCount(2);
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}