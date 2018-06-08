using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.CommonTests.Help
{
    class HelpLinkTheFootnotesWorks_36709TC : TestBase
    {
        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void HelpLinkTheFootnotesWorks_Integrate_36709()
        {
            RunTest("client2", "project2");
        }

        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        public void HelpLinkTheFootnotesWorks_MD_36709()
        {
            RunTest("client2", "project1");
        }

        [Test]
        [Category("UI")]
        [Category("MailOnly")]
        public void HelpLinkTheFootnotesWorks_MO_36709()
        {
            RunTest("client1", "project1");
        }

        private void RunTest(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var project = client.GetByReference<Project>(projectName);
            var expectedUrl = Automation.Settings.HelpURL;
            HelpLinkTheFootnotesWorks(client.Administrator.Username, client.Administrator.Password, client.Name, project.Name, expectedUrl);
        }

        private void HelpLinkTheFootnotesWorks(string username, string password, string client, string projectName, string expectedUrl)
        {
            var helpPage = Automation.Common
                .SingIn(username, password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(client)
                .ProjectSelect(projectName)
                .GetPage<ProjectDetailsPage>()
                .ClickHelp();                               

            // verify the help url 
            Assert.IsTrue(helpPage.GetUrl().Contains(expectedUrl), "The help link incorrect");


        }


    }
}
