using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using TestFramework.Interceptors;
using TestFramework.PageObjects;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Steps;
using TestFramework.Steps.Interfaces;

namespace TestFramework.IoC
{
    public class ComponentRegistration:IRegistration
    {
        public void Register(IKernelInternal kernel)
        {
            kernel.Register(
                Component.For<PageLoggingInterceptor>()
                    .ImplementedBy<PageLoggingInterceptor>());

            kernel.Register(
                Component.For<StepsLoggingInterceptor>()
                .ImplementedBy<StepsLoggingInterceptor>());

            kernel.Register(
                Component.For<IStartPage>()
                         .ImplementedBy<StartPage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IMicrosoftLoginPage>()
                         .ImplementedBy<MicrosoftLoginPage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IProjectsListPage>()
                         .ImplementedBy<ProjectsListPage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IStartPageSteps>()
                         .ImplementedBy<StartPageSteps>()
                         .Interceptors(InterceptorReference.ForType<StepsLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IMicrosoftLoginPageSteps>()
                         .ImplementedBy<MicrosoftLoginPageSteps>()
                         .Interceptors(InterceptorReference.ForType<StepsLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IProjectSteps>()
                         .ImplementedBy<ProjectSteps>()
                         .Interceptors(InterceptorReference.ForType<StepsLoggingInterceptor>()).Anywhere);
        }
    }
}
