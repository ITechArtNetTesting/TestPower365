using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class MenuElement: ElementBase
    {
        private static readonly By _locator = By.Id("hamburger");

        private By _clientsComboBox = By.ClassName("dropdown");
        
        private string _menuSelectionLocatorFormat = "//span[contains(text(), '{0}')]";
        private string _clientLocatorFormat = "//div[contains(@class, 'open')]//a[contains(text(), '{0}')]";

        //PROJECTS
        private const string EN_ALLPROJECTS = "All Projects";
        private const string EN_NEWPROJECT = "New Project";

        //ADMIN
        private const string EN_CLIENTS = "Clients";
        private const string EN_LICENSES = "Licenses";
        private const string EN_USERS = "Users";
        private const string EN_PROBES = "Probes";
        private const string EN_RESOURCES = "Resources";

        //LANGUAGE
        //TODO Add Language Selection

        //GENERAL
        private const string EN_HELP = "Help";
        private const string EN_SIGNOUT = "Sign Out";


        public MenuElement(IWebDriver webDriver)
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
            if(!IsElementVisible(_clientsComboBox, 1))
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

        public void ClickUsers()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public HomePage ClickSignOut()
        {
            return ClickMenuSelection<HomePage>(EN_SIGNOUT);
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
