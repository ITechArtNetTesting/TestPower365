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
		    string login = configurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = configurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = configurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string sourceMailbox1 = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry1']/..//source");
		    string sourceMailbox2 = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry2']/..//source");
		    string groupMailbox3 = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry3']/..//group");
		    string sourceMailbox3 = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry3']/..//source");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox1);
		        User.AtUsersForm().ExportUsers();
                configurator.CheckUsersExportFileIsDownloaded();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox1);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox2);
		        User.AtUsersForm().ExportUsers();
                configurator.CheckUsersExportFileIsDownloaded();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox2);
		        User.AtUsersForm().OpenGroupFilter();
		        User.AtUsersForm().SelectFilterGroup(groupMailbox3);
		      
		        User.AtUsersForm().CloseModalWindow();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox3);
		        User.AtUsersForm().ExportUsers();
                configurator.CheckUsersExportFileIsDownloaded();
		        User.AtUsersForm().PerformSearch(sourceMailbox1);
		        User.AtUsersForm().VerifyLinesCount(1);
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox1);
		        User.AtUsersForm().ExportUsers();
                configurator.CheckUsersExportFileIsDownloaded();
            }
		    catch (Exception e)
		    {
		        LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw e;
            }
		}
	}
}