using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.ImportAndExportTests
{
	[TestClass]
	public class ImportTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
        [TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_ImportTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//name");
		    string fileName1 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='file1']/..//filename");
		    string fileName2 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//metaname[text()='file1']/..//filename");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().VerifyLinesCount(RunConfigurator.GetCSVlinesCount(Path.GetFullPath(RunConfigurator.ResourcesPath + fileName2)));
		        User.AtUsersForm().OpenImportDialog();
		        User.AtUsersForm().ChooseFile(fileName1);
		        User.AtUsersForm().ConfirmUploading();
		        User.AtUsersForm().AssertImportFailed();
            }
		    catch (Exception e)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
		}
	}
}