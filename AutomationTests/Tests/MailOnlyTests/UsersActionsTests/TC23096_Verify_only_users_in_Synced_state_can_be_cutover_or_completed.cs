using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailOnlyTests.UsersActionsTests
{
    [TestClass]
    public class TC23096_Verify_only_users_in_Synced_state_can_be_cutover_or_completed: LoginAndConfigureTest
    {
        int SelectedUser = -1;
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifyOnlyUsersInSyncedStateCanBeCutoverOrCompleted()
        {            
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
            string sourceMailbox3 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry1']/..//source");

            try
            {                
                LoginAndSelectRole(RunConfigurator.GetUserLogin("client1"),
                                RunConfigurator.GetPassword("client1"),
                                RunConfigurator.GetClient("client1"));
                SelectProject(RunConfigurator.GetProjectName("client1", "project1"));
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().SelectFirstNotSyncedUser(ref SelectedUser);
                User.AtUsersForm().OpenDetailsOfSelectedUser(SelectedUser);
                User.AtUsersForm().AssertCutoverCompliteDetailsIsDisabled();
                User.AtUsersForm().CloseUserDetails();
                User.AtUsersForm().SelectFirstNotSyncedUser(ref SelectedUser);
                User.AtUsersForm().SelectAction(Framework.Enums.ActionType.Cutover);
                User.AtUsersForm().CheckApplyButtonIsDisabled();
                User.AtUsersForm().SelectAction(Framework.Enums.ActionType.Complete);
                User.AtUsersForm().CheckApplyButtonIsDisabled();
                User.AtUsersForm().SelectAction(Framework.Enums.ActionType.Sync);
                User.AtUsersForm().Apply();
                User.AtUsersForm().Confirm();
                User.AtUsersForm().OpenDetailsOfSelectedUser(SelectedUser);
                User.AtUsersForm().WaitForJobIsCreated();
                User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
                User.AtUsersForm().WaitForSyncedState();
                User.AtUsersForm().AssertCutoverCompliteDetailsIsEnabled();
                User.AtUsersForm().CloseUserDetails();
                User.AtUsersForm().SelectFirstNotSyncedUser(ref SelectedUser);
                User.AtUsersForm().SelectAction(Framework.Enums.ActionType.Cutover);
                User.AtUsersForm().CheckApplyButtonIsEnabled();
                User.AtUsersForm().SelectAction(Framework.Enums.ActionType.Complete);
                User.AtUsersForm().CheckApplyButtonIsEnabled();
            }
            catch (Exception ex)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw ex;
            }
        }
    }
}
