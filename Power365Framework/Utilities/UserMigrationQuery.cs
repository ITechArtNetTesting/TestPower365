using BinaryTree.Power365.AutomationFramework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Utilities
{
   public class UserMigrationQuery
    {
        public bool SelectIsLockedByUsermail(string usermail, string projectName, string connectionString)
        {
            using (var sqlClient = new SqlClient(connectionString))
            {
                return sqlClient.ExecuteScalar<bool>($"select IsLocked from [UserMigration] where NewUserPrincipalName='{usermail}' and ProjectId in (select projectId from Project where ProjectName='{projectName}')");
            }
            
        }

        public void SetMigrationStateTo(StateType state,string usermail, string projectName, string connectionString)
        {
            int migrationStateId=3;
            switch (state)
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
                    throw new Exception(string.Format("MigrationStateId was not be found for state '{0}' ", state));                    
            }
            using (var sqlClient = new SqlClient(connectionString))
            {
                sqlClient.ExecuteNonQuery($"update UserMigration set MigrationStateId={migrationStateId} where lower(NewUserPrincipalName)=lower('{usermail}') and ProjectId in (select projectId from Project where ProjectName='{projectName}')");
            }
        }
    }
}
