using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.SortingAndFilterTests
{
	[TestClass]
	public class ArchiveTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_ArchiveTest()
		{
		    string login = configurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = configurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = configurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string fileName = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='file1']/..//filename");
		    string sourceMailbox8 = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry8']/..//source");

		    try
		    {
		        LoginAndReloadFile(login, password, client, projectName, fileName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().SelectEntryBylocator(sourceMailbox8);
		        User.AtUsersForm().SelectAction(ActionType.Archive);
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().ConfirmArchive();
		        User.AtUsersForm().VerifyLineNotExist(sourceMailbox8);
            }
		    catch (Exception)
		    {
		        LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
		}
	}
}