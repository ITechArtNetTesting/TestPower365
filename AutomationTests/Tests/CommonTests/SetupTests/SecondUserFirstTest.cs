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
			LoginAndSelectRole(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user"),
				RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password"),
				RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name"));
			User.AtTenantRestructuringForm().AddProjectClick();
			User.AtChooseYourProjectTypeForm().ChooseMailWithDiscovery();
			User.AtChooseYourProjectTypeForm().GoNext();
			User.AtSetProjectNameForm().SetName(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/../name"));
			User.AtSetProjectNameForm().GoNext();
			User.AtSetProjectDescriptionForm().SetDescription(StringRandomazer.MakeRandomString(20));
			User.AtSetProjectDescriptionForm().GoNext();

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T1->T2", "source", "user"), RunConfigurator.GetTenantValue("T1->T2", "source", "password"));
            
			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T1->T2", "target", "user"), RunConfigurator.GetTenantValue("T1->T2", "target", "password"));
            
			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
			User.AtAddTenantsForm().GoNext();
			User.AtSelectSourceTenantForm().SelectTenant(RunConfigurator.GetTenantValue("T1->T2", "source", "name"));
			User.AtSelectSourceTenantForm().GoNext();
			User.AtSelectTargetTenantForm().SelectTenant(RunConfigurator.GetTenantValue("T1->T2", "target", "name"));
			User.AtSelectTargetTenantForm().GoNext();
			User.AtReviewTenantPairsForm().GoNext();
			User.AtSelectSourceDomainForm().SelectDomain(RunConfigurator.GetTenantValue("T1->T2", "source", "domain"));
			User.AtSelectSourceDomainForm().GoNext();
			User.AtSelectTargetDomainForm().SelectDomain(RunConfigurator.GetTenantValue("T1->T2", "target", "domain"));
			User.AtSelectTargetDomainForm().GoNext();
			User.AtReviewDomainsPairsForm().GoNext();
			User.AtMigrationTypeForm().SelectGroupsOption();
			User.AtMigrationTypeForm().GoNext();
			User.AtSelectMigrationGroupForm().SetGroup(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='adgroup1']/../name"));
			User.AtSelectMigrationGroupForm().SelectGroup(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='adgroup1']/../name"));
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