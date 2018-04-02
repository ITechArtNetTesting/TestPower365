using System;
using System.Data.SqlClient;

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
            var now = DateTime.Now;
            int delay = 60;
            bool ExecutedWithoutExeptions = false;
            while (ExecutedWithoutExeptions == false)
            {
                if ((DateTime.Now - now) - TimeSpan.FromSeconds(delay) > TimeSpan.Zero)
                {
                    break;
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(commandStr, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        ExecutedWithoutExeptions = true;
                    }
                }
                catch (Exception ex)
                {
                    //Log exeption
                }
            }
            if (ExecutedWithoutExeptions)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ReturnFirstExecuted(string commandStr)
        {
            var now = DateTime.Now;
            int delay = 60;
            bool ExecutedWithoutExeptions = false;
            string result = null;
            while (ExecutedWithoutExeptions == false)
            {
                if ((DateTime.Now - now) - TimeSpan.FromSeconds(delay) > TimeSpan.Zero)
                {
                    break;
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(commandStr, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            result = Convert.ToString(reader[0]);
                        }
                        reader.Close();
                        ExecutedWithoutExeptions = true;
                    }
                }
                catch (Exception ex)
                {
                    //Log exeption
                }
            }
            return result;
        }
    }
}
