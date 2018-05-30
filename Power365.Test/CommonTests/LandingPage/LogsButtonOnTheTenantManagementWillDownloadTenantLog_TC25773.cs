using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        private void SetTestCaseParams(string clientName, string projectName)
        {           
          
           var client = Automation.Settings.GetByReference<Client>(clientName);
           var _project = client.GetByReference<Project>(projectName);
           string  _client = client.Name;
           string _username = client.Administrator.Username;
           string _password = client.Administrator.Password;
           string _projectName = _project.Name;
           string _tenant = Automation.Settings.GetByReference<Tenant>(_project.Source).Name;
           string _downloadsPath = Automation.Settings.DownloadsPath;
           LogsButtonOnTheTenantManagementWillDownloadTenantLog(_client, _projectName, _username, _password, _downloadsPath, _tenant);
    }

        private void LogsButtonOnTheTenantManagementWillDownloadTenantLog(string _client, string _projectName,string _username, string _password, string _downloadsPath, string _tenant)
        {
            
           var  tenantsEditPage = Automation.Common
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
        [Category("MailWithDiscovery")]
        [Category("UI")]
        public void LogsButtonOnTheTenantManagementWillDownloadTenantLog_MD_25773()
        {
            SetTestCaseParams("client2", "project1");
        }

        [Test]
        [Category("Integration")]
        [Category("UI")]
        public void LogsButtonOnTheTenantManagementWillDownloadTenantLog_Integration_25773()
        {
            SetTestCaseParams("client2", "project2");
        }
    }
}
