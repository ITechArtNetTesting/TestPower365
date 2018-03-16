using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.TP365TestCases.Abstractions;
using Product.WorkElements.Steps;
using System;

namespace Product.Tests.TP365TestCases
{
    [TestClass]
    public class LogInTest : TestCase
    {        
        LoginSteps loginSteps { get; set; }

        [TestMethod]
        [TestCategory("NewTest")]
        public void LogInApp()
        {         
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");

            try
            {
                loginSteps.LoginAndSelectRole(login, password, client);
                loginSteps.SelectProject(projectName);
                //User.AtProjectOverviewForm().AssertAllContentBlocksArePresent();
                //User.AtProjectOverviewForm().AssertConnectionsStatusesExist(1);
            }
            catch (Exception e)
            {
                //  LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }

        }
    }
}
