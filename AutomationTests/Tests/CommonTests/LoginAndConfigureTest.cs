using System;
using System.Threading;
using Product.Framework;

namespace Product.Tests.CommonTests
{
	public class LoginAndConfigureTest : BaseTest
	{

		protected void LoginAndSelectRole(string login, string password, string role)
		{
            LogIn(login, password);
		    //NOTE: Temp solution to avoid 1 symbol in role textbox
            Thread.Sleep(2000);
			User.AtTenantRestructuringForm().OpenMainMenu();
			User.AtTenantRestructuringForm().AtMainMenu().SelectRole(role);
            User.AtTenantRestructuringForm().GoToProjects();
		}

	    protected void LogIn(string login, string password)
	    {
	        User.AtMainForm().ClickSignIn();
            Office365Login(login, password);
           
	        try
	        {
	            User.AtTenantRestructuringForm().WaitForProjectsContainer();
            }
	        catch (Exception)
	        {
                Log.Info("Login failed");
                throw;
	        }
        }

        protected void Office365Login(string login, string password)
        {
            User.AtOffice365LoginForm().SetLogin(login);
            User.AtOffice365LoginForm().NextClick();
            User.AtOffice365LoginForm().SetPassword(password);
            User.AtOffice365LoginForm().NextClick();
        }

        protected void Office365TenantAuthorization(string login, string password)
        {
            User.AtOffice365LoginForm().UseAnotherAccountClick();
            Office365Login(login, password);
            User.AtOffice365LoginForm().NextClick();
        }

	    protected void SelectProject(string projectName)
		{
			User.AtTenantRestructuringForm().SelectProject(projectName);
		}

		protected void AddMailOnlyProject(string testName, string sourceTenant, string sourcePassword, string targetTenant,
			string targetPassword, string fileName)
		{
			User.AtTenantRestructuringForm().AddProjectClick();
			User.AtChooseYourProjectTypeForm().ChooseMailOnly();
			User.AtChooseYourProjectTypeForm().GoNext();
			User.AtSetProjectNameForm().SetName(testName);
			User.AtSetProjectNameForm().GoNext();
			User.AtSetProjectDescriptionForm().SetDescription(StringRandomazer.MakeRandomString(20));
			User.AtSetProjectDescriptionForm().GoNext();

			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();

            Office365TenantAuthorization(sourceTenant, sourcePassword);
            
            Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();

            Office365TenantAuthorization(sourceTenant, sourcePassword);

            Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
			User.AtAddTenantsForm().GoNext();
			User.AtUploadFilesForm().SelectFile(fileName);
			User.AtUploadFilesForm().WaitUntillFileUploaded();
			User.AtUploadFilesForm().GoNext();
			User.AtUploadedUsersForm().GoNext();
			//User.AtEnablePublicFoldersForm().SetNo();
			//User.AtEnablePublicFoldersForm().GoNext();
			User.AtSyncScheduleForm().GoNext();
			User.AtAlmostDoneForm().GoNext();
		}


		public void LoginAndReloadFile(string login, string password, string role, string project, string file)
		{
			LoginAndSelectRole(login,
			password,
			role);
			SelectProject(project);
		    try
		    {
		        User.AtProjectOverviewForm().OpenProjectDetails();
		    }
		    catch (Exception)
		    {
		        Log.Info("Selecting project failed");
		        SelectProject(project);
		        User.AtProjectOverviewForm().OpenProjectDetails();
		    }
            ReplaceFile(file);
		}

	    public void LoginAndReloadFile(string login, string password, string project, string file)
	    {
	        LogIn(login, password);
	        SelectProject(project);
	        try
	        {
	            User.AtProjectOverviewForm().OpenProjectDetails();
            }
	        catch (Exception)
	        {
	            Log.Info("Selecting project failed");
	            SelectProject(project);
	            User.AtProjectOverviewForm().OpenProjectDetails();
            }
	        ReplaceFile(file);
	    }

        public void ReplaceFile(string fileName)
		{
			User.AtSetProjectNameForm().GoNext();
			User.AtSetProjectDescriptionForm().GoNext();
			User.AtAddTenantsForm().GoNext();
			User.AtKeepUsersForm().UploadNewUsers();
			User.AtKeepUsersForm().GoNext();
			User.AtUploadFilesForm().SelectFile(fileName);
			User.AtUploadFilesForm().WaitUntillFileUploaded();
			User.AtUploadFilesForm().GoNext();
			User.AtUploadedUsersForm().GoNext();
			//User.AtEnablePublicFoldersForm().SetNo();
			//User.AtEnablePublicFoldersForm().GoNext();
			User.AtSyncScheduleForm().GoNext();
			User.AtAlmostDoneForm().GoNext();
		}
	}
}