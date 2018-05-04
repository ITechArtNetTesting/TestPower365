using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;

namespace Product.Tests.CommonTests.SetupTests
{
    [TestClass]
    public class FirstUserThirdProject : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("Setup")]
        public void SetupFirstUserThirdProject_MailOnly()
        {
            LoginAndSelectRole(RunConfigurator.GetUserLogin("client1"),
                                RunConfigurator.GetPassword("client1"),
                                RunConfigurator.GetClient("client1"));

            AddMailOnlyProject(RunConfigurator.GetProjectName("client1", "project3"),
                RunConfigurator.GetTenantValue("T3->T4", "source", "user"),
                RunConfigurator.GetTenantValue("T3->T4", "source", "password"),
                RunConfigurator.GetTenantValue("T3->T4", "target", "user"),
                RunConfigurator.GetTenantValue("T3->T4", "target", "password"),
                RunConfigurator.GetFileName("client1", "project3", "file1"));
            User.AtProjectOverviewForm().OpenUsersList();
        }
    }
}
