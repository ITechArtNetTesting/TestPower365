using System;
using System.Threading;
using Product.Framework;
using Product.Framework.Enums;

namespace Product.Tests.CommonTests
{
	public class LoginAndConfigureTest : BaseTest
	{

		protected void LoginAndSelectRole(string login, string password, string role)
		{
            LogIn(login, password);
            //NOTE: Temp solution to avoid 1 symbol in role textbox
            
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
                User.AtLandingForm().ClickT2T();
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
            User.AtOffice365LoginForm().AccertClick();
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

            Office365TenantAuthorization(targetTenant, targetPassword);

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

        public void PerformAction(string mailbox, ActionType action)
        {
            Log.Debug(string.Format("PerformAction: '{0}' action '{1}'", mailbox, action));

            if (action == ActionType.Rollback)
                throw new NotImplementedException("Rollback requires special confirmation");

            User.AtUsersForm().PerformSearch(mailbox);
            User.AtUsersForm().SelectEntryBylocator(mailbox);
            User.AtUsersForm().SelectAction(action);
            User.AtUsersForm().Apply();
            User.AtUsersForm().ConfirmAction();
        }

        public void PerformActionAndWaitForState(string mailbox, ActionType action, State state, int timeout = 5000, int pollIntervalSec = 0)
        {
            PerformAction(mailbox, action);
            WaitForState(mailbox, state, timeout, pollIntervalSec);
        }

        public void WaitForState(string mailbox, State state, int timeout = 5000, int pollIntervalSec = 0)
        {
            Log.Debug(string.Format("WaitForState: '{0}' waiting for state '{1}' [Timeout]: '{2}' [pollInterval] = '{3}'", mailbox, state, timeout, pollIntervalSec));
            User.AtUsersForm().PerformSearch(mailbox);
            User.AtUsersForm().WaitForState(mailbox, state, timeout, pollIntervalSec);
        }

        public void WaitForAnyState(string mailbox, State[] states, int timeout = 5000, int pollIntervalSec = 0)
        {
            Log.Debug(string.Format("WaitForAnyState: '{0}' waiting for any state '{1}' [Timeout]: '{2}' [pollInterval] = '{3}'", mailbox, string.Join(", ", states), timeout, pollIntervalSec));
            User.AtUsersForm().PerformSearch(mailbox);
            User.AtUsersForm().WaitForAnyState(mailbox, states, timeout, pollIntervalSec);
        }
    }
}