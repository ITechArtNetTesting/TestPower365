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
			LoginAndSelectRole(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user"),
			RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password"),
			RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name"));
			User.AtTenantRestructuringForm().AddProjectClick();
			User.AtChooseYourProjectTypeForm().ChooseIntegration();
			User.AtChooseYourProjectTypeForm().GoNext();
			User.AtSetProjectNameForm().SetName(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/../name"));
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
			User.AtSelectMigrationGroupForm().SetGroup(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='adgroup1']/../name")); 
			User.AtSelectMigrationGroupForm().SelectGroup(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='adgroup1']/../name")); 
			User.AtSelectMigrationGroupForm().GoNext();
			User.AtReviewGroupsForm().GoNext();
			User.AtHowToMatchUsersForm().GoNext();
			User.AtCreateUsersForm().SelectCreateUsers();
			User.AtCreateUsersForm().GoNext();
			User.AtMigrateDistributionGroupsForm().SelectUploadList();
			User.AtMigrateDistributionGroupsForm().GoNext();
			User.AtUploadDistributionListForm().SelectFile(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='file1']/..//filename"));
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
			User.AtDiscoveryIsCompleteForm().GoNext();
			User.AtUserMigrationExpirienceForm().GoNext();
			User.AtSyncAddressBooksForm().SelectDontSyncAtAll();
			User.AtSyncAddressBooksForm().GoNext();
			User.AtShareCalendarForm().SelectYes();
			User.AtShareCalendarForm().GoNext();
			User.AtWhichUsersShareCalendarForm().SelectByAd();
			User.AtWhichUsersShareCalendarForm().GoNext();
			User.AtCalendarActiveDirectoryGroupForm().SetGroup(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='adgroup1']/../name"));
			User.AtCalendarActiveDirectoryGroupForm().SelectGroup(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='adgroup1']/../name"));
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
		}
	}
}
