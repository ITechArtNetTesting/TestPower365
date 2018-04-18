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
        Store store;
        public RunConfigurator(Store store)
        {
            this.store = store;
        }

        private readonly XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
        public string RunPath;
        public string DownloadPath;
        public string ResourcesPath;
        
        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>String.</returns>
        public string GetValue(string tag)
		{
           
           xmlDoc.Load(RunPath); // Load the XML document from the specified file
           var browser = xmlDoc.GetElementsByTagName(tag);
			return browser[0].InnerText;
		}

		public string GetValueByXpath(string path)
		{
			xmlDoc.Load(RunPath);
			return xmlDoc.SelectSingleNode(path).InnerText;
		}

		public string GetTenantValue(string metaname, string tenant, string value)
		{
			xmlDoc.Load(RunPath);
			string tenantName= xmlDoc.SelectSingleNode($"//tenantmigration[@metaname='{metaname}']").Attributes[$"{tenant}"].Value;
			return xmlDoc.SelectSingleNode($"//tenant//name[text()='{tenantName}']/..//{value}").InnerText;
		}

        public string GetUserLogin(string client)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//user").InnerText; ;
        }

        public string GetPassword(string client)
        {
             return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//password").InnerText;
        }

        public string GetClient(string client)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/../name").InnerText;
        }

        public string GetRole(string client)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/../name").InnerText;
        }

        public string GetProjectName(string client, string project )
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//name").InnerText;
        }

        public string GetFileName(string client, string project, string file)
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{file}']/..//filename").InnerText;
        }

        public string GetADGroupName(string client, string project, string group)
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/../name").InnerText;
        }

        public string GetMail(string client, string project, string group)
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//mail").InnerText;
        }

        public string GetGroupMember(string client, string project, string group, string member)
        {
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//{member}").InnerText;
        }

        public string GetSourceMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//source").InnerText;
        }

        public string GetTargetMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//target").InnerText;
        }

        public string GetTargetSmtpMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//targetsmtp").InnerText;
        }

        public string GetTargetX500Mailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//targetx500").InnerText;
        }

        public string GetGroupFirstMember(string client, string project, string group)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//member1").InnerText;
        }

        public string GetGroupOwner(string client, string project, string group)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//owner").InnerText;
        }

        public string GetGroupMail(string client, string project, string group)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{group}']/..//mail").InnerText;
        }

        public string GetSourceSmtpMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//smtp").InnerText;
        }

        public string GetUpnMailbox(string client, string project, string entry)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode($"//metaname[text()='{client}']/..//metaname[text()='{project}']/..//metaname[text()='{entry}']/..//upn").InnerText;
        }



        /// <summary>
        ///     Sets the value.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string tag, string value)
		{
			xmlDoc.Load(RunPath); // Load the XML document from the specified file
			var element = xmlDoc.GetElementsByTagName(tag);
			element[0].InnerText = value;
			xmlDoc.Save(RunPath);
		}

		public bool IsCmtAccount(string login)
		{
			return login.EndsWith("cmtsandbox.com");
		}

		public void CheckUserMigrationFileIsDownloaded()
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

		public void CheckLogsFileIsDownloaded()
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

	    public void CheckRollbackLogsFileIsDownloaded()
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

        public void CheckDiscoveryFileIsDownloaded()
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
            store.TenantLog = Directory.EnumerateFiles(DownloadPath, "Tenant*.csv").First();
		}

		public bool AssertLineExistsInCsv(string file, string line)
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

		public bool AssertLineExistsInCsv(string file, string line, int count)
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

		public void CheckUserMatchFileIsDownloaded()
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

		public void CheckUsersExportFileIsDownloaded()
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

		public void ClearDownloads()
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

		public int GetCSVlinesCount(string path)
		{
			return File.ReadAllLines(path).Length-1;
		}

		public void CheckProvisioningLogsFileIsDownloadedAndNotEmpty()
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

		public void CreateFlagFolder(string path)
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

		public void CreateEmptyFile(string path)
		{
			File.Create(path).Dispose();
		}


        public string GetConnectionString()
        {
            xmlDoc.Load(RunPath);
            String initialCatalog = xmlDoc.SelectSingleNode("//database//initialCatalog").InnerText;
            String userID = xmlDoc.SelectSingleNode("//database//userID").InnerText;
            String password = xmlDoc.SelectSingleNode("//database//password").InnerText;
            return ($"Server=tcp:bt-qa-sql.database.windows.net,1433; Initial Catalog = {initialCatalog}; Persist Security Info = False; User ID = {userID}; Password ={password}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
            
        }

        public string GetConnectionStringDBClients()
        {
            xmlDoc.Load(RunPath);
            String initialCatalog = xmlDoc.SelectSingleNode("//database//initialClientsCatalog").InnerText;
            String userID = xmlDoc.SelectSingleNode("//database//userID").InnerText;
            String password = xmlDoc.SelectSingleNode("//database//password").InnerText;
            return ($"Server=tcp:bt-qa-sql.database.windows.net,1433; Initial Catalog={initialCatalog}; Persist Security Info = False; User ID ={userID}; Password ={password}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
        
        }
    }
}