using OpenQA.Selenium;
using BinaryTree.Power365.AutomationFramework.Pages;
using System;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public class CommonWorkflow : WorkflowBase
    {
        private readonly HomePage _homePage;
        
        public CommonWorkflow(HomePage homePage, IWebDriver webDriver) 
            : base(homePage, webDriver)
        {
            _homePage = homePage;
        }

        public CommonWorkflow SingIn(string username, string password)
        {
            var o365SignIn = _homePage.ClickSignIn();
            CurrentPage = o365SignIn.SignIn(username, password);
            return this;
        }

        public CommonWorkflow ClientSelect(string client)
        {
            var pageWithMenu = GetCurrentPage<PageBase>();
            CurrentPage = pageWithMenu.Menu.SelectClient(client);
            return this;
        }
        
        public CommonWorkflow ProjectSelect(string project)
        {
            var projectListPage = GetCurrentPage<ProjectListPage>();
            CurrentPage = projectListPage.ClickProjectByName(project);
            return this;
        }

        public CommonWorkflow UsersEdit()
        {
            var projectDetailsPage = GetCurrentPage<ProjectDetailsPage>();
            CurrentPage = projectDetailsPage.ClickUsersEdit();
            return this;
        }
        
        public CommonWorkflow UsersPerformAction(string user, ActionType action, bool isYes = true)
        {
            if (action == ActionType.Rollback)
                throw new Exception("User method UsersPerformRollback.");
            if (action == ActionType.AddToWave)
                throw new Exception("User method UsersPerformAddToWave.");

            var manageUsersPage = GetCurrentPage<ManageUsersPage>();
            manageUsersPage.Users.ClickRowByValue(user);
            manageUsersPage.PerformAction(action);
            manageUsersPage.ConfirmAction(isYes);
            return this;
        }

        public CommonWorkflow WavesPerformAction(string wave, ActionType action, bool isYes = true)
        {
            if (action == ActionType.Rollback)
                throw new Exception("User method UsersPerformRollback.");
            if (action == ActionType.AddToWave)
                throw new Exception("User method UsersPerformAddToWave.");

            var manageUsersPage = GetCurrentPage<ManageUsersPage>();
            manageUsersPage.Waves.ClickRowByValue(wave);
            manageUsersPage.PerformAction(action);
            manageUsersPage.ConfirmAction(isYes);
            return this;
        }
        public CommonWorkflow UsersFindAndPerformAction(string user, ActionType action, bool isYes = true)
        {
            var manageUsersPage = GetCurrentPage<ManageUsersPage>();
            manageUsersPage.PerformSearch(user);
            return UsersPerformAction( user, action, isYes );
        }

        public CommonWorkflow UsersPerformRollback(string user, bool resetPermissions = true)
        {
            var manageUsersPage = GetCurrentPage<ManageUsersPage>();
            manageUsersPage.Users.ClickRowByValue(user);
            manageUsersPage.PerformAction(ActionType.Rollback);
            manageUsersPage.ConfirmRollback(resetPermissions);
            return this;
        }

        public CommonWorkflow UsersPerformAddToWave(string user, string waveName )
        {
            var manageUsersPage = GetCurrentPage<ManageUsersPage>();
            manageUsersPage.PerformSearch(user);
            manageUsersPage.Users.ClickRowByValue(user);
            manageUsersPage.PerformAction(ActionType.AddToWave);
            manageUsersPage.SelectWave(waveName);         
            return this;
        }

        public CommonWorkflow UsersValidateState(string user, StateType state)
        {
            var manageUsersPage = GetCurrentPage<ManageUsersPage>();

            int timeoutSec = 5;
            int pollIntervalSec = 0;

            switch (state)
            {
                case StateType.Stopping:
                case StateType.Preparing:
                case StateType.Syncing:
                case StateType.RollbackInProgress:
                case StateType.Finalizing:
                    timeoutSec = 30;
                    pollIntervalSec = 5;
                    break;
                case StateType.Prepared:                
                case StateType.Synced:
                case StateType.RollbackCompleted:
                case StateType.Complete:
                    timeoutSec = 30 * 60;
                    pollIntervalSec = 60;
                    break;
                case StateType.NoMatch:
                default:
                    break;
            }

            if (!manageUsersPage.IsUserState(user, state, timeoutSec, pollIntervalSec))
                throw new Exception(string.Format("State of '{0}' was not reached within '{1}' seconds", state.GetDescription(), timeoutSec));
            return this;
        }

        public CommonWorkflow PublicFoldersEdit()
        {
            var projectDetailsPage = GetCurrentPage<ProjectDetailsPage>();
            CurrentPage = projectDetailsPage.ClickPublicFoldersEdit();
            return this;
        }

        public CommonWorkflow PublicFoldersPerformAction(string folder, ActionType action, bool isYes = true)
        {
            var managePublicfoldersPage = GetCurrentPage<ManagePublicFoldersPage>();
            managePublicfoldersPage.PublicFolders.ClickRowByValue(folder);
            managePublicfoldersPage.PerformAction(action);
            managePublicfoldersPage.ConfirmAction(isYes);
            return this;
        }
    }
}
