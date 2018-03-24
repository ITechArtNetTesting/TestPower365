using TestFramework.IoC;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Steps.Interfaces;

namespace TestFramework.Steps
{
    public class StartPageSteps : IStartPageSteps
    {
        IStartPage startPage = DependencyResolver.For<IStartPage>();        

        public void SignIn()
        {
            startPage.ClickSignIn();
        }
    }
}
