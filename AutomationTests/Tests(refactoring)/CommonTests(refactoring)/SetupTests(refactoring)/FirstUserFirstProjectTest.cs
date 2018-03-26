using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using T365.Database;
using T365Framework;

namespace Product.Tests_refactoring_.CommonTests_refactoring_.SetupTests_refactoring_
{
    [TestClass]
    public class FirstUserFirstProjectTest : BaseTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Setup")]
        public void SetupFirstUserFirstProject()
        {
            SQLQuery sqlForDelete = new SQLQuery(RunConfigurator.GetConnectionString());
            sqlForDelete.DeleteProject("2");
            sqlForDelete.DeleteTenant("2");

            Tester.AtStartPage().SignIn();
            Tester.AtMicrosoftLoginPage().LogIn(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"));
            Tester.AtStartPage().OpenRightBar();
            Tester.AtRightBar().ChooseClientByKeys(RunConfigurator.GetUserLogin("client1"));
        }
    }
}
