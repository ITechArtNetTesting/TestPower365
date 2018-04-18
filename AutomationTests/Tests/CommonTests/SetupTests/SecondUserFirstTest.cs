using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Product.Framework;
using Product.Framework.Elements;

namespace Product.Tests.CommonTests.SetupTests
{
	[TestClass]
	public class SecondUserFirstTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("Setup")]
		public void SetupSecondUserFirstProject()
		{
            LoginAndSelectRole(configurator.GetUserLogin("client2"),
                               configurator.GetPassword("client2"),
                               configurator.GetClient("client2"));

			User.AtTenantRestructuringForm().AddProjectClick();
			User.AtChooseYourProjectTypeForm().ChooseMailWithDiscovery();
			User.AtChooseYourProjectTypeForm().GoNext();
            User.AtSetProjectNameForm().SetName(configurator.GetProjectName("client2", "project1"));
                       
			User.AtSetProjectNameForm().GoNext();
			User.AtSetProjectDescriptionForm().SetDescription(StringRandomazer.MakeRandomString(20));
			User.AtSetProjectDescriptionForm().GoNext();

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(configurator.GetTenantValue("T1->T2", "source", "user"), configurator.GetTenantValue("T1->T2", "source", "password"));
            
			Driver.GetDriver(driver.GetDriverKey()).SwitchTo().Window(store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(configurator.GetTenantValue("T1->T2", "target", "user"), configurator.GetTenantValue("T1->T2", "target", "password"));
            
			Driver.GetDriver(driver.GetDriverKey()).SwitchTo().Window(store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
			User.AtAddTenantsForm().GoNext();
			User.AtSelectSourceTenantForm().SelectTenant(configurator.GetTenantValue("T1->T2", "source", "name"));
			User.AtSelectSourceTenantForm().GoNext();
			User.AtSelectTargetTenantForm().SelectTenant(configurator.GetTenantValue("T1->T2", "target", "name"));
			User.AtSelectTargetTenantForm().GoNext();
			User.AtReviewTenantPairsForm().GoNext();
			User.AtSelectSourceDomainForm().SelectDomain(configurator.GetTenantValue("T1->T2", "source", "domain"));
			User.AtSelectSourceDomainForm().GoNext();
			User.AtSelectTargetDomainForm().SelectDomain(configurator.GetTenantValue("T1->T2", "target", "domain"));
			User.AtSelectTargetDomainForm().GoNext();
			User.AtReviewDomainsPairsForm().GoNext();
			User.AtMigrationTypeForm().SelectGroupsOption();
			User.AtMigrationTypeForm().GoNext();
          
            User.AtSelectMigrationGroupForm().SetGroup(configurator.GetADGroupName("client2", "project1", "adgroup1"));
            User.AtSelectMigrationGroupForm().SelectGroup(configurator.GetADGroupName("client2", "project1", "adgroup1"));

           // User.AtSelectMigrationGroupForm().SetGroup(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='adgroup1']/../name"));
			//User.AtSelectMigrationGroupForm().SelectGroup(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='adgroup1']/../name"));
			User.AtSelectMigrationGroupForm().GoNext();
			User.AtReviewGroupsForm().GoNext();
			User.AtHowToMatchUsersForm().GoNext();
			User.AtMigrationWavesForm().GoNext();
			User.AtDefineMigrationWavesForm().SelectNo();
			User.AtDefineMigrationWavesForm().GoNext();
			User.AtSyncScheduleForm().GoNext();
			User.AtGoodToGoForm().GoNext();
			User.AtBeginDiscoveryForm().GoNext();
			User.AtDiscoveryProgressForm().WaitForDiscoveryIsCompleted();
			User.AtDiscoveryIsCompleteForm().GoNext();
			User.AtProjectOverviewForm().OpenUsersList();
		}
	}
}