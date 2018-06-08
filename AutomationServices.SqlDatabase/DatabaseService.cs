using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace AutomationServices.SqlDatabase
{
    public class DatabaseService : IDisposable
    {
        public SqlClient Clients { get { return _clients ?? (_clients = new SqlClient(_clientsDatabase.GetAzureSqlConnectionString())); } }
        public SqlClient T2T { get { return _t2t ?? (_t2t = new SqlClient(_t2tDatabase.GetAzureSqlConnectionString())); } }
        public SqlClient CDS { get { return _cds ?? (_cds = new SqlClient(_cdsDatabase.GetAzureSqlConnectionString())); } }
        public SqlClient DirSyncLite { get { return _dsLite ?? (_dsLite = new SqlClient(_dsLiteDatabase.GetLocalSqlConnectionString())); } }

        private SqlClient _clients;
        private SqlClient _t2t;
        private SqlClient _cds;
        private SqlClient _dsLite;

        private readonly Settings _settings;
        private readonly Database _clientsDatabase;
        private readonly Database _t2tDatabase;
        private readonly Database _cdsDatabase;
        private readonly Database _dsLiteDatabase;

        public DatabaseService(Settings settings)
        {
            _settings = settings;
            _clientsDatabase = _settings.GetByReference<Database>("clients");
            _t2tDatabase = _settings.GetByReference<Database>("t2t");
            _cdsDatabase = _settings.GetByReference<Database>("cds");
            _dsLiteDatabase = settings.GetByReference<Database>("dslite1");
        }

        public void SetDirSyncLiteTenantId(string dirSyncProfileName, int tenantId)
        {
            var configDataSet = DirSyncLite.ExecuteDataSet("SELECT BT_Config_PK, ConfigXml FROM BT_Config");

            var table = configDataSet.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                var profileId = (int)row["BT_Config_PK"];
                var xml = (string)row["ConfigXml"];

                xml = xml.Replace("&lt;", "<").Replace("&gt;", ">");

                XDocument xmlDoc = XDocument.Parse(xml);

                var syncName = xmlDoc.Descendants().Where(e => e.Name.LocalName == "SyncName").FirstOrDefault();
                if (syncName.Value != dirSyncProfileName)
                    continue;

                var sourceContainerDefaults = xmlDoc.Descendants().Where(e => e.Name.LocalName == "SourceContainerDefaults").FirstOrDefault();
                var element = sourceContainerDefaults.Descendants().Where(e => e.Name.LocalName == "TenantId").FirstOrDefault();
                element.Value = tenantId.ToString();

                var result = xmlDoc.ToString(SaveOptions.DisableFormatting);

                result = result.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&lt;Value&gt;", "<Value>").Replace("&lt;/Value&gt;", "</Value>");

                SqlCommand updateCommand = new SqlCommand("UPDATE BT_Config SET ConfigXml = @ConfigXml WHERE BT_Config_PK = @Id");
                updateCommand.Parameters.AddWithValue("@Id", profileId);
                updateCommand.Parameters.AddWithValue("@ConfigXml", result);

                DirSyncLite.ExecuteNonQuery(updateCommand);
            }
        }

        public void SetUserMigrationState(string clientName, string projectName, string userPrincipalName, StateType desiredState)
        {
            var clientId = GetClientId(clientName);
            var projectId = GetProjectId(projectName, clientId);
            var userMigrationId = GetUserMigrationId(userPrincipalName, projectId);

            int migrationStateId = 3;
            switch (desiredState)
            {
                case StateType.Stopping:
                case StateType.Preparing:
                case StateType.Syncing:
                case StateType.RollbackInProgress:
                case StateType.RollbackError:
                    migrationStateId = 18;
                    break;
                case StateType.Matched:
                    migrationStateId = 3;
                    break;
                case StateType.Prepared:
                case StateType.Synced:
                case StateType.RollbackCompleted:
                case StateType.Complete:
                    migrationStateId = 20;
                    break;
                case StateType.Moved:
                    migrationStateId = 10;
                    break;
                default:
                    throw new Exception(string.Format("MigrationStateId was not be found for state '{0}' ", desiredState));
            }

            T2T.ExecuteNonQuery(string.Format("UPDATE UserMigration SET MigrationStateID = {0} WHERE UserMigrationID = {1}", migrationStateId, userMigrationId));
        }

        public void ResetUser(string clientName, string projectName, string sourceUserPrincipalName, StateType desiredState)
        {
            var clientId = GetClientId(clientName);
            var projectId = GetProjectId(projectName, clientId);
            var userMigrationId = GetUserMigrationId(sourceUserPrincipalName, projectId);

            bool removePrepareJobs = false;
            bool removeSyncJobs = false;
            bool removeCutoverJobs = false;

            bool removeTargetUserId = false;
            bool resetIsLocked = false;
            bool resetIsArchived = false;

            int migrationStateId = StateTypeToMigrationStateId(desiredState);

            switch (desiredState)
            {
                case StateType.Ready:
                case StateType.Matched:
                case StateType.NoMatch:
                    removePrepareJobs = true;
                    removeSyncJobs = true;
                    removeCutoverJobs = true;
                    removeTargetUserId = true;
                    resetIsLocked = true;
                    resetIsArchived = true;
                    break;
                case StateType.Prepared:
                    removeSyncJobs = true;
                    removeCutoverJobs = true;
                    break;
                case StateType.Synced:
                    removeCutoverJobs = true;
                    break;
                default:
                    break;
            }

            using (var transaction = T2T.BeginTransaction())
            {
                try
                {
                    if (removePrepareJobs)
                        RemovePrepareJobs(userMigrationId, transaction);
                    if (removeSyncJobs)
                        RemoveSyncJobs(userMigrationId, transaction);
                    if (removeCutoverJobs)
                        RemoveCutoverJobs(userMigrationId, transaction);

                    var setValues = string.Format("MigrationStateId = {0}", migrationStateId);

                    if (removeTargetUserId)
                        setValues = setValues + string.Format(", TargetUserId = NULL, NewPrimarySMTPAddress = NULL, NewUserPrincipalName = NULL");

                    if (resetIsLocked)
                        setValues = setValues + string.Format(", IsLocked = 0");

                    if (resetIsArchived)
                        setValues = setValues + string.Format(", IsArchived = 0");

                    T2T.ExecuteNonQuery(string.Format("UPDATE UserMigration SET {0} WHERE UserMigrationID = {1}", setValues, userMigrationId), transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void RemovePrepareJobs(int userMigrationId, SqlTransaction transaction = null)
        {
            T2T.ExecuteNonQuery(string.Format("DELETE FROM PrepareJob WHERE UserMigrationID = {0}", userMigrationId), transaction);
        }

        public void RemoveSyncJobs(int userMigrationId, SqlTransaction transaction = null)
        {
            T2T.ExecuteNonQuery(string.Format("DELETE FROM SyncJob WHERE UserMigrationID = {0}", userMigrationId), transaction);
        }

        public void RemoveCutoverJobs(int userMigrationId, SqlTransaction transaction = null)
        {
            T2T.ExecuteNonQuery(string.Format("DELETE FROM CutoverJob WHERE UserMigrationID = {0}", userMigrationId), transaction);
        }

        public int GetClientId(string clientName)
        {
            return Clients.ExecuteScalar<int>(string.Format("SELECT [ClientId] FROM Client WHERE [ClientName] = '{0}'", clientName));
        }

        public int GetProjectId(string projectName, int clientId = 0)
        {
            return T2T.ExecuteScalar<int>(string.Format("SELECT [ProjectId] FROM Project WHERE [ProjectName] = '{0}' {1}", projectName, clientId > 0 ? string.Format("AND [ClientId] = {0}", clientId) : string.Empty));
        }

        public int GetUserMigrationId(string sourceUserPrincipalName, int projectId)
        {
            return T2T.ExecuteScalar<int>(string.Format("SELECT [UserMigrationId] FROM UserMigration AS UM JOIN [User] AS SU ON UM.SourceUserID = SU.UserId WHERE [SU].[UserPrincipalName] = '{0}' AND [UM].[ProjectId] = {1}", sourceUserPrincipalName, projectId));
        }

        public void Dispose()
        {
            if (_clients != null)
                _clients.Dispose();
            if (_t2t != null)
                _t2t.Dispose();
            if (_cds != null)
                _cds.Dispose();
        }

        private int StateTypeToMigrationStateId(StateType stateType)
        {
            switch (stateType)
            {
                case StateType.None:
                    return 0;
                case StateType.NoMatch:
                    return 1;
                case StateType.MultipleMatches:
                    return 2;
                case StateType.Ready:
                case StateType.Matched:
                    return 3;
                case StateType.Preparing:
                    return 4;
                case StateType.Prepared:
                    return 5;
                case StateType.Syncing:
                    return 6;
                case StateType.Synced:
                    return 7;
                case StateType.Stopping:
                    return 8;
                case StateType.Finalizing:
                    return 9;
                case StateType.Complete:
                    return 10;
                case StateType.PrepareError:
                    return 11;
                case StateType.SyncError:
                    return 12;
                case StateType.CutoverError:
                    return 13;
                case StateType.WaveError:
                    return 14;
                case StateType.RollbackInProgress:
                    return 16;
                case StateType.RollbackCompleted:
                    return 17;
                case StateType.RollbackError:
                    return 18;
                case StateType.Moving:
                    return 19;
                case StateType.Moved:
                    return 20;
                case StateType.MoveError:
                    return 21;
                case StateType.Matching:
                    return 25;
                default:
                    throw new Exception(string.Format("MigrationStateId could not be found for state '{0}' ", stateType));
            }
        }
    }
}
