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
    public class TC25829_Verify_Sync_Action_Button_is_avaliable_in_the_User_Detail_Screen : LoginAndConfigureTest
    {
        int SelectedUser = -1;
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void VerifySyncActionButtonIsAvaliableInTheUserDetailScreen()
        {
            LoginAndSelectRole(RunConfigurator.GetUserLogin("client1"),
                              RunConfigurator.GetPassword("client1"),
                              RunConfigurator.GetClient("client2"));
            SelectProject(RunConfigurator.GetProjectName("client2", "project1"));
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().SelectFirstNotSyncedFreeUser(ref SelectedUser);
            User.AtUsersForm().OpenDetailsOfSelectedUser(SelectedUser);
            User.AtUsersForm().AssertDetailsSyncButtonIsEnabled();
            User.AtUsersForm().ClickSyncOnDetailsPopup();
            User.AtUsersForm().Confirm();
            User.AtUsersForm().VerifyStateIS("Syncing");
            User.AtUsersForm().WaitForJobIsCreated();
            User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
            User.AtUsersForm().AssertDetailsSyncButtonIsDisabled();
        }
    }
}
