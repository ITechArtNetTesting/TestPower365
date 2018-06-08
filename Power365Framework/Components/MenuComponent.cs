using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Components
{
    public class MenuComponent: Element
    {
        private static readonly By _locator = By.Id("hamburger");

        private By _clientsComboBox = By.ClassName("dropdown");
        
        private string _menuSelectionLocatorFormat = "//*[@class='menulink']//span[contains(text(), '{0}')]";
        private string _clientLocatorFormat = "//div[contains(@class, 'open')]//a[contains(text(), '{0}')]";

        //PROJECTS
        private const string EN_ALLPROJECTS = "All Projects";
        private const string EN_NEWPROJECT = "New Project";

        //ADMIN
        private const string EN_CLIENTS = "Clients";
        private const string EN_LICENSES = "Licenses";
        
        private const string EN_PROBES = "Probes";
        private const string EN_RESOURCES = "Resources";

        //LANGUAGE
        //TODO Add Language Selection

        //GENERAL
        private const string EN_HELP = "Help";
        private const string EN_SIGNOUT = "Sign Out";
        private const string EN_ERRORS = "Errors";

        //MANAGE
        private const string EN_MIGRATION_PROFILES = "Migration Profiles";
        private const string EN_USERS = "Users";

        public MenuComponent(IWebDriver webDriver)
            :base(_locator, webDriver) { }

        public void OpenMenu()
        {
            if (IsElementVisible(_locator))
                ClickElementBy(_locator);
            if (!IsElementVisible(_clientsComboBox))
                throw new Exception("Could not Open Menu");
        }
        public ProjectListPage SelectClient(string clientName)
        {
            if (!IsElementVisible(_clientsComboBox, 1))
                OpenMenu();

            ClickElementBy(_clientsComboBox);

            var clientSelection = By.XPath(string.Format(_clientLocatorFormat, clientName));
            return ClickElementBy<ProjectListPage>(clientSelection);
        }
                
        public ProjectListPage ClickAllProjects()
        {
            throw new NotImplementedException();
        }

        public EditProjectPage ClickNewProject()
        {
            return ClickMenuSelection<EditProjectPage>(EN_NEWPROJECT);
        }

        public ClientsPage ClickClients()
        {
            throw new NotImplementedException();
        }

        public LicensesPage ClickLicenses()
        {
            throw new NotImplementedException();
        }

        public ManageUsersPage ClickUsers()
        {
            OpenMenu();
            var clientSelection = By.XPath(string.Format(_menuSelectionLocatorFormat, EN_USERS));
            return ClickElementBy<ManageUsersPage>(clientSelection,15);
        }

        public ProbesPage ClickProbes()
        {
            throw new NotImplementedException();
        }

        public ResourcesPage ClickResources()
        {
            throw new NotImplementedException();
        }

        public HelpPage ClickHelp()
        {
            OpenMenu();
            IsElementVisible(By.XPath(string.Format(_menuSelectionLocatorFormat, EN_HELP)));
            return ClickPopupElementBy<HelpPage>(By.XPath(string.Format(_menuSelectionLocatorFormat, EN_HELP))).Page;          
        }

        public HomePage ClickSignOut()
        {
            return ClickMenuSelection<HomePage>(EN_SIGNOUT);
        }

        public ErrorsPage ClickErrors()
        {
            OpenMenu();
            IsElementVisible(By.XPath(string.Format(_menuSelectionLocatorFormat, EN_ERRORS)));
           // var clientSelection = By.XPath(string.Format(_menuSelectionLocatorFormat, EN_ERRORS));
            return ClickElementBy<ErrorsPage>(By.XPath(string.Format(_menuSelectionLocatorFormat, EN_ERRORS)));
        }

        public MigrationProfilesPage  ClickMigrationProfiles()
        {
            return ClickMenuSelection<MigrationProfilesPage>(EN_MIGRATION_PROFILES);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of page that will be navigated to.</typeparam>
        /// <param name="selection">Page menu text.</param>
        /// <returns>Type of page that will be navigated to.</returns>
        private T ClickMenuSelection<T>(string selection)
            where T: PageBase
        {
            if (!IsElementVisible(_clientsComboBox))
                OpenMenu();

            var menuSelection = By.XPath(string.Format(_menuSelectionLocatorFormat, selection));
            return ClickElementBy<T>(menuSelection);
        }
    }
}
