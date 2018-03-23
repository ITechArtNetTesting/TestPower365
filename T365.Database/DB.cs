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

        public bool Execute(string commandStr)
        {

          
            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandStr, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }                
            }
        }

        public string ReturnFirstExecuted(string commandStr)
        {
            string result = null;
            using (SqlConnection connection =
              new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandStr, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result = Convert.ToString(reader[0]);
                    }
                    reader.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }

            }
        }
    }
