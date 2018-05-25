using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.AddressBook
{
    [TestFixture]
    public class VerifyAddressBookWillBeDisplayedInManageTenants_TC32860: TestBase
    {
        public VerifyAddressBookWillBeDisplayedInManageTenants_TC32860() : base() { }

        private string _client;
        private string _username;
        private string _projectName;
        private string _password;
        private string _tenant;

        private EditTenantsPage tenantsEditPage;

        private void VerifyAddressBookWillBeDisplayedInManageTenants(string clientName, string project)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(project);
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _projectName = _project.Name;
            _tenant = Automation.Settings.GetByReference<Tenant>(_project.Source).Name;
            tenantsEditPage = Automation.Common
                .SingIn(_username, _password)
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();
            Assert.IsTrue(tenantsEditPage.AddressBookTabIsVisible());
        }
       
        [Test]
        public void VerifyAddressBookWillBeDisplayedInManageTenants_Integration_32860()
        {
            VerifyAddressBookWillBeDisplayedInManageTenants("client2", "project2");
        }
    }
}
