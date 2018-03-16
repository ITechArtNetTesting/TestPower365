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
			LoginAndSelectRole(RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user"),
				RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password"), 
				RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name"));
			AddMailOnlyProject(RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name"),
				RunConfigurator.GetTenantValue("T1->T2", "source", "user"),
				RunConfigurator.GetTenantValue("T1->T2", "source", "password"),
				RunConfigurator.GetTenantValue("T1->T2", "target", "user"),
				RunConfigurator.GetTenantValue("T1->T2", "target", "password"),
				RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='file1']/..//filename"));
			User.AtProjectOverviewForm().OpenUsersList();
		}
	}
}