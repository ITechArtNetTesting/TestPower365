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
    public class ProjectSteps : IProjectSteps
    {
        IProjectsListPage projectsListPage = DependencyResolver.For<IProjectsListPage>();
        IProjectCreatePage projectCreatePage = DependencyResolver.For<IProjectCreatePage>();
        IMicrosoftLoginPageWindow microsoftPageWindow = DependencyResolver.For<IMicrosoftLoginPageWindow>();
        IProjectOverviewPage projectOverviewPage = DependencyResolver.For<IProjectOverviewPage>();
        IProjectGroupsPage projectGroupsPage = DependencyResolver.For<IProjectGroupsPage>();

        public void AddNewEmailFromFileProject(string testName, string sourceTenant, string sourcePassword, string targetTenant, string targetPassword, string fileName)
        {
            projectsListPage.ClickNewProjectButton();
            projectCreatePage.ChooseEmailFromFileProjectType();
            projectCreatePage.ClickNextButton();
            projectCreatePage.CallProjectWithKeys(testName);
            projectCreatePage.ClickNextButton();
            projectCreatePage.SendRandomKeysToDescription();
            projectCreatePage.ClickNextButton();
            projectCreatePage.ClickAddTenantButton();
            microsoftPageWindow.ClickUseAnotherAccount();
            microsoftPageWindow.SendKeysToEmailInput(sourceTenant);
            microsoftPageWindow.ClickNextButton();
            microsoftPageWindow.SendKeysToPasswordInput(sourcePassword);
            microsoftPageWindow.ClickNextButton();
            microsoftPageWindow.ClickNextButton();
            projectCreatePage.ClickAddTenantButton();
            microsoftPageWindow.ClickUseAnotherAccount();
            microsoftPageWindow.SendKeysToEmailInput(targetTenant);
            microsoftPageWindow.ClickNextButton();
            microsoftPageWindow.SendKeysToPasswordInput(targetPassword);
            microsoftPageWindow.ClickNextButton();
            microsoftPageWindow.ClickNextButton();
            projectCreatePage.ClickNextButton();
            projectCreatePage.UploadFile(fileName);
            projectCreatePage.ClickNextButton();
            projectCreatePage.ClickNextButton();
            projectCreatePage.ClickNextButton();
            projectCreatePage.ClickSubmitButton();
        }

        public void AddNewEmailWithDiscoveryProject(string projectName, string sourceUserLogin, string sourceUserPassword, string targetUserLogin, string targetUserPassword)
        {
            projectsListPage.ClickNewProjectButton();
            projectCreatePage.ChooseEmailWithDiscoveryProjectType();
            projectCreatePage.ClickNextButton();
            projectCreatePage.CallProjectWithKeys(projectName);
            projectCreatePage.ClickNextButton();
            projectCreatePage.SendRandomKeysToDescription();
            projectCreatePage.ClickNextButton();
            projectCreatePage.ClickAddTenantButton();
            microsoftPageWindow.ClickUseAnotherAccount();
            microsoftPageWindow.SendKeysToEmailInput(sourceUserLogin);
            microsoftPageWindow.ClickNextButton();
            microsoftPageWindow.SendKeysToPasswordInput(sourceUserPassword);
            microsoftPageWindow.ClickNextButton();
            microsoftPageWindow.ClickNextButton();
            projectCreatePage.ClickAddTenantButton();
            microsoftPageWindow.ClickUseAnotherAccount();
            microsoftPageWindow.SendKeysToEmailInput(targetUserLogin);
            microsoftPageWindow.ClickNextButton();
            microsoftPageWindow.SendKeysToPasswordInput(targetUserPassword);
            microsoftPageWindow.ClickNextButton();
            microsoftPageWindow.ClickNextButton();


        }

        public void ViewGroups(string projectName,string groupName)
        {
            projectsListPage.ChooseProjectByName(projectName);
            projectOverviewPage.ClickGroupsEditButton();
            projectGroupsPage.SearchGroupByName(groupName);
            projectGroupsPage.ClickTargetGroup();
        }
    }
}
