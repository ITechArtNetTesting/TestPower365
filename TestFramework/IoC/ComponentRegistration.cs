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


            //Pages
            kernel.Register(
                Component.For<IStartPage>()
                         .ImplementedBy<StartPage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                            Component.For<IProjectOverviewPage>()
                                     .ImplementedBy<ProjectOverviewPage>()
                                     .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IProjectGroupsPage>()
                         .ImplementedBy<ProjectGroupsPage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<ITenantManagementPage>()
                         .ImplementedBy<TenantManagementPage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
               Component.For<IPublicFolderMigrationPage>()
                        .ImplementedBy<PublicFolderMigrationPage>()
                        .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IMicrosoftLoginPage>()
                         .ImplementedBy<MicrosoftLoginPage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
               Component.For<IMicrosoftLoginPageWindow>()
                        .ImplementedBy<MicrosoftLoginPageWindow>()
                        .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IRightBar>()
                         .ImplementedBy<RightBar>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IProjectCreatePage>()
                         .ImplementedBy<ProjectCreatePage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);

            kernel.Register(
                Component.For<IProjectsListPage>()
                         .ImplementedBy<ProjectsListPage>()
                         .Interceptors(InterceptorReference.ForType<PageLoggingInterceptor>()).Anywhere);


            //Steps
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


            kernel.Register(
                Component.For<IRightBarSteps>()
                         .ImplementedBy<RightBarSteps>()
                         .Interceptors(InterceptorReference.ForType<StepsLoggingInterceptor>()).Anywhere);
        }
    }
}
