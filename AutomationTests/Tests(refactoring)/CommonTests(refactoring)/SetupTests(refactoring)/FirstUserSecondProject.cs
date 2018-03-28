using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using T365.Database;
using T365Framework;

namespace Product.Tests_refactoring_.CommonTests_refactoring_.SetupTests_refactoring_
{
    [TestClass]
    public class FirstUserSecondProject : BaseTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Setup")]
        public void SetupFirstUserSecondProject()
        {
            DatabaseCleanup cleaner = new DatabaseCleanup();
            cleaner.CleanUp(RunConfigurator.GetRole("client1"));

            Tester.AtStartPage().SignIn();
            Tester.AtMicrosoftLoginPage().LogIn(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"));
            Tester.AtStartPage().OpenRightBar();
            Tester.AtRightBar().ChooseClientByKeys(RunConfigurator.GetRole("client1"));
            Tester.AtProjectsPage().AddNewEmailFromFileProject(RunConfigurator.GetProjectName("client1", "project2"),
                RunConfigurator.GetTenantValue("T3->T4", "source", "user"),
                RunConfigurator.GetTenantValue("T3->T4", "source", "password"),
                RunConfigurator.GetTenantValue("T3->T4", "target", "user"),
                RunConfigurator.GetTenantValue("T3->T4", "target", "password"),
                RunConfigurator.GetFileName("client1", "project2", "file1"));
        }
    }
}
