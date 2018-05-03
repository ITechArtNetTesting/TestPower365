using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ProjectListPage : PageBase
    {
        private static readonly By _locator = By.Id("projectsContainer");

        private readonly By _newProjectButton = By.XPath("//div[contains(@class, 'topnav')]//a[contains(@href, 'Project/Create')]");

        //@@@ Better selector
        private string _projectSelectFormat = "//span[contains(text(), '{0}')][contains(@class, 'notranslate')]";
        
        public ProjectListPage(IWebDriver webDriver)
            : base(_locator, webDriver) { }
     
        public ProjectDetailsPage ClickProjectByName(string projectName)
        {
            var projectLink = By.XPath(string.Format(_projectSelectFormat, projectName));
            return ClickElementBy<ProjectDetailsPage>(projectLink);
        }

        public EditProjectPage ClickNewProject()
        {
            return ClickElementBy<EditProjectPage>(_newProjectButton);
        }
    }
}
