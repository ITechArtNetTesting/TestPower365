using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.SortingAndFilterTests
{
	[TestClass]
	public class FilterTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_FilterTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox6 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry6']/..//source");
		    string groupMailbox6 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry6']/..//group");
		    string profileMailbox6 =RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry6']/..//profile");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SyncUserByLocator(sourceMailbox6);
		        User.AtUsersForm().ConfirmSync();
		        User.AtUsersForm().AssertUserHaveSyncingState(sourceMailbox6);
		        User.AtUsersForm().SwitchFilter(FilterState.Open);
		        User.AtUsersForm().SetMatched();
		        User.AtUsersForm().SwitchFilter(FilterState.Closed);
		        Thread.Sleep(5000);
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().AssertStateIsFilteredFor("Matched");
		        User.AtUsersForm().SwitchFilter(FilterState.Open);
		        User.AtUsersForm().SetMatched();
		        Thread.Sleep(5000);
		        User.AtUsersForm().CheckFilterGroup(groupMailbox6);
		        User.AtUsersForm().SwitchFilter(FilterState.Closed);
		        Thread.Sleep(5000);
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().AssertGroupIsFilteredFor(groupMailbox6);
		        User.AtUsersForm().SwitchFilter(FilterState.Open);
		        User.AtUsersForm().UncheckFilterGroup(groupMailbox6);
		        Thread.Sleep(5000);
		        User.AtUsersForm().CheckFilterGroup(profileMailbox6);
		        User.AtUsersForm().SwitchFilter(FilterState.Closed);
		        Thread.Sleep(5000);
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().AssertProfileIsFilteredFor(profileMailbox6);
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}