using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;

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
                      
            LoginAndSelectRole(RunConfigurator.GetUserLogin("client1"),
			                   RunConfigurator.GetPassword("client1"),
                               RunConfigurator.GetClient("client1"));
           
			AddMailOnlyProject(RunConfigurator.GetProjectName("client1","project1"),
				                RunConfigurator.GetTenantValue("T1->T2", "source", "user"),
				                RunConfigurator.GetTenantValue("T1->T2", "source", "password"),
			                	RunConfigurator.GetTenantValue("T1->T2", "target", "user"),
			                	RunConfigurator.GetTenantValue("T1->T2", "target", "password"),
			                	RunConfigurator.GetFileName("client1","project1", "file1"));
			User.AtProjectOverviewForm().OpenUsersList();
		}
	}
}