using System;
using System.Data;
using System.Data.SqlClient;


namespace BinaryTree.Power365.AutomationFramework.Utilities
{
    public class SqlClient : IDisposable
    {
        private string _connectionString;
        private SqlConnection _connection;

        public SqlClient(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
        }

        public SqlTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        public void CommitTransaction(SqlTransaction transaction)
        {
            transaction.Commit();
        }

        public void RollbackTransaction(SqlTransaction transaction)
        {
            transaction.Rollback();
        }

        public int ExecuteNonQuery(string command, SqlTransaction transaction = null)
        {
            CheckConnection();

            var cmd = transaction != null ? new SqlCommand(command, _connection, transaction) : new SqlCommand(command, _connection);

            using (cmd)
            {
                cmd.CommandTimeout = 180;
                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            CheckConnection();
            
            using (command)
            {
                command.CommandTimeout = 180;
                return command.ExecuteNonQuery();
            }
        }

        public T ExecuteScalar<T>(string query, SqlTransaction transaction = null)
        {
            CheckConnection();

            var cmd = transaction != null ? new SqlCommand(query, _connection, transaction) : new SqlCommand(query, _connection);

            using (cmd)
            {
                var result = cmd.ExecuteScalar();
                if (result is T)
                {
                    return (T)result;
                }
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        public DataSet ExecuteDataSet(string query, SqlTransaction transaction = null)
        {
            CheckConnection();

            var cmd = transaction != null ? new SqlCommand(query, _connection, transaction) : new SqlCommand(query, _connection);
            DataSet dataSet = new DataSet();

            using (cmd)
            {
                var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        private void CheckConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                switch (_connection.State)
                {
                    case ConnectionState.Broken:
                    case ConnectionState.Closed:
                        _connection.Close();
                        _connection.Open();
                        break;
                    case ConnectionState.Executing:
                    case ConnectionState.Fetching:
                    default:
                        throw new Exception("SqlConnection is busy...");
                }
            }
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }
    }
}