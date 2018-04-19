using System;
using System.Data;
using System.Data.SqlClient;

namespace Product.Utilities
{
    public class SqlClient: IDisposable
    {
        private string _connectionString;
        private SqlConnection _connection;

        public SqlClient(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
        }

        public int ExecuteNonQuery(string command)
        {
            CheckConnection();
            using (SqlCommand cmd = new SqlCommand(command, _connection))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public T SelectValue<T>(string query)
        {
            CheckConnection();
            using (var cmd = new SqlCommand(query, _connection))
            {
                var result = cmd.ExecuteScalar();
                if (result is T)
                {
                    return (T)result;
                }
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        private void CheckConnection()
        {
            if(_connection.State != ConnectionState.Open)
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
