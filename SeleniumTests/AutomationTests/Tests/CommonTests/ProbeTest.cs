using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Framework;

namespace Product.Tests.CommonTests
{
	public class ProbeTest : LoginAndConfigureTest
	{
		private static string _probeType;
		private static int _id;
		private static DateTime _startTime = DateTime.UtcNow;
		private static string _connectionString; 

		public ProbeTest(string probeType)
		{
			RunOnce();
			RunConfigurator.RunPath = "resources/probeRun.xml";
			_connectionString = $"Server={RunConfigurator.GetValueByXpath("//sql//server")};Database={RunConfigurator.GetValueByXpath("//sql//db")};User Id={RunConfigurator.GetValueByXpath("//sql//user")};Password={RunConfigurator.GetValueByXpath("//sql//password")};";
			_probeType = probeType;
			InitialStoring(_probeType);
			CleanUp();
		}

		private void InitialStoring(string probetype)
		{
			string queryString = $"Insert into Probe (ProbeType, Started) output INSERTED.ProbeId VALUES ('{probetype}', '{_startTime}')";
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand(queryString, connection);
				connection.Open();
				_id = (int)command.ExecuteScalar();
				connection.Close();
			}
		}

		public void InsertDataToSql(DateTime end)
		{
			string queryString = $"Update Probe SET Completed='{end}', IsSuccess='1', ErrorText='---' where ProbeType='{_probeType}' AND ProbeId='{_id}'";
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand(queryString, connection);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		public void InsertDataToSql(DateTime end, string error)
		{
			string queryString = $"Update Probe SET Completed='{end}', IsSuccess='0', ErrorText='{error.Replace("'", "*")}' where ProbeType='{_probeType}' AND ProbeId='{_id}'";
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand(queryString, connection);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}

		public void CleanUp()
		{
			string insertString =
				$"Insert into ProbeArchive Select * from Probe where ProbeType='{_probeType}' AND Started NOT IN (Select TOP 10 Started from Probe Where ProbeType='{_probeType}' ORDER BY Started DESC) ORDER BY Started DESC";
			string deleteString =
				$"Delete from Probe where ProbeType='{_probeType}' AND Started NOT IN (Select TOP 10 Started from Probe Where ProbeType='{_probeType}' ORDER BY Started DESC)";
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand insertCommand = new SqlCommand(insertString, connection);
				SqlCommand deleteCommand = new SqlCommand(deleteString, connection);
				connection.Open();
				insertCommand.ExecuteNonQuery();
				deleteCommand.ExecuteNonQuery();
				connection.Close();
			}
		}
	}
}
