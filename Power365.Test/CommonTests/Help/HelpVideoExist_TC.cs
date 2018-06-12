using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Pages.SetUpProjectPages;
using BinaryTree.Power365.AutomationFramework.Workflows;
using NUnit.Framework;


namespace BinaryTree.Power365.Test.CommonTests.Help
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class HelpVideoExist_TC : TestBase
    {
        [Test]
        [Category("UI")]
        [Category("Integration")]       
        public void DirSyncHelpVideoExist_39086TC()
        {
            var settings = Automation.Settings;

            var client = settings.GetByReference<Client>("client2");
            var clientName = client.Name;

            var signInUser = client.Administrator.Username;
            var signInPassword = client.Administrator.Password;

            var project = client.GetByReference<Project>("project2");
            var projectName = project.Name;
            var projectDescription = project.Description;

            var sourceTenant = settings.GetByReference<Tenant>(project.Source);
            var targetTenant = settings.GetByReference<Tenant>(project.Target);

            var _sourceTenant = sourceTenant.Name;
            var _targetTenant = targetTenant.Name;

            string _sourceFolder = project.GetByReference<UserMigration>("entry4").Source;
            string _targetFolder = project.GetByReference<UserMigration>("entry4").Target;
          
            ProjectDetailsPage projectDetailPage = Automation.Common
                                        .SingIn(signInUser, signInPassword)
                                        .MigrateAndIntegrateSelect()
                                        .ClientSelect(clientName)
                                        .ProjectSelect(projectName)
                                        .GetPage<ProjectDetailsPage>();

            var editProjectWorkflow = projectDetailPage.ClickProjectEdit<IntegrationProjectWorkflow>();
            var dirSyncPage = editProjectWorkflow
                 .ProjectName()
                 .ProjectDescription(projectDescription)
                 .AddTenant(true)
                 .ReviewTenant()
                 .SelectDomain(true)
                 .DiscoverUsers(true)
                 .DiscoverADGroups(true)
                 .ReviewGroups(true)
                 .MatchSourceUsers(true)
                 .CreateUsers(true)
                 .DiscoveDistribuitionGroups(true)
                 .MatchDistribuitionGroups(true)
                 .CreateGroupsInTarget(true)
                 .MigrationWaves(true)
                 .DefineMigrationWaves(false, true)
                 .AssignSyncSheduleOfWave(true)
                 .CreateAddressBook(true)
                 .ShareCalendar(true)
                 .IdentifyUsersThatWillShareCalendarAvailability(true)
                 .SelectGroupToUseCalendar(true)
                 .YesMigratePublicFolders(true)
                 .KeepMyExistingPublicFolder()
                 .TenantsAndAzureADConnectStatus()
                 .DownloadDirSync()
                 .GetPage<DirSyncDownloadPage>();

            Assert.True(dirSyncPage.IsVidioElementPresent(), "Video element should be visible and clickable");
        }

        [Test]
        [Category("UI")]
        [Category("Integration")]     
        public void DKIMHelpVideoExist_39087TC()
        {
            var settings = Automation.Settings;
            var client = settings.GetByReference<Client>("client2");
            var clientName = client.Name;

            var signInUser = client.Administrator.Username;
            var signInPassword = client.Administrator.Password;

            var project = client.GetByReference<Project>("project2");
            var projectName = project.Name;
            var projectDescription = project.Description;

           
            ProjectDetailsPage projectDetailPage = Automation.Common
                                        .SingIn(signInUser, signInPassword)
                                        .MigrateAndIntegrateSelect()
                                        .ClientSelect(clientName)
                                        .ProjectSelect(projectName)
                                        .GetPage<ProjectDetailsPage>();

            var editProjectWorkflow = projectDetailPage.ClickProjectEdit<IntegrationProjectWorkflow>();
            var emailSignaturePage = editProjectWorkflow
                 .ProjectName()
                 .ProjectDescription(projectDescription)
                 .AddTenant(true)
                 .ReviewTenant()
                 .SelectDomain(true)
                 .DiscoverUsers(true)
                 .DiscoverADGroups(true)
                 .ReviewGroups(true)
                 .MatchSourceUsers(true)
                 .CreateUsers(true)
                 .DiscoveDistribuitionGroups(true)
                 .MatchDistribuitionGroups(true)
                 .CreateGroupsInTarget(true)
                 .MigrationWaves(true)
                 .DefineMigrationWaves(false, true)
                 .AssignSyncSheduleOfWave(true)
                 .CreateAddressBook(true)
                 .ShareCalendar(true)
                 .IdentifyUsersThatWillShareCalendarAvailability(true)
                 .SelectGroupToUseCalendar(true)
                 .YesMigratePublicFolders(true)
                 .KeepMyExistingPublicFolder()
                 .TenantsAndAzureADConnectStatus()
                 .DownloadDirSync(true)
                 .ConfiguringPower365(true)
                 .PasswordToSecurePower365(true)
                 .EmailRewriting(true)
                 .WantToHaveEmailRewritten(true)
                 .ExchangeOnlineEmailRewriteGroupSteps(true)
                 .EmailSignatures(false)
                 .GetPage<EmailSignaturesPage>();

            Assert.True(emailSignaturePage.IsVidioElementPresent(), "Video element should be visible and clickable");
        }

    }
}
