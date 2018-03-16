using ITechArtTestFramework.StaticClasses;
using ITechArtTestFramework.TestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tests.Steps;

namespace Tests.TestCases
{
    [TestClass]
    public class LogInTest : TestCase
    {        
        LoginSteps loginSteps { get; set; }

        [TestMethod]
        [TestCategory("NewTest")]
        public void LogInApp()
        {         
            string login = XmlConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = XmlConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = XmlConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
            string projectName = XmlConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");

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
