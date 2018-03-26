using TestFramework.IoC;
using TestFramework.Steps.Interfaces;

namespace TestFramework.Users
{
    public class Tester
    {
        public IStartPageSteps AtStartPage()
        {
            return DependencyResolver.For<IStartPageSteps>();
        }
        public IMicrosoftLoginPageSteps AtMicrosoftLoginPage()
        {
            return DependencyResolver.For<IMicrosoftLoginPageSteps>();
        }
        public IRightBarSteps AtRightBar()
        {
            return DependencyResolver.For<IRightBarSteps>();
        }
    }
}
