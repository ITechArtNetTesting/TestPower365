using Product.WorkElements.Pages;
using System;
using TP365Framework.Steps;

namespace Product.WorkElements.Steps
{
    public class LoginSteps : BaseSteps
    {
        LoginPage loginPage { get; set; }

        public void LoginAndSelectRole(string login, string password, string client)
        {
            LogIn(login, password);
            
            //User.AtTenantRestructuringForm().OpenMainMenu();
            //User.AtTenantRestructuringForm().AtMainMenu().SelectRole(role);
            //User.AtTenantRestructuringForm().GoToProjects();
        }

        public void SelectProject(string projectName)
        {
            throw new NotImplementedException();
        }

        public void LogIn(string login, string password)
        {
            loginPage.ClickSignIn();
            loginPage.SetLogin(login);
            loginPage.NextClick();
            loginPage.WaitElementShows(loginPage.passwordInputText);
            loginPage.SetPassword(password);
            loginPage.NextClick();


            try
            {
                // User.AtTenantRestructuringForm().WaitForProjectsContainer();
            }
            catch (Exception)
            {
                // Log.Info("Login failed");
                throw;
            }
        }

    }
}
