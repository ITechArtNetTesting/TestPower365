using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.IntegrationTests.GroupsTests
{
    [TestClass]
    public class TC31796_Verify_group_sync_UI_will_show_accurate_job_status_throughout_group_sync : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Integration")]
        public void VerifyGroupSyncUIWillShowAccurateJobStatusThroughoutGroupSync()
        {
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string group = RunConfigurator.GetValueByXpath("//client[child::metaname='client2']//project[child::metaname='project2']//disrtibutiongroup[child::metaname='group1']/name");

            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenTotalGroups();
                User.AtGroupsMigrationForm().SearchGroup(group);
                User.AtGroupsMigrationForm().PerfomActionForGroup(group, ActionType.Sync);
                User.AtGroupsMigrationForm().ConfirmSync();
                User.AtGroupsMigrationForm().AssertUserHaveSyncingState(group);
                User.AtGroupsMigrationForm().OpenDetailsByLocator(group);
                User.AtGroupsMigrationForm().VerifyStateIS("Syncing");
                User.AtGroupsMigrationForm().WaitForJobIsCreated();
                User.AtGroupsMigrationForm().AssertDetailsStopButtonIsEnabled();
                User.AtGroupsMigrationForm().WaitForSyncedState();
                User.AtGroupsMigrationForm().AssertDetailsSyncButtonIsEnabled();
                User.AtGroupsMigrationForm().CloseUserDetails();
            }
            catch (Exception e)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
        }
    }
}
