using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailWithDiscoveryTests.ActionButtonsTests
{
    [TestClass]
    public class TC25824_Verify_Sync_Action_is_available_in_the_User_Migration_View : LoginAndConfigureTest
    {     
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void VerifySyncActionIsAvailableInTheUserMigrationView()
        {            
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
            string sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='TC25824Entry']/..//source");
            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().VerifyForMatchedUsersSyncActionIsAvailable();
                User.AtUsersForm().SyncUserByLocator(sourceMailbox);
                User.AtUsersForm().Confirm();
                User.AtUsersForm().VerifySubmittingForMigration(sourceMailbox);
            }
            catch (Exception e)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
        }
    }
}
