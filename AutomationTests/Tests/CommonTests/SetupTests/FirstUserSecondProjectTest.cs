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
		    LoginAndSelectRole(RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user"),
		        RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password"),
		        RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name"));

		    AddMailOnlyProject(RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//name"),
		        RunConfigurator.GetTenantValue("T3->T4", "source", "user"),
		        RunConfigurator.GetTenantValue("T3->T4", "source", "password"),
		        RunConfigurator.GetTenantValue("T3->T4", "target", "user"),
		        RunConfigurator.GetTenantValue("T3->T4", "target", "password"),
		        RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//metaname[text()='file1']/..//filename"));
		    User.AtProjectOverviewForm().OpenUsersList();
        }

	    public void SetupTest()
	    {
            LoginAndSelectRole(RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user"),
            RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password"),
            RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name"));
            User.AtTenantRestructuringForm().AddProjectClick();
            User.AtChooseYourProjectTypeForm().ChooseMailOnly();
            User.AtChooseYourProjectTypeForm().GoNext();
            User.AtSetProjectNameForm().SetName(RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//name"));
            User.AtSetProjectNameForm().GoNext();
            User.AtSetProjectDescriptionForm().SetDescription(StringRandomazer.MakeRandomString(20));
            User.AtSetProjectDescriptionForm().GoNext();

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T3->T4", "source", "user"), RunConfigurator.GetTenantValue("T3->T4", "source", "password"));
            
            Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
            User.AtAddTenantsForm().WaitForTenantAdded(1);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T3->T4", "target", "user"), RunConfigurator.GetTenantValue("T3->T4", "target", "password"));

            Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
            User.AtAddTenantsForm().WaitForTenantAdded(2);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T1->T2", "target", "user"), RunConfigurator.GetTenantValue("T1->T2", "target", "password"));
            
            Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
            User.AtAddTenantsForm().WaitForTenantAdded(3);

            User.AtAddTenantsForm().GoNext();
            User.AtUploadFilesForm().DownloadSample();
            RunConfigurator.CheckUserMatchFileIsDownloaded();
            User.AtUploadFilesForm().SelectFile(RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//metaname[text()='file1']/..//filename"));
            User.AtUploadFilesForm().WaitUntillFileUploaded();
            User.AtUploadFilesForm().GoNext();
            User.AtUploadedUsersForm().GoNext();
            User.AtAlmostDoneForm().VerifySubmitIsEnabled();
            User.AtAlmostDoneForm().GoBack();
            User.AtEnablePublicFoldersForm().GoBack();
            User.AtUploadedUsersForm().GoBack();
            User.AtUploadFilesForm().GoBack();
            User.AtAddTenantsForm().RemoveTenant(RunConfigurator.GetTenantValue("T1->T2", "target", "name"));
            User.AtAddTenantsForm().GoNext();
            User.AtKeepUsersForm().SelectKeepExisting();
            User.AtKeepUsersForm().GoNext();
            User.AtSyncScheduleForm().GoNext();
            User.AtAlmostDoneForm().GoNext();
            User.AtProjectOverviewForm().OpenUsersList();
        }
    }
}