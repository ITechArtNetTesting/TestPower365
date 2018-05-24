using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.CommonTests.LandingPage
{

    [TestFixture]
    public class LogsButtonOnTheTenantManagementWillDownloadTenantLog_TC25773: TestBase
    {
        public LogsButtonOnTheTenantManagementWillDownloadTenantLog_TC25773() : base() { }

        private string _client;
        private string _username;
        private string _projectName;
        private string _password;
        private string _tenant;
        private string _downloadsPath;
        private EditTenantsPage tenantsEditPage;        

        private void SetTestCaseParams(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _projectName = _project.Name;
            _tenant = Automation.Settings.GetByReference<Tenant>(_project.Source).Name;
            _downloadsPath = Automation.Settings.DownloadsPath;
        }

        private void LogsButtonOnTheTenantManagementWillDownloadTenantLog(string client, string project)
        {
            SetTestCaseParams(client, project);
            tenantsEditPage = Automation.Common
                .SingIn(_username, _password)
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .TenantsEdit()
                .GetPage<EditTenantsPage>();
            tenantsEditPage.ClickDiscoveryTab();
            tenantsEditPage.DeleteTenantLogs(_downloadsPath);
            tenantsEditPage.DownloadLogs(_tenant);
            NUnit.Framework.Assert.IsTrue(tenantsEditPage.CheckDiscoveryFileIsDownloaded(_downloadsPath,15),"");
        }

        [Test]
      //  [TestCategory("MailWithDiscovery")]
     //   [TestCategory("UI")]
        public void LogsButtonOnTheTenantManagementWillDownloadTenantLog_MD_25773()
        {
            LogsButtonOnTheTenantManagementWillDownloadTenantLog("client2", "project1");
        }


        //  [TestCategory("Integration")]
        //   [TestCategory("UI")]
        [Test]
        public void LogsButtonOnTheTenantManagementWillDownloadTenantLog_Integration_25773()
        {
            LogsButtonOnTheTenantManagementWillDownloadTenantLog("client2", "project2");
        }
    }
}
