using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BinaryTree.Power365DirSync.Test.Steps
{
    [Binding]
    public class SignInSteps: StepsBase
    {
        public SignInSteps(FeatureContext featureContext, ScenarioContext scenarioContext, AutomationContext automationContext)
            :base(featureContext, scenarioContext, automationContext) { }

        [Given(@"I have opened the website")]
        public void GivenIHaveOpenedTheWebsite()
        {
            var homePage = AutomationContext.Browser.Navigate<HomePage>(AutomationContext.Settings.BaseUrl);
            homePage.Validate();
            ScenarioContext.Set(homePage);
        }

        [When(@"I authenticate with ""(.*)""")]
        public void WhenIAuthenticateWith(string credentialReference)
        {
            //var creds = AutomationContext.GetCredential(credentialReference);

            //string username = creds.Username;
            //string password = creds.Password;

            //var homePage = ScenarioContext.Get<HomePage>();

            //var o365SignInPage = homePage.ClickSignIn();
            //var projectListPage = o365SignInPage.SignIn(username, password);

            //ScenarioContext.Set(projectListPage);
        }

        [Then(@"the Projects List should be visible")]
        public void ThenTheProjectsListShouldBeVisible()
        {
            var projectListPage = ScenarioContext.Get<ProjectListPage>();
            projectListPage.Validate();
        }
        
        [When(@"I open the menu")]
        public void WhenIOpenTheMenu()
        {
            var projectListPage = ScenarioContext.Get<ProjectListPage>();
            projectListPage.Menu.OpenMenu();
        }

        [When(@"I select a client ""(.*)""")]
        public void WhenISelectAClient(string clientName)
        {
            var projectListPage = ScenarioContext.Get<ProjectListPage>();
            projectListPage = projectListPage.Menu.SelectClient(clientName);

            ScenarioContext.Set(projectListPage);
        }

        [Then(@"the client Projects list should be visible")]
        public void ThenTheClientProjectsListShouldBeVisible()
        {
            var projectListPage = ScenarioContext.Get<ProjectListPage>();
            projectListPage.Validate();
        }

        [Given(@"I have signed into the website as ""(.*)""")]
        public void GivenIHaveSignedIntoTheWebsiteAs(string credentialReference)
        {
            var homePage = AutomationContext.Browser.Navigate<HomePage>(AutomationContext.Settings.BaseUrl);
            ScenarioContext.Set(homePage);

            //var creds = AutomationContext.GetCredential(credentialReference);

            //string username = creds.Username;
            //string password = creds.Password;
            
            var o365SignInPage = homePage.ClickSignIn();
            //var projectListPage = o365SignInPage.SignIn(username, password);

            //ScenarioContext.Set(projectListPage);
        }

        [Given(@"I have clicked ""(.*)"" link in the menu")]
        public void GivenIHaveClickedLinkInTheMenu(string menuLink)
        {
            var projectListPage = ScenarioContext.Get<ProjectListPage>();

            switch (menuLink.ToLowerInvariant())
            {
                case "all projects":
                    break;
                case "new project":
                    var editProjectsPage = projectListPage.Menu.ClickNewProject();
                    ScenarioContext.Set(editProjectsPage);
                    break;
            }
        }

        [Then(@"the ""(.*)"" page should be displayed")]
        public void ThenThePageShouldBeDisplayed(string page)
        {
            switch (page.ToLowerInvariant())
            {
                case "edit project":
                    var editProjectPage = ScenarioContext.Get<EditProjectPage>();
                    Assert.IsNotNull(editProjectPage);
                    editProjectPage.Validate();
                    break;
            }
        }
        
        [When(@"I have complete the wizard with ""(.*)"" workflow settings")]
        public void WhenIHaveCompleteTheWizardWithWorkflowSettings(string workflowSettingsReference)
        {
            var editProjectPage = ScenarioContext.Get<EditProjectPage>();
            editProjectPage.Validate();

            //var workflowSettings = AutomationContext.GetWorkflowSettings<EditProjectWorkflowSettings>(workflowSettingsReference);

            //editProjectPage.PerformWorkflow<Inte>(workflowSettings);
        }
        
    }
}
