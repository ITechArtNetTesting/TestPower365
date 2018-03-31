using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T365.Framework;

namespace Product.Tests_refactoring_.CommonTests_refactoring_.IntegrationTests_refactoring_
{
    [TestClass]
    public class GroupsViewTest : BaseTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        public void Automation_IN_GroupsViewTest()
        {
            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string adGroup2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='adgroup2']/../name");

            Tester.AtStartPage().SignIn();
            Tester.AtMicrosoftLoginPage().LogIn(userName, password);
            Tester.AtStartPage().OpenRightBar();
            Tester.AtRightBar().ChooseClientByKeys(client);
            Tester.AtProjectsPage().ViewProjectsGroups(project, adGroup2);
        }

    }
}
