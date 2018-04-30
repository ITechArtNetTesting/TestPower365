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
    public class TC32698_Verify_discovery_frequency_can_be_set_through_the_Discovery_page : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Migration")]
        public void VerifyDiscoveryFrequencyCanBeSetThroughTheDiscoveryPage()
        {
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");

            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenDiscoveryOverview();
                User.AtDiscoveryOverviewForm().VerifyDiscoveryFrequencyHoursMatchesDisplayedNumber(client);
                User.AtDiscoveryOverviewForm().ChangeDiscoveryFrequencyHours(1);
                User.AtDiscoveryOverviewForm().VerifyDiscoveryFrequencyHoursMatchesDisplayedNumber(client);
                User.AtDiscoveryOverviewForm().ChangeDiscoveryFrequencyHours(24);
                User.AtDiscoveryOverviewForm().VerifyDiscoveryFrequencyHoursMatchesDisplayedNumber(client);
            }
            catch (Exception ex)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw ex;
            }
        }
    }
}
