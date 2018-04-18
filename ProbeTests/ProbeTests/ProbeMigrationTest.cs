using System;
using System.Globalization;
using ProbeTests.Model;
using Product.Framework;
using Product.Framework.Enums;

namespace ProbeTests.ProbeTests
{
    public class ProbeMigrationTest : ProbeTest, IProbeTest
    {
        private string clientName;
        private string projectName;

        public ProbeMigrationTest() : base(ProbeType.Migration)
        {
            clientName = configurator.GetValueByXpath("//MigrationProbe/@client");
            projectName = configurator.GetValueByXpath("//MigrationProbe/@project");
        }

        public override void Run()
        {
            var username = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//user");
            var password = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//password");
            var project = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//name");

            var syncUsername = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//source");

            try
            {
                LogIn(username, password);
                SelectProject(project);
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error during login and reload file");
                throw e;
            }
            try
            {
                User.AtProjectOverviewForm().OpenUsersList();
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error navigating to user migration form");
                throw e;
            }
            try
            {
                User.AtUsersForm().SyncUserByLocator(syncUsername);
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().AssertUserHaveSyncingState(syncUsername);
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error selecting mailbox and syncing it");
                throw e;
            }
            try
            {
                User.AtUsersForm().WaitForState(syncUsername, State.Synced, 30000);
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Sync error");
                throw e;
            }
            try
            {
                User.AtUsersForm().OpenDetailsByLocator(syncUsername);
                User.AtUsersForm().SortStartedJobs();
                User.AtUsersForm().WaitForJobSortedByTime();
                User.AtUsersForm().DownloadLogs();
                configurator.CheckLogsFileIsDownloaded();
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Log downloading error");
                throw e;
            }
            InsertDataToSql(DateTime.UtcNow);
        }
    }
}