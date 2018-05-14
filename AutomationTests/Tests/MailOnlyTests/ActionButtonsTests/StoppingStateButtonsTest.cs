using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.ActionButtonsTests
{
	[TestClass]
	public class StoppingStateButtonsTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
        [TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_StoppingStateButtonsTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry2']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
		        User.AtUsersForm().SelectAction(ActionType.Sync);
		        User.AtUsersForm().AssertApplyIsEnabled();
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().ConfirmSync();
		        User.AtUsersForm().WaitForState(sourceMailbox, State.Syncing, 10000);
		        User.AtUsersForm().SelectAction(ActionType.Sync);
		        User.AtUsersForm().AssertApplyIsDisabled();
		        User.AtUsersForm().SelectAction(ActionType.Stop);
		        User.AtUsersForm().AssertApplyIsDisabled();
		        User.AtUsersForm().SelectAction(ActionType.Complete);
		        User.AtUsersForm().AssertApplyIsDisabled();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
		        User.AtUsersForm().SelectAction(ActionType.Stop);
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().ConfirmStop();
		        User.AtUsersForm().WaitForState(sourceMailbox, State.Stopping, 5000);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
		        User.AtUsersForm().SelectAction(ActionType.Sync);
		        User.AtUsersForm().AssertApplyIsDisabled();
		        User.AtUsersForm().SelectAction(ActionType.Stop);
		        User.AtUsersForm().AssertApplyIsDisabled();
		        User.AtUsersForm().SelectAction(ActionType.Complete);
		        User.AtUsersForm().AssertApplyIsDisabled();
            }
		    catch (Exception e)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw e;
		    }
		}
	}
}