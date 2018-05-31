using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.AddressBook
{
    [TestFixture]
    public class VerifyAddressBookWillBeDisplayedInManageTenants_TC32860: TestBase
    {
        public VerifyAddressBookWillBeDisplayedInManageTenants_TC32860() : base() { }


        private void VerifyAddressBookWillBeDisplayedInManageTenants(string _client, string _username, string _projectName, string _password)
        {
            EditTenantsPage tenantsEditPage;

            tenantsEditPage = Automation.Common
                .SingIn(_username, _password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();
            Assert.IsTrue(tenantsEditPage.AddressBookTab.IsVisible(),"Addres book tab is not visible");
        }
       
        [Test]
        public void VerifyAddressBookWillBeDisplayedInManageTenants_Integration_32860()
        {
            TestRun("client2", "project2");
        }

        public void TestRun(string clientName, string project)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(project);
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = _project.Name;
            

            VerifyAddressBookWillBeDisplayedInManageTenants(_client, _username, _projectName, _password);
        }

    }
}
