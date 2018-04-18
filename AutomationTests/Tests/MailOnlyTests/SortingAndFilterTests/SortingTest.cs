using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.SortingAndFilterTests
{
	[TestClass]
	public class SortingTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_SortingTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string filename = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='file1']/..//filename");
		    string sourceMailbox4 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry4']/..//source");

		    try
		    {
		        LoginAndReloadFile(login, password, client, projectName, filename);
		        User.AtProjectOverviewForm().OpenUsersList();
		        Thread.Sleep(5000);
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortSource();
		        Thread.Sleep(5000);
		        User.AtUsersForm().AssertSourceSorted();
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortTarget();
		        Thread.Sleep(5000);
		        User.AtUsersForm().AssertTargetSorted();
		        User.AtUsersForm().SyncUserByLocator(sourceMailbox4);
		        User.AtUsersForm().ConfirmSync();
		        User.AtUsersForm().AssertUserHaveSyncingState(sourceMailbox4);
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortStatus();
		        Thread.Sleep(5000);
		        User.AtUsersForm().AssertStatusSorted();
		        User.AtUsersForm().StoreEntriesData();
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}