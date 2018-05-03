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

        public static string GetUserLogin(string client)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//user").InnerText; ;
        }

        public static string GetPassword(string client)
        {
             return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//password").InnerText;
        }

        public static string GetClient(string client)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/../name").InnerText;
        }

        public static string GetRole(string client)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/../name").InnerText;
        }

        public static string GetProjectName(string client, string project )
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//name").InnerText;            
        }

        public static bool IsUserFree(string Source)
        {
            bool result = true;
            foreach (XmlElement entry in xmlDoc.SelectNodes($"//client//project//entry/source"))
            {
                if (Source == entry.InnerText)
                {
                    result = false;
                }
            }
            return result;
        }

        public static string GetFileName(string client, string project, string file)
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{file}']/..//filename").InnerText;
        }

        public static string GetADGroupName(string client, string project, string group)
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/../name").InnerText;
        }

        public static string GetMail(string client, string project, string group)
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//mail").InnerText;
        }

        public static string GetGroupMember(string client, string project, string group, string member)
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//{member}").InnerText;
        }

        public static string GetSourceMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//source").InnerText;
        }

        public static string GetTargetMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//target").InnerText;
        }

        public static string GetTargetSmtpMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//targetsmtp").InnerText;
        }

        public static string GetTargetX500Mailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//targetx500").InnerText;
        }

        public static string GetGroupFirstMember(string client, string project, string group)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//member1").InnerText;
        }

        public static string GetGroupOwner(string client, string project, string group)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//owner").InnerText;
        }

        public static string GetGroupMail(string client, string project, string group)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//mail").InnerText;
        }

        public static string GetSourceSmtpMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//smtp").InnerText;
        }

        public static string GetUpnMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//upn").InnerText;
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


        public static string GetConnectionString()
        {
            xmlDoc.Load(RunPath);
            String initialCatalog = xmlDoc.SelectSingleNode("//database//initialCatalog").InnerText;
            String userID = xmlDoc.SelectSingleNode("//database//userID").InnerText;
            String password = xmlDoc.SelectSingleNode("//database//password").InnerText;
            String server = xmlDoc.SelectSingleNode("//database//server").InnerText;
            return ($"Server=tcp:{server},1433; Initial Catalog = {initialCatalog}; Persist Security Info = False; User ID = {userID}; Password ={password}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
            
        }

        public static string GetConnectionStringDBClients()
        {
            xmlDoc.Load(RunPath);
            String initialCatalog = xmlDoc.SelectSingleNode("//database//initialClientsCatalog").InnerText;
            String userID = xmlDoc.SelectSingleNode("//database//userID").InnerText;
            String password = xmlDoc.SelectSingleNode("//database//password").InnerText;
            String server = xmlDoc.SelectSingleNode("//database//server").InnerText;
            return ($"Server=tcp:{server},1433; Initial Catalog={initialCatalog}; Persist Security Info = False; User ID ={userID}; Password ={password}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
        
        }
    }
}