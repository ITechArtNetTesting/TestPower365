using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using T365Framework;

namespace Product.Tests_refactoring_.CommonTests_refactoring_.SetupTests_refactoring_
{
    [TestClass]
    public class SecondUserFirstProject:BaseTest
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
            DatabaseCleanup cleaner = new DatabaseCleanup();
            cleaner.CleanUp(RunConfigurator.GetRole("client2"));

            Tester.AtStartPage().SignIn();
            Tester.AtMicrosoftLoginPage().LogIn(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"));
            Tester.AtStartPage().OpenRightBar();
            Tester.AtRightBar().ChooseClientByKeys(RunConfigurator.GetRole("client2"));
            Tester.AtProjectsPage().AddNewEmailWithDiscoveryProject(RunConfigurator.GetProjectName("client2", "project1"), RunConfigurator.GetTenantValue("T1->T2", "source", "user"), RunConfigurator.GetTenantValue("T1->T2", "source", "password"), RunConfigurator.GetTenantValue("T1->T2", "target", "user"), RunConfigurator.GetTenantValue("T1->T2", "target", "password"));
        }
    }
}
