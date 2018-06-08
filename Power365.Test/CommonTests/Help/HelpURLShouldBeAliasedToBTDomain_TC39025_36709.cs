using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.Help
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class HelpURLShouldBeAliasedToBTDomain_TC39025_36709 : TestBase
    {
        // without project select 
        [Test]
        [Category("UI")]
        [Category("Menu")]
        public void HelpURLShouldBeAliasedToBTDomain_39025()
        {            
            var expectedUrl = Automation.Settings.HelpURL;
            var client = Automation.Settings.GetByReference<Client>("client1");
            var username = client.Administrator.Username;
            var password = client.Administrator.Password;

            var helpPage = Automation.Common
                        .SingIn(username, password)
                        .GetPage<PageBase>()
                        .Menu
                        .ClickHelp();

            Assert.IsTrue(helpPage.GetUrl().Contains(expectedUrl), "The help link incorrect");
        }

        // after project select 
        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void HelpLinkUnderGeneralAndTheFootnotesWorks_Integrate_36709_part1()
        {
            RunTest("client2", "project2");
        }

        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        public void HelpLinkUnderGeneralAndTheFootnotesWorks_MD_36709_part1()
        {
            RunTest("client2", "project1");
        }

        [Test]
        [Category("UI")]
        [Category("MailOnly")]
        public void HelpLinkUnderGeneralAndTheFootnotesWorks_MO_36709_part1()
        {
            RunTest("client1", "project1");
        }

        private void RunTest(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var project = client.GetByReference<Project>(projectName);
            var expectedUrl = Automation.Settings.HelpURL;
            HelpLinkUnderGeneral(client.Administrator.Username, client.Administrator.Password, client.Name, project.Name, expectedUrl);
        }

        private void HelpLinkUnderGeneral(string username, string password, string client, string projectName, string expectedUrl)
        {
            var helpPage = Automation.Common
                .SingIn(username, password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(client)
                .ProjectSelect(projectName)
                .GetPage<ProjectDetailsPage>()
                .Menu
                .ClickHelp();

            // verify the help url after project choose
            Assert.IsTrue(helpPage.GetUrl().Contains(expectedUrl), "The help link incorrect");


        }



    }
}
