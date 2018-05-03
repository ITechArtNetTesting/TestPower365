using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailWithDiscoveryTests.ActionButtonsTests
{
    [TestClass]
    public class TC25830_Verify_Stop_Action_is_available_in_the_User_Detail_Screen : LoginAndConfigureTest
    {        
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void VerifyStopActionIsAvailableInTheUserDetailScreen()
        {
            string sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='TC25830Entry']/..//source");            
            LoginAndSelectRole(RunConfigurator.GetUserLogin("client1"),
                          RunConfigurator.GetPassword("client1"),
                          RunConfigurator.GetClient("client2"));
            SelectProject(RunConfigurator.GetProjectName("client2", "project1"));
            User.AtProjectOverviewForm().OpenUsersList();            
            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox);
            User.AtUsersForm().ClickSyncOnDetailsPopup();
            User.AtUsersForm().Confirm();
            User.AtUsersForm().VerifyStateIS("Syncing");
            User.AtUsersForm().WaitForJobIsCreated();
            User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
            User.AtUsersForm().StopSyncing();            
            User.AtUsersForm().Confirm();
            User.AtUsersForm().VerifyStateIS("Stopping");
            User.AtUsersForm().CloseUserDetails();
            User.AtUsersForm().VerifyTheMailMigrationJobHasStoped(sourceMailbox);
        }
    }
}
