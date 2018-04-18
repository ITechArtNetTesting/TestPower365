using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using T365.Database;

namespace Product.Tests.CommonTests.SetupTests
{
	[TestClass]
	public class FirstUserFirstProjectTest : LoginAndConfigureTest
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
                      
            LoginAndSelectRole(configurator.GetUserLogin("client1"),
                               configurator.GetPassword("client1"),
                               configurator.GetClient("client1"));
           
			AddMailOnlyProject(configurator.GetProjectName("client1","project1"),
                                configurator.GetTenantValue("T1->T2", "source", "user"),
                                configurator.GetTenantValue("T1->T2", "source", "password"),
                                configurator.GetTenantValue("T1->T2", "target", "user"),
                                configurator.GetTenantValue("T1->T2", "target", "password"),
                                configurator.GetFileName("client1","project1", "file1"));
			User.AtProjectOverviewForm().OpenUsersList();
		}
	}
}