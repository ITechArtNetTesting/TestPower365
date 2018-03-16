using ITechArtTestFramework.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Pages;

namespace Tests.Steps
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
            loginPage.Wait(loginPage.passwordInputText);
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
