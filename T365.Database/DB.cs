using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T365.Database
{
    class DB
    {
        string connectionString;

        public DB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Execute(string commandStr)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(commandStr, connection);
                    command.CommandTimeout = 80;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can not execute " + commandStr, ex);
            }
        }

        public string ReturnFirstExecuted(string commandStr)
        {
            string result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(commandStr, connection);
                    command.CommandTimeout = 80;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result = Convert.ToString(reader[0]);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can not execute " + commandStr, ex);
            }
            return result;
        }
    }
}
