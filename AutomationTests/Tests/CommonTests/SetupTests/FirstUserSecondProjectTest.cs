using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;

namespace Product.Tests.CommonTests.SetupTests
{
	[TestClass]
	public class FirstUserSecondProjectTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("Setup")]
		public void SetupFirstUserSecondProject()
		{
            LoginAndSelectRole(configurator.GetUserLogin("client1"),
                                configurator.GetPassword("client1"),
                                configurator.GetClient("client1"));
          
            AddMailOnlyProject(configurator.GetProjectName("client1", "project2"),
                configurator.GetTenantValue("T3->T4", "source", "user"),
                configurator.GetTenantValue("T3->T4", "source", "password"),
                configurator.GetTenantValue("T3->T4", "target", "user"),
                configurator.GetTenantValue("T3->T4", "target", "password"),
                configurator.GetFileName("client1", "project2", "file1"));
            User.AtProjectOverviewForm().OpenUsersList();
        }

	    public void SetupTest()
	    {
            LoginAndSelectRole(configurator.GetValueByXpath("//metaname[text()='client1']/..//user"),
            configurator.GetValueByXpath("//metaname[text()='client1']/..//password"),
            configurator.GetValueByXpath("//metaname[text()='client1']/../name"));
            User.AtTenantRestructuringForm().AddProjectClick();
            User.AtChooseYourProjectTypeForm().ChooseMailOnly();
            User.AtChooseYourProjectTypeForm().GoNext();
            User.AtSetProjectNameForm().SetName(configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//name"));
            User.AtSetProjectNameForm().GoNext();
            User.AtSetProjectDescriptionForm().SetDescription(StringRandomazer.MakeRandomString(20));
            User.AtSetProjectDescriptionForm().GoNext();

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(configurator.GetTenantValue("T3->T4", "source", "user"), configurator.GetTenantValue("T3->T4", "source", "password"));
            
            Driver.GetDriver(driver.GetDriverKey()).SwitchTo().Window(store.MainHandle);
            User.AtAddTenantsForm().WaitForTenantAdded(1);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(configurator.GetTenantValue("T3->T4", "target", "user"), configurator.GetTenantValue("T3->T4", "target", "password"));

            Driver.GetDriver(driver.GetDriverKey()).SwitchTo().Window(store.MainHandle);
            User.AtAddTenantsForm().WaitForTenantAdded(2);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(configurator.GetTenantValue("T1->T2", "target", "user"), configurator.GetTenantValue("T1->T2", "target", "password"));
            
            Driver.GetDriver(driver.GetDriverKey()).SwitchTo().Window(store.MainHandle);
            User.AtAddTenantsForm().WaitForTenantAdded(3);

            User.AtAddTenantsForm().GoNext();
            User.AtUploadFilesForm().DownloadSample();
            configurator.CheckUserMatchFileIsDownloaded();
            User.AtUploadFilesForm().SelectFile(configurator.GetFileName("client1", "project2", "file1"));
            User.AtUploadFilesForm().SelectFile(configurator.GetFileName("client1", "project2", "file1"));
            User.AtUploadFilesForm().WaitUntillFileUploaded();
            User.AtUploadFilesForm().GoNext();
            User.AtUploadedUsersForm().GoNext();
            User.AtAlmostDoneForm().VerifySubmitIsEnabled();
            User.AtAlmostDoneForm().GoBack();
            User.AtEnablePublicFoldersForm().GoBack();
            User.AtUploadedUsersForm().GoBack();
            User.AtUploadFilesForm().GoBack();
            User.AtAddTenantsForm().RemoveTenant(configurator.GetTenantValue("T1->T2", "target", "name"));
            User.AtAddTenantsForm().GoNext();
            User.AtKeepUsersForm().SelectKeepExisting();
            User.AtKeepUsersForm().GoNext();
            User.AtSyncScheduleForm().GoNext();
            User.AtAlmostDoneForm().GoNext();
            User.AtProjectOverviewForm().OpenUsersList();
        }
    }
}