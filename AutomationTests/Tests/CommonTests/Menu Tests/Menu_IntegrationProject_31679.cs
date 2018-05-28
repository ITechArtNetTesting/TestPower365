using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;

namespace Product.Tests.CommonTests.Menu_Tests
{
    [TestClass]
    public class Menu_IntegrationProject_31679 : LoginAndConfigureTest
    {
       private string userName;
       private string password;
       private string client;
       private string project;

        [ClassInitialize]
		public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;             
        }

        [TestInitialize()]
        public void Initialize()
        {
            userName = RunConfigurator.GetUserLogin("client2");
            password = RunConfigurator.GetPassword("client2");
            client = RunConfigurator.GetClient("client2");
            project = RunConfigurator.GetProjectName("client2", "project2");


        }

        [TestMethod]
        [TestCategory("Menu")]
        public void Menu_IntegrationProject_31679_part1_menuItemsShouldBeDisplayed()
        {

            LoginAndSelectRole(userName, password, client);
            SelectProject(project);
            User.AtProjectOverviewForm().OpenMainMenu();
            User.AtProjectOverviewForm().AtMainMenu().AssertMenuForIntegrationProject();

        }

        [TestMethod]
        [TestCategory("Menu")]
        public void Menu_IntegrationProject_53237_correctLinkForEachMenuItems()
        {

            LoginAndSelectRole(userName, password, client);
            SelectProject(project);
            User.AtProjectOverviewForm().OpenMainMenu();
            User.AtProjectOverviewForm().AtMainMenu().AssertMenuLinkForIntegrationProject();
            

        }
      
    }


}
