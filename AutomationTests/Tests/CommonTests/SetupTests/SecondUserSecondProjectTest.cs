using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Utilities;

namespace Product.Tests.CommonTests.SetupTests
{
	[TestClass]
	public class SecondUserSecondProjectTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		
		[TestMethod]
		[TestCategory("Setup")]
		public void SetupSecondUserSecondProject()
		{
            LoginAndSelectRole(RunConfigurator.GetUserLogin("client2"),
                               RunConfigurator.GetPassword("client2"),
                               RunConfigurator.GetClient("client2"));

            var projectName = RunConfigurator.GetProjectName("client2", "project2");

            User.AtTenantRestructuringForm().AddProjectClick();
			User.AtChooseYourProjectTypeForm().ChooseIntegration();
			User.AtChooseYourProjectTypeForm().GoNext();
            User.AtSetProjectNameForm().SetName(projectName);
            User.AtSetProjectNameForm().GoNext();
			User.AtSetProjectDescriptionForm().SetDescription(StringRandomazer.MakeRandomString(20));
			User.AtSetProjectDescriptionForm().GoNext();
			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();

            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T5->T6", "source", "user"), RunConfigurator.GetTenantValue("T5->T6", "source", "password"));
            
			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T5->T6", "target", "user"), RunConfigurator.GetTenantValue("T5->T6", "target", "password"));

			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
			User.AtAddTenantsForm().GoNext();
			User.AtSelectSourceTenantForm().SelectTenant(RunConfigurator.GetTenantValue("T5->T6", "source", "name"));
			User.AtSelectSourceTenantForm().GoNext();
			User.AtSelectTargetTenantForm().SelectTenant(RunConfigurator.GetTenantValue("T5->T6", "target", "name"));
			User.AtSelectTargetTenantForm().GoNext();
			User.AtReviewTenantPairsForm().GoNext();
			User.AtSelectSourceDomainForm().SelectDomain(RunConfigurator.GetTenantValue("T5->T6", "source", "domain"));
			User.AtSelectSourceDomainForm().GoNext();
			User.AtSelectTargetDomainForm().SelectDomain(RunConfigurator.GetTenantValue("T5->T6", "target", "domain"));
			User.AtSelectTargetDomainForm().GoNext();
			User.AtReviewDomainsPairsForm().AddAnotherPair();
			User.AtSelectSourceDomainForm().SelectDomain(RunConfigurator.GetTenantValue("T5->T6", "source", "additionaldomain"));
			User.AtSelectSourceDomainForm().GoNext();
			User.AtSelectTargetDomainForm().SelectDomain(RunConfigurator.GetTenantValue("T5->T6", "target", "additionaldomain"));
			User.AtSelectTargetDomainForm().GoNext();
			User.AtReviewDomainsPairsForm().GoNext();
			User.AtMigrationTypeForm().SelectGroupsOption();
			User.AtMigrationTypeForm().GoNext();
            User.AtSelectMigrationGroupForm().SetGroup(RunConfigurator.GetADGroupName ("client2", "project2", "adgroup1"));
            User.AtSelectMigrationGroupForm().SelectGroup(RunConfigurator.GetADGroupName("client2", "project2", "adgroup1"));

			User.AtSelectMigrationGroupForm().GoNext();
			User.AtReviewGroupsForm().GoNext();
			User.AtHowToMatchUsersForm().GoNext();
			User.AtCreateUsersForm().SelectCreateUsers();
			User.AtCreateUsersForm().GoNext();
			User.AtMigrateDistributionGroupsForm().SelectUploadList();
			User.AtMigrateDistributionGroupsForm().GoNext();
            User.AtUploadDistributionListForm().SelectFile(RunConfigurator.GetFileName("client2","project2","file1")); 
			User.AtUploadDistributionListForm().GoNext();
			User.AtHowToMatchGroupsForm().GoNext();
			User.AtCreateDistributionGroupsForm().SelectCreateGroups();
			User.AtCreateDistributionGroupsForm().GoNext();
			User.AtMigrationWavesForm().GoNext();
			//NOTE: check radiobutton
			User.AtDefineMigrationWavesForm().SelectNo();
			User.AtDefineMigrationWavesForm().GoNext();
			User.AtSyncScheduleForm().GoNext();
			User.AtGoodToGoForm().GoNext();
			User.AtBeginDiscoveryForm().GoNext();
			User.AtDiscoveryProgressForm().WaitForDiscoveryIsCompleted();
            User.AtDiscoveryIsCompleteForm().ScrollToTheBottom();
            User.AtDiscoveryIsCompleteForm().GoNext();
			User.AtUserMigrationExpirienceForm().GoNext();
			User.AtSyncAddressBooksForm().SelectDontSyncAtAll();
			User.AtSyncAddressBooksForm().GoNext();
			User.AtShareCalendarForm().SelectYes();
			User.AtShareCalendarForm().GoNext();
			User.AtWhichUsersShareCalendarForm().SelectByAd();
			User.AtWhichUsersShareCalendarForm().GoNext();
            User.AtCalendarActiveDirectoryGroupForm().SetGroup(RunConfigurator.GetADGroupName("client2", "project2", "adgroup1"));
            User.AtCalendarActiveDirectoryGroupForm().SelectGroup(RunConfigurator.GetADGroupName("client2", "project2", "adgroup1"));
             
			User.AtCalendarActiveDirectoryGroupForm().GoNext();
			User.AtEnablePublicFoldersForm().SetYes();
			User.AtEnablePublicFoldersForm().GoNext();
            User.AtPublicFolderListForm().SelectNo();
            User.AtPublicFolderListForm().GoNext();
            User.AtTenantPareForm().SelectPare(RunConfigurator.GetTenantValue("T5->T6", "source", "name"));
		    User.AtTenantPareForm().GoNext();
            User.AtPublicFolderSourceFilePathForm().SetFilePath(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		    User.AtPublicFolderSourceFilePathForm().GoNext();
            User.AtPublicFolderTargetFilePathForm().SetFilePath(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//target"));
		    User.AtPublicFolderTargetFilePathForm().GoNext();
            User.AtPublicFolderSyncLevelForm().SelectAllSubFolders();
		    User.AtPublicFolderSyncLevelForm().GoNext();
            User.AtPublicFolderScheduleForm().SelectOnDemand();
		    User.AtPublicFolderScheduleForm().GoNext();
            User.AtPublicFolderConflictsForm().Overwrites();
		    User.AtPublicFolderConflictsForm().GoNext();
            User.AtPublicFolderCompleteForm().GoNext();
            User.AtDirectorySyncStatusForm().GoNext();
            //User.AtConfigureDirectorySyncForm().GoNext();
		    User.AtDownloadDirSyncForm().ScrollToTheBottom();
            User.AtDownloadDirSyncForm().GoNext();
			User.AtDirectorySyncSettingsForm().StoreAccessKey();
			User.AtDirectorySyncSettingsForm().StoreAccessUrl();
			User.AtDirectorySyncSettingsForm().GoNext();
			User.AtEnterPasswordForm().SetPassword(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='password1']/../value"));
			User.AtEnterPasswordForm().SetConfirmPassword(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='password1']/../value"));
			User.AtEnterPasswordForm().GoNext();
			User.AtEmailRewritingForm().GoNext();
			User.AtConfigureEmailRewrittingForm().SelectNo();
			User.AtConfigureEmailRewrittingForm().GoNext();
			User.AtGoodToGoForm().ScrollToTheBottom();
			User.AtGoodToGoForm().GoNext();
			User.AtProjectOverviewForm().OpenUsersList();

            var appKey = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//metaname[text()='project2']/..//dirSyncAppKey");

            Assert.IsFalse(string.IsNullOrWhiteSpace(appKey));
            Assert.IsFalse(string.IsNullOrWhiteSpace(projectName));

            using (var dbT2T = new SqlClient(RunConfigurator.GetConnectionString()))
            {
                var results = dbT2T.ExecuteNonQuery(string.Format("UPDATE Project SET DirSyncAppKey = '{1}' WHERE ProjectName = '{0}'", projectName, appKey));
                Log.InfoFormat("Updated DirSyncAppKey for {0} projects", results);
            }
        }
	}
}
