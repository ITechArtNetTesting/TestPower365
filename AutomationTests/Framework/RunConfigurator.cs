using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Product.Framework
{
	/// <summary>
	///     Class RunConfigurator.
	/// </summary>
	public class RunConfigurator : BaseEntity
	{
		private static readonly XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
		public static string RunPath;
		public static string DownloadPath;
		public static string ResourcesPath;
		/// <summary>
		///     Gets the value.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <returns>String.</returns>
		public static string GetValue(string tag)
		{
			xmlDoc.Load(RunPath); // Load the XML document from the specified file
			var browser = xmlDoc.GetElementsByTagName(tag);
			return browser[0].InnerText;
		}

		public static string GetValueByXpath(string path)
		{
			xmlDoc.Load(RunPath);
			return xmlDoc.SelectSingleNode(path).InnerText;
		}

		public static string GetTenantValue(string metaname, string tenant, string value)
		{
			xmlDoc.Load(RunPath);
			string tenantName= xmlDoc.SelectSingleNode($"//tenantmigration[@metaname='{metaname}']").Attributes[$"{tenant}"].Value;
			return xmlDoc.SelectSingleNode($"//tenant//name[text()='{tenantName}']/..//{value}").InnerText;
		}

		/// <summary>
		///     Sets the value.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <param name="value">The value.</param>
		public static void SetValue(string tag, string value)
		{
			xmlDoc.Load(RunPath); // Load the XML document from the specified file
			var element = xmlDoc.GetElementsByTagName(tag);
			element[0].InnerText = value;
			xmlDoc.Save(RunPath);
		}

		public static bool IsCmtAccount(string login)
		{
			return login.EndsWith("cmtsandbox.com");
		}

		public static void CheckUserMigrationFileIsDownloaded()
		{
			for (var i = 0; i < 30; i++)
			{
				if (File.Exists(DownloadPath+"user-migration-template.csv"))
				{
					break;
				}
				Thread.Sleep(1000);
			}
			Assert.IsTrue(File.Exists(DownloadPath+"user-migration-template.csv"), "File is not downloaded");
			File.Delete(DownloadPath+"user-migration-template.csv");
		}

		public static void CheckLogsFileIsDownloaded()
		{
			for (var i = 0; i < 30; i++)
			{
				if (Directory.EnumerateFiles(DownloadPath, "SyncJob*.csv").Any())
				{
					break;
				}
				Thread.Sleep(1000);
			}
			Assert.IsTrue(Directory.EnumerateFiles(DownloadPath, "SyncJob*.csv").Any(), "File is not downloaded");
			ClearDownloads();
		}

	    public static void CheckRollbackLogsFileIsDownloaded()
	    {
	        for (var i = 0; i < 30; i++)
	        {
	            if (Directory.EnumerateFiles(DownloadPath, "RollbackJob*.csv").Any())
	            {
	                break;
	            }
	            Thread.Sleep(1000);
	        }
	        Assert.IsTrue(Directory.EnumerateFiles(DownloadPath, "RollbackJob*.csv").Any(), "File is not downloaded");
	        ClearDownloads();
	    }

        public static void CheckDiscoveryFileIsDownloaded()
		{
			for (var i = 0; i < 30; i++)
			{
				if (Directory.EnumerateFiles(DownloadPath, "Tenant*.csv").Any())
				{
					break;
				}
				Thread.Sleep(1000);
			}
			Assert.IsTrue(Directory.EnumerateFiles(DownloadPath, "Tenant*.csv").Any(), "File is not downloaded");
			Store.TenantLog = Directory.EnumerateFiles(DownloadPath, "Tenant*.csv").First();
		}

		public static bool AssertLineExistsInCsv(string file, string line)
		{
			bool result = false;
			var reader = new StreamReader(File.OpenRead(file));
			while (!reader.EndOfStream)
			{
				string currentLine = reader.ReadLine();
				Log.Info(currentLine);
				if (currentLine.Contains(line))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public static bool AssertLineExistsInCsv(string file, string line, int count)
		{
			var reader = new StreamReader(File.OpenRead(file));
			int counter = 0;
			
			while (!reader.EndOfStream)
			{
				Log.Info(reader.ReadLine());
				if (reader.ReadLine().Contains(line))
				{
					counter++;
				}
			}
			return counter >= count;
		}

		public static void CheckUserMatchFileIsDownloaded()
		{
			for (var i = 0; i < 30; i++)
			{
				if (File.Exists(DownloadPath+"user-match-template.csv"))
				{
					break;
				}
				Thread.Sleep(1000);
			}
			Assert.IsTrue(File.Exists(DownloadPath+"user-match-template.csv"), "File is not downloaded");
			File.Delete(DownloadPath+"user-match-template.csv");
		}

		public static void CheckUsersExportFileIsDownloaded()
		{
			for (var i = 0; i < 30; i++)
			{
				if (File.Exists(DownloadPath+"users.csv"))
				{
					break;
				}
				Thread.Sleep(1000);
			}
			Assert.IsTrue(File.Exists(DownloadPath+"users.csv"), "File is not downloaded");
			File.Delete(DownloadPath+"users.csv");
		}

		public static void ClearDownloads()
		{
			try
			{
				var downloadDirectory = new DirectoryInfo(DownloadPath);
				foreach (var file in downloadDirectory.GetFiles())
				{
					file.Delete();
				}
			}
			catch (Exception)
			{
				Log.Info("IMPOSSIBLE TO CLEAN DOWNLOAD FOLDER");
			}
		
		}

		public static int GetCSVlinesCount(string path)
		{
			return File.ReadAllLines(path).Length-1;
		}

		public static void CheckProvisioningLogsFileIsDownloadedAndNotEmpty()
		{
			for (var i = 0; i < 30; i++)
			{
				if (Directory.EnumerateFiles(DownloadPath, "PFSyncJob*.csv").Any())
				{
					break;
				}
				Thread.Sleep(1000);
			}
			Assert.IsTrue(Directory.EnumerateFiles(DownloadPath, "PFSyncJob*.csv").Any(), "File is not downloaded");
			FileInfo file = new FileInfo(Directory.EnumerateFiles(DownloadPath, "PFSyncJob*.csv").First());
			Assert.IsTrue(file.Length>0, "Log File is empty");
			file.Delete();
		}

		public static void CreateFlagFolder(string path)
		{
			if (Directory.Exists(path))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (directoryInfo.GetFiles().Length > 0)
				{
					foreach (FileInfo file in directoryInfo.GetFiles())
					{
						file.Delete();
					}
				}
			}
			else
			{
				Directory.CreateDirectory(path);
			}
		}

		public static void CreateEmptyFile(string path)
		{
			File.Create(path).Dispose();
		}
	}
}