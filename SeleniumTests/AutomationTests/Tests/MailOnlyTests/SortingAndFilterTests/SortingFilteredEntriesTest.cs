using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.SortingAndFilterTests
{
	[TestClass]
	public class SortingFilteredEntriesTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_SortingFilteredEntriesTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox7 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry7']/..//source");
		    string groupMailbox7 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry7']/..//group");
		    string profileMailbox7 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry7']/..//profile");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SwitchFilter(FilterState.Open);
		        User.AtUsersForm().SelectFilterGroup(groupMailbox7);
		        Thread.Sleep(5000);
		        User.AtUsersForm().SelectFilterGroup(profileMailbox7);
		        User.AtUsersForm().SwitchFilter(FilterState.Closed);
		        Thread.Sleep(5000);
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortSource();
		        Thread.Sleep(5000);
		        User.AtUsersForm().AssertSourceSorted();
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortTarget();
		        Thread.Sleep(5000);
		        User.AtUsersForm().AssertTargetSorted();
		        User.AtUsersForm().SyncUserByLocator(sourceMailbox7);
		        User.AtUsersForm().ConfirmSync();
		        User.AtUsersForm().AssertUserHaveSyncingState(sourceMailbox7);
		        Thread.Sleep(5000);
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortStatus();
		        Thread.Sleep(5000);
		        User.AtUsersForm().AssertStatusSorted();
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}