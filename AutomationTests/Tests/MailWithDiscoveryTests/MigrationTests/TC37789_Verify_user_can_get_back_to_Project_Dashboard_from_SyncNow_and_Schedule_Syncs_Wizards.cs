using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;

namespace Product.Tests.MailWithDiscoveryTests.MigrationTests
{
    [TestClass]
    public class TC37789_Verify_user_can_get_back_to_Project_Dashboard_from_SyncNow_and_Schedule_Syncs_Wizards : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Migration")]
        public void VerifyUserCanGetBackToProjectDashboardFromSyncNowAndScheduleSyncsWizards()
        {
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");

            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().ClickSyncNow();
                User.AtSyncNowForm().ClickBackButton();
                User.AtProjectOverviewForm().AssertUserAtCurrentPage();
                User.AtProjectOverviewForm().ClickScheduleSync();
                User.AtSyncScheduleForm().ClickBackButton();
                User.AtProjectOverviewForm().AssertUserAtCurrentPage();
            }
            catch (Exception ex)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw ex;
            }
        }
    }
}
