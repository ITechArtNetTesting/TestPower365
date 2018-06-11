using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.IntegrationProjectTests
{
    [TestFixture]
    class VerifySortAndFilterOptionsForGroupSyncUI_TC31797 : TestBase
    {
        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void VerifySortAndFilterOptionsForGroupSyncUI_Integration_31797()
        {
            TestRun("client2", "project2");
        }

        private void TestRun(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            string username = client.Administrator.Username;
            string password = client.Administrator.Password;

            VerifySortAndFilterOptionsForGroupSyncUI(username, password, client.Name, _project.Name);
        }

        private void VerifySortAndFilterOptionsForGroupSyncUI(string username,string password,string client,string project)
        {
            var groupsPage = Automation.Common
                .SingIn(username, password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(client)
                .ProjectSelect(project)
                .ClickEditGroups()
                .GetPage<GroupsPage>();
            groupsPage.SelectVisibleGroups();
            groupsPage.ClickViewSelectedGroups();
            groupsPage.SortSource();
            Assert.IsTrue(groupsPage.CheckSortWorkingCorrectly());
            groupsPage.ClickViewAllGroups();
            groupsPage.ClickFilter();
            groupsPage.ChooseFilter("Matched");
            Assert.IsTrue(groupsPage.CheckAllGroupsStatus("Matched"));
        }
    }
}
