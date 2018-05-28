using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;



namespace Product.Tests.IntegrationTests.DistributionGroupTests
{
    [TestClass]
    public class VerifyGroupSyncWillShowAccurateJobStatus_TC31796 : LoginAndConfigureTest
    {

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        string login;
        string password;
        string client;
        string projectName;
        string group;

        [TestInitialize()]
        public void Initialize()
        {
            login = RunConfigurator.GetUserLogin("client1");
            password = RunConfigurator.GetPassword("client1");
            client = RunConfigurator.GetClient("client2");
            projectName = RunConfigurator.GetProjectName("client2", "project2");
            group = RunConfigurator.GetADGroupName("client2", "project2", "group7");

        }


        [TestMethod]
        [TestCategory("Integration")]
        public void VerifyGroupSyncWillShowAccurateJobStatus_31796()
        {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenTotalGroups();
                User.AtGroupsMigrationForm().SearchGroup(group);
                User.AtGroupsMigrationForm().SelectGroupBylocator(group);
                User.AtGroupsMigrationForm().SelectAction(ActionType.Sync);
                User.AtGroupsMigrationForm().Apply();
                User.AtGroupsMigrationForm().ConfirmAction();
                User.AtGroupsMigrationForm().AssertUserHaveSyncingState(group);
                User.AtGroupsMigrationForm().OpenDetailsByLocator(group);
                User.AtGroupsMigrationForm().AssertState(State.Syncing);
                User.AtGroupsMigrationForm().WaitForJobIsCreated(group, State.Syncing, 12000000, 10);
                User.AtGroupsMigrationForm().WaitForState_DetailPage(group, State.Synced, 12000000, 10);
                User.AtGroupsMigrationForm().CloseUserDetails();
            
        }
    }
}
