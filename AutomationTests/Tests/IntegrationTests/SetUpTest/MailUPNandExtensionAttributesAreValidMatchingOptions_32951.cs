using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.IntegrationTests.SetUpTest
{
    [TestClass]
    public class MailUPNandExtensionAttributesAreValidMatchingOptions_TC32951 : LoginAndConfigureTest
    {

        private string login;
        private string password;
        private string client;
        private string project;
        private string sourceMailbox9;


        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestInitialize()]
        public void Initialize()
        {
            login = RunConfigurator.GetUserLogin("client2");
            password = RunConfigurator.GetPassword("client2");
            client = RunConfigurator.GetClient("client2");          
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("SetUpProject")]
        [TestCategory("UI")]
        public void MailUPNandExtensionAttributesAreValidMatchingOptions_32951()
        {
            LoginAndSelectRole(login, password, client);
            User.AtTenantRestructuringForm().AddProjectClick();
            User.AtChooseYourProjectTypeForm().ChooseIntegration();
            User.AtChooseYourProjectTypeForm().GoNext();
            User.AtSetProjectNameForm().SetName("TC32951Project_" + Convert.ToString(new Random().Next(1000)));
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
    } 
}
