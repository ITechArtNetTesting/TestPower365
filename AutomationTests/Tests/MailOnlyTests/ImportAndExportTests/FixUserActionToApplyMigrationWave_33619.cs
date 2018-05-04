using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailOnlyTests.ImportAndExportTests
{
    [TestClass]
    public class FixUserActionToApplyMigrationWave_TC33619 : LoginAndConfigureTest
    {

        string login;
        string password;
        string client;
        string projectName;
        string fileName2;

       [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        
        [TestInitialize()]
        public void Initialize()
        {
            login = RunConfigurator.GetUserLogin("client1");
            password = RunConfigurator.GetPassword("client1");
            client = RunConfigurator.GetClient("client1");
            projectName = RunConfigurator.GetProjectName("client1", "project3");
            fileName2 = RunConfigurator.GetFileName("client1","project3","file2");

        }

        [TestMethod]
        [TestCategory("MailOnly")]
        [TestCategory("Import")]
        public void FixUserActionToApplyMigrationWave_33619()
        {
            
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().OpenImportDialog();
                //Import new file
                User.AtUsersForm().ChooseFile(fileName2);
                User.AtUsersForm().ConfirmUploading();
                User.AtUsersForm().AssertImportSuccessful();
                User.AtUsersForm().СloseSuccessfulImportWindow();
                  //Verify count Wave
                User.AtUsersForm().VerifyLinesCountAndProperties("Wave1", 6);
                User.AtUsersForm().VerifyLinesCountAndProperties("Wave2", 4);
        }
    }
}
