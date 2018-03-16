using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.MigrationTests
{
	[TestClass]
	public class MailMigrationTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
        [TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_MailMigrationTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox3 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry3']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().GetUsersCount();
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SyncUserByLocator(sourceMailbox3);
		        User.AtUsersForm().ConfirmSync();
		        User.AtUsersForm().AssertUserHaveSyncingState(sourceMailbox3);
		        User.AtUsersForm().OpenDetailsByLocator(sourceMailbox3);
		        User.AtUsersForm().VerifyStateIS("Syncing");
		        User.AtUsersForm().WaitForJobIsCreated();
		        User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
		        User.AtUsersForm().WaitForSyncedState();
		        User.AtUsersForm().DownloadLogs();
		        RunConfigurator.CheckLogsFileIsDownloaded();
		        User.AtUsersForm().AssertDetailsSyncButtonIsEnabled();
		        User.AtUsersForm().CompleteSync();
		        User.AtUsersForm().ConfirmComplete();
		        User.AtUsersForm().VerifyStateIS("Complete");
		        User.AtUsersForm().CloseUserDetails();
            }
		    catch (Exception e)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
		}
	}
}