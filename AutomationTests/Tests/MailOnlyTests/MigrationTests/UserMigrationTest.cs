using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.MigrationTests
{
	[TestClass]
	public class UserMigrationTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}

      [TestMethod]
		[TestCategory("MailOnly")]
		public void Automation_MO_UserMigrationTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName1 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
		    string projectName2 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//name");
		    string sourceDomain1 = RunConfigurator.GetTenantValue("T1->T2", "source", "domain");
		    string targetDomain1 = RunConfigurator.GetTenantValue("T1->T2", "target", "domain");
		    string groupMailbox1 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry1']/..//group");
		    string filename1 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='file1']/..//filename");
		    string sourceDomain2 = RunConfigurator.GetTenantValue("T3->T4", "source", "domain");
		    string targetDomain2 = RunConfigurator.GetTenantValue("T3->T4", "target", "domain");
		    string groupMailbox5 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//group");
		    string filename2 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//metaname[text()='file1']/..//filename");

		    try
		    {
		        LoginAndSelectRole(login, password, client);
		        SelectProject(projectName1);
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().VerifyLinesCountAndProperties(sourceDomain1, targetDomain1, RunConfigurator.GetCSVlinesCount(Path.GetFullPath(RunConfigurator.ResourcesPath + filename1)));
		        User.AtUsersForm().SelectAllLines();
		        User.AtUsersForm().SelectAction(ActionType.AddToWave);
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().SelectMigrationGroup(groupMailbox1);
		        User.AtUsersForm().AddToWave();
		        User.AtUsersForm().VerifyLinesCountAndProperties(groupMailbox1, RunConfigurator.GetCSVlinesCount(Path.GetFullPath(RunConfigurator.ResourcesPath + filename1)));
		        User.AtUsersForm().SelectAllLines();
		        User.AtUsersForm().OpenTenantRestructuring();
		        SelectProject(projectName2);
		        User.AtProjectOverviewForm().GetUsersCount();
		        User.AtProjectOverviewForm().AssertReadyUserEqualToAll();
		        User.AtProjectOverviewForm().OpenUsersList();
		        User.AtUsersForm().VerifyLinesCountAndProperties(sourceDomain2, targetDomain2, RunConfigurator.GetCSVlinesCount(Path.GetFullPath(RunConfigurator.ResourcesPath + filename2)));
		        User.AtUsersForm().SelectAllLines();
		        User.AtUsersForm().SelectAction(ActionType.AddToWave);
		        User.AtUsersForm().Apply();
		        User.AtUsersForm().SelectMigrationGroup(groupMailbox5);
		        User.AtUsersForm().AddToWave();
		        User.AtUsersForm().VerifyLinesCountAndProperties(groupMailbox5, RunConfigurator.GetCSVlinesCount(Path.GetFullPath(RunConfigurator.ResourcesPath + filename2)));
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}