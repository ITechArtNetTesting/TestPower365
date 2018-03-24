using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.IoC;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Steps.Interfaces;

namespace TestFramework.Steps
{
    public class MicrosoftLoginPageSteps : IMicrosoftLoginPageSteps
    {
        IMicrosoftLoginPage microsoftLoginPage= DependencyResolver.For<IMicrosoftLoginPage>();
        public void LogIn(string email,string password)
        {
            microsoftLoginPage.SendKeysToEmailPhoneOrSkypeInput(email);
            microsoftLoginPage.ClickNextButton();
            microsoftLoginPage.SendKeysToPasswordInput(password);
            microsoftLoginPage.ClickSignInButton();
            microsoftLoginPage.ClickYesToStaySignedButton();
        }
    }
}
