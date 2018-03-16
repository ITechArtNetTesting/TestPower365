using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;

namespace Product.Tests.CommonTests.SetupTests
{
    [TestClass]
    public class ProfileConfigurationTest : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("Setup")]
        public void ConfigureProfileTest()
        {
            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string sourceMailbox14 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry14']/..//source");
            string sourceMailbox15 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry15']/..//source");
            LoginAndSelectRole(userName, password, client);
            SelectProject(project);
            User.AtProjectOverviewForm().OpenMainMenu();
            User.AtProjectOverviewForm().AtMainMenu().OpenProfiles();
            User.AtProfilesOverviewForm().ModifyProfile("Default Profile");
            User.AtProfileNameForm().GoNext();
            User.AtProfileOutlookConfigForm().SelectNo();
            User.AtProfileOutlookConfigForm().GoNext();
            User.AtProfileCreateUsersForm().SelectYes();
            User.AtProfileCreateUsersForm().GoNext();
            User.AtProfileCreateDistributionGroupsForm().SelectYes();
            User.AtProfileCreateDistributionGroupsForm().GoNext();
            User.AtProfileSyncDistributionGroupsForm().GoNext();
            User.AtProfileContentToMigrateForm.GoNext();
            User.AtProfileFilterMessagesForm.GoNext();
            User.AtProfileFilterCalendarForm.GoNext();
            User.AtProfileFilterTasksForm.GoNext();
            User.AtProfileFilterNoteForm.GoNext();
            User.AtProfileLargeItemsHandleForm.GoNext();
            User.AtAlmostDoneForm().GoNext();


            User.AtProfilesOverviewForm().AddProfile();
            User.AtProfileNameForm().SetName("TestProfile1");
            User.AtProfileNameForm().GoNext();
            User.AtProfileOutlookConfigForm().SelectYes();
            User.AtProfileOutlookConfigForm().GoNext();
            User.AtProfileCreateUsersForm().SelectYes();
            User.AtProfileCreateUsersForm().GoNext();
            User.AtProfileCreateDistributionGroupsForm().SelectYes();
            User.AtProfileCreateDistributionGroupsForm().GoNext();
            User.AtProfileSyncDistributionGroupsForm().GoNext();
            User.AtProfileContentToMigrateForm.SelectType(ContentType.Calendar);
            User.AtProfileContentToMigrateForm.SelectType(ContentType.Contacts);
            User.AtProfileContentToMigrateForm.SelectType(ContentType.Email);
            User.AtProfileContentToMigrateForm.SelectType(ContentType.Notes);
            User.AtProfileContentToMigrateForm.SelectType(ContentType.Tasks);
            User.AtProfileContentToMigrateForm.GoNext();
            User.AtProfileFilterMessagesForm.GoNext();
            User.AtProfileFilterCalendarForm.GoNext();
            User.AtProfileFilterTasksForm.GoNext();
            User.AtProfileFilterNoteForm.GoNext();
            User.AtProfileLargeItemsHandleForm.GoNext();
            User.AtAlmostDoneForm().GoNext();
            User.AtProfilesOverviewForm().GoToProjectOverview();
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().PerformSearch(sourceMailbox14);
            User.AtUsersForm().SelectEntryBylocator(sourceMailbox14);
            User.AtUsersForm().SelectAction(ActionType.AddToProfile);
            User.AtUsersForm().Apply();
            User.AtUsersForm().SelectProfile("TestProfile1");
            User.AtUsersForm().ClickProfileOk();
            User.AtUsersForm().PerformSearch(sourceMailbox15);
            User.AtUsersForm().SelectEntryBylocator(sourceMailbox15);
            User.AtUsersForm().SelectAction(ActionType.AddToProfile);
            User.AtUsersForm().Apply();
            User.AtUsersForm().SelectProfile("TestProfile1");
            User.AtUsersForm().ClickProfileOk();
            //User.AtProjectOverviewForm().OpenUsersList();
            ////NOTE: Sync entry13
            //User.AtUsersForm().PerformSearch(sourceMailbox13);
            //User.AtUsersForm().SelectEntryBylocator(sourceMailbox13);
            //User.AtUsersForm().SelectAction(ActionType.AddToProfile);
            //User.AtUsersForm().Apply();
            //User.AtUsersForm().ModifyProfile("Default Profile");
            //Browser.GetDriver().SwitchTo().Window(Browser.GetDriver().WindowHandles.Last());
            //User.AtUsersForm().PerformSearch(sourceMailbox13);
        }
    }
}
