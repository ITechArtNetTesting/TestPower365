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
    public class TC25825_Verify_Stop_Action_is_avaliable_in_the_User_Migration_View_In_Progress: LoginAndConfigureTest
    {        
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void VerifyStopActionIsAvaliableInTheUserMigrationViewInProgress()
        {            
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
            string sourceMailbox = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='TC25825Entry']/..//source");

            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().GetUsersCount();
                User.AtProjectOverviewForm().OpenUsersList();                
                User.AtUsersForm().SyncUserByLocator(sourceMailbox);
                User.AtUsersForm().Confirm();                
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                User.AtUsersForm().VerifyStopButtonIsAvailiable();
                User.AtUsersForm().StopSyncingSelectedUser();
                User.AtUsersForm().AssertMigrationJobWasStopped(sourceMailbox);
            }
            catch (Exception e)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
        }
    }
}
