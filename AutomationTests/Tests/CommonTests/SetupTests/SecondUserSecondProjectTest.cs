using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using T365.Database;

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
            LoginAndSelectRole(configurator.GetUserLogin("client2"),
                               configurator.GetPassword("client2"),
                               configurator.GetClient("client2"));

            var projectName = configurator.GetProjectName("client2", "project2");

            User.AtTenantRestructuringForm().AddProjectClick();
			User.AtChooseYourProjectTypeForm().ChooseIntegration();
			User.AtChooseYourProjectTypeForm().GoNext();
            User.AtSetProjectNameForm().SetName(projectName);
            User.AtSetProjectNameForm().GoNext();
			User.AtSetProjectDescriptionForm().SetDescription(StringRandomazer.MakeRandomString(20));
			User.AtSetProjectDescriptionForm().GoNext();
			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();

            Office365TenantAuthorization(configurator.GetTenantValue("T5->T6", "source", "user"), configurator.GetTenantValue("T5->T6", "source", "password"));
            
			Driver.GetDriver(driver.GetDriverKey()).SwitchTo().Window(store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(configurator.GetTenantValue("T5->T6", "target", "user"), configurator.GetTenantValue("T5->T6", "target", "password"));

			Driver.GetDriver(driver.GetDriverKey()).SwitchTo().Window(store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
			User.AtAddTenantsForm().GoNext();
			User.AtSelectSourceTenantForm().SelectTenant(configurator.GetTenantValue("T5->T6", "source", "name"));
			User.AtSelectSourceTenantForm().GoNext();
			User.AtSelectTargetTenantForm().SelectTenant(configurator.GetTenantValue("T5->T6", "target", "name"));
			User.AtSelectTargetTenantForm().GoNext();
			User.AtReviewTenantPairsForm().GoNext();
			User.AtSelectSourceDomainForm().SelectDomain(configurator.GetTenantValue("T5->T6", "source", "domain"));
			User.AtSelectSourceDomainForm().GoNext();
			User.AtSelectTargetDomainForm().SelectDomain(configurator.GetTenantValue("T5->T6", "target", "domain"));
			User.AtSelectTargetDomainForm().GoNext();
			User.AtReviewDomainsPairsForm().AddAnotherPair();
			User.AtSelectSourceDomainForm().SelectDomain(configurator.GetTenantValue("T5->T6", "source", "additionaldomain"));
			User.AtSelectSourceDomainForm().GoNext();
			User.AtSelectTargetDomainForm().SelectDomain(configurator.GetTenantValue("T5->T6", "target", "additionaldomain"));
			User.AtSelectTargetDomainForm().GoNext();
			User.AtReviewDomainsPairsForm().GoNext();
			User.AtMigrationTypeForm().SelectGroupsOption();
			User.AtMigrationTypeForm().GoNext();
            User.AtSelectMigrationGroupForm().SetGroup(configurator.GetADGroupName ("client2", "project2", "adgroup1"));
            User.AtSelectMigrationGroupForm().SelectGroup(configurator.GetADGroupName("client2", "project2", "adgroup1"));

			User.AtSelectMigrationGroupForm().GoNext();
			User.AtReviewGroupsForm().GoNext();
			User.AtHowToMatchUsersForm().GoNext();
			User.AtCreateUsersForm().SelectCreateUsers();
			User.AtCreateUsersForm().GoNext();
			User.AtMigrateDistributionGroupsForm().SelectUploadList();
			User.AtMigrateDistributionGroupsForm().GoNext();
            User.AtUploadDistributionListForm().SelectFile(configurator.GetFileName("client2","project2","file1")); 
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
            User.AtCalendarActiveDirectoryGroupForm().SetGroup(configurator.GetADGroupName("client2", "project2", "adgroup1"));
            User.AtCalendarActiveDirectoryGroupForm().SelectGroup(configurator.GetADGroupName("client2", "project2", "adgroup1"));
             
			User.AtCalendarActiveDirectoryGroupForm().GoNext();
			User.AtEnablePublicFoldersForm().SetYes();
			User.AtEnablePublicFoldersForm().GoNext();
            User.AtPublicFolderListForm().SelectNo();
            User.AtPublicFolderListForm().GoNext();
            User.AtTenantPareForm().SelectPare(configurator.GetTenantValue("T5->T6", "source", "name"));
		    User.AtTenantPareForm().GoNext();
            User.AtPublicFolderSourceFilePathForm().SetFilePath(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		    User.AtPublicFolderSourceFilePathForm().GoNext();
            User.AtPublicFolderTargetFilePathForm().SetFilePath(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//target"));
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
			User.AtEnterPasswordForm().SetPassword(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='password1']/../value"));
			User.AtEnterPasswordForm().SetConfirmPassword(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='password1']/../value"));
			User.AtEnterPasswordForm().GoNext();
			User.AtEmailRewritingForm().GoNext();
			User.AtConfigureEmailRewrittingForm().SelectNo();
			User.AtConfigureEmailRewrittingForm().GoNext();
			User.AtGoodToGoForm().ScrollToTheBottom();
			User.AtGoodToGoForm().GoNext();
			User.AtProjectOverviewForm().OpenUsersList();

            var appKey = configurator.GetValueByXpath($"//metaname[text()='client2']/..//metaname[text()='project2']/..//dirSyncAppKey");

            Assert.IsFalse(string.IsNullOrWhiteSpace(appKey));
            Assert.IsFalse(string.IsNullOrWhiteSpace(projectName));

            SQLQuery dbUpdate = new SQLQuery(configurator.GetConnectionString());
          //  dbUpdate.SetDirSyncAppKeyByProjectName(projectName, appKey);
        }
	}
}
