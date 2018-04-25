using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.IntegrationTests
{
    [TestClass]
    public class TC32951_IntegrationPro_Verify_only_Mail_UPN_and_ExtensionAttributes_will_be_valid_matching_options_for_users_during_Project_Wizard: LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Integration")]
        public void IntegrationProVerifyOnlyMailUPNAndExtensionAttributesWillBeValidMatchingOptionsForUsersDuringProjectWizard()
        {
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");          

            try
            {
                LoginAndSelectRole(login, password, client);
                User.AtTenantRestructuringForm().AddProjectClick();
                User.AtChooseYourProjectTypeForm().ChooseIntegration();
                User.AtChooseYourProjectTypeForm().GoNext();
                User.AtSetProjectNameForm().SetName("TC32951Project"+Convert.ToString(new Random().Next(1000)));
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
                User.AtSelectMigrationGroupForm().SetGroup(RunConfigurator.GetADGroupName("client2", "project2", "adgroup1"));
                User.AtSelectMigrationGroupForm().SelectGroup(RunConfigurator.GetADGroupName("client2", "project2", "adgroup1"));
                User.AtSelectMigrationGroupForm().GoNext();
                User.AtReviewGroupsForm().GoNext();
                User.AtHowToMatchUsersForm().CheckAttributes();              
            }
            catch (Exception ex)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw ex;
            }
        }
    }
}
