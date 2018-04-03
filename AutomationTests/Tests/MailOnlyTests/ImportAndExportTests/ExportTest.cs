using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.ImportAndExportTests
{
	[TestClass]
	public class ExportTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
        [TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_ExportTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox1 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry1']/..//source");
		    string sourceMailbox2 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry2']/..//source");
		    string groupMailbox3 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry3']/..//group");
		    string sourceMailbox3 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry3']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox1);
		        User.AtUsersForm().ExportUsers();
		        RunConfigurator.CheckUsersExportFileIsDownloaded();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox1);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox2);
		        User.AtUsersForm().ExportUsers();
		        RunConfigurator.CheckUsersExportFileIsDownloaded();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox2);
		        User.AtUsersForm().OpenGroupFilter();
		        User.AtUsersForm().SelectFilterGroup(groupMailbox3);
		        Thread.Sleep(3000);
		        User.AtUsersForm().CloseModalWindow();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox3);
		        User.AtUsersForm().ExportUsers();
		        RunConfigurator.CheckUsersExportFileIsDownloaded();
		        User.AtUsersForm().PerformSearch(sourceMailbox1);
		        User.AtUsersForm().VerifyLinesCount(1);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox1);
		        User.AtUsersForm().ExportUsers();
		        RunConfigurator.CheckUsersExportFileIsDownloaded();
            }
		    catch (Exception e)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
		}
	}
}