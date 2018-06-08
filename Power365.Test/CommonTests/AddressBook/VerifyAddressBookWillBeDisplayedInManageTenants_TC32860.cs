using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests.AddressBook
{
    [TestFixture]
    public class VerifyAddressBookWillBeDisplayedInManageTenants_TC32860: TestBase
    {
        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void VerifyAddressBookWillBeDisplayedInManageTenants_Integration_32860()
        {
            var client = Automation.Settings.GetByReference<Client>("client2");
            var _project = client.GetByReference<Project>("project2");
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = _project.Name;

            EditTenantsPage tenantsEditPage;

            tenantsEditPage = Automation.Common
                .SingIn(_username, _password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();

            Assert.IsTrue(tenantsEditPage.AddressBookTab.IsVisible(), "Addres book tab is not visible");
        }  

    }
}
