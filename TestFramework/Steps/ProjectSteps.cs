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

        public void AddNewProject(string testName, string sourceTenant, string sourcePassword, string targetTenant, string targetPassword, string fileName)
        {
            projectsListPage.ClickNewProjectButton();
            projectCreatePage.ChooseEmailFromFileProjectType();
            projectCreatePage.ClickNextButton();
            projectCreatePage.CallProjectWithKeys(testName);
            projectCreatePage.ClickNextButton();
            projectCreatePage.SendRandomKeysToDescription();
            projectCreatePage.ClickNextButton();
            projectCreatePage.ClickAddTenantButton();          
        }
    }
}
