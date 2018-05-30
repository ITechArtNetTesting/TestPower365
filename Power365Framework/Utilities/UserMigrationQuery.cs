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
                return sqlClient.SelectValue<bool>($"select IsLocked from [UserMigration] where lower(NewUserPrincipalName)=lower('{usermail}') and ProjectId in (select projectId from Project where ProjectName='{projectName}')");
            }
            
        }

        public void SetMigrtationStateId(string usermail, string projectName, string connectionString)
        {
            using (var sqlClient = new SqlClient(connectionString))
            {
                sqlClient.ExecuteNonQuery($"update UserMigration set MigrationStateId=5 where lower(NewUserPrincipalName)=lower('{usermail}') and ProjectId in (select projectId from Project where ProjectName='{projectName}')");
            }

        }


        public void SetMigrtationStateToCompleted(string usermail, string projectName, string connectionString)
        {
            using (var sqlClient = new SqlClient(connectionString))
            {
                sqlClient.ExecuteNonQuery($"update UserMigration set MigrationStateId=20 where lower(NewUserPrincipalName)=lower('{usermail}') and ProjectId in (select projectId from Project where ProjectName='{projectName}')");
            }

        }

    }
}
