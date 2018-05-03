using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using BinaryTree.Power365.AutomationFramework;
using IO = System.IO;

namespace Power365Framework.Test
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void CanSerialize_Settings()
        {
            Settings settings = new Settings();

            settings.BaseUrl = "https://bt-qa-pa-web-ui.azurewebsites.net/";
            settings.O365PowerShellUrl = "https://ps.outlook.com/powershell";
            settings.DownloadsPath = "downloads";
            settings.ChromeDriverPath = "resources";
            settings.TimeoutSec = 60;
            settings.Browser = "chrome";
            settings.Bitness = "x86";

            #region Client1
            Client client1 = new Client();
            client1.Reference = "client1";
            client1.Name = "BT-AutoQA1";

            Credential client1Administrator = new Credential();
            client1Administrator.Username = "BT-Automation-QA1@btpower365dev.onmicrosoft.com";
            client1Administrator.Password = "BinTree123";

            client1.Administrator = client1Administrator;


            #region Tenants
            Tenant tenant1 = new Tenant();
            tenant1.Reference = "tenant1";

            Credential client1Project1Tenant1CloudCredential = new Credential();
            client1Project1Tenant1CloudCredential.Reference = "cloud";
            client1Project1Tenant1CloudCredential.Username = "C7O365Admin@btcorp7.onmicrosoft.com";
            client1Project1Tenant1CloudCredential.Password = "BinTree123";

            Credential client1Project1Tenant1LocalCredential = new Credential();
            client1Project1Tenant1LocalCredential.Reference = "local";
            client1Project1Tenant1LocalCredential.Username = "corp7\administrator";
            client1Project1Tenant1LocalCredential.Password = "BinTree123";

            Credential client1Project1Tenant1PowerShellCredential1 = new Credential();
            client1Project1Tenant1PowerShellCredential1.Reference = "ps1";
            client1Project1Tenant1PowerShellCredential1.Username = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp7.onmicrosoft.com";
            client1Project1Tenant1PowerShellCredential1.Password = "P@ssw0rd";

            Credential client1Project1Tenant1PowerShellCredential2 = new Credential();
            client1Project1Tenant1PowerShellCredential2.Reference = "ps2";
            client1Project1Tenant1PowerShellCredential2.Username = "P365PowerShellTest@btcorp7.onmicrosoft.com";
            client1Project1Tenant1PowerShellCredential2.Password = "BinTree123";

            tenant1.PrimaryDomain = "btcorp7.onmicrosoft.com";

            tenant1.Credentials.AddRange(new[]
            {
                client1Project1Tenant1CloudCredential,
                client1Project1Tenant1LocalCredential,
                client1Project1Tenant1PowerShellCredential1,
                client1Project1Tenant1PowerShellCredential2
            });

            Tenant tenant2 = new Tenant();
            tenant2.Reference = "tenant2";

            Credential client1Project1Tenant2CloudCredential = new Credential();
            client1Project1Tenant2CloudCredential.Reference = "cloud";
            client1Project1Tenant2CloudCredential.Username = "corp9O365admin@btcorp9.onmicrosoft.com";
            client1Project1Tenant2CloudCredential.Password = "BinTree123";

            Credential client1Project1Tenant2LocalCredential = new Credential();
            client1Project1Tenant2LocalCredential.Reference = "local";
            client1Project1Tenant2LocalCredential.Username = "corp32\administrator";
            client1Project1Tenant2LocalCredential.Password = "Password32";

            Credential client1Project1Tenant2PowerShellCredential1 = new Credential();
            client1Project1Tenant2PowerShellCredential1.Reference = "ps1";
            client1Project1Tenant2PowerShellCredential1.Username = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp9.onmicrosoft.com";
            client1Project1Tenant2PowerShellCredential1.Password = "P@ssw0rd";

            Credential client1Project1Tenant2PowerShellCredential2 = new Credential();
            client1Project1Tenant2PowerShellCredential2.Reference = "ps2";
            client1Project1Tenant2PowerShellCredential2.Username = "P365PowerShellTest@btcorp9.onmicrosoft.com";
            client1Project1Tenant2PowerShellCredential2.Password = "BinTree123";

            tenant2.PrimaryDomain = "btcorp9.onmicrosoft.com";

            tenant2.Credentials.AddRange(new[]
            {
                client1Project1Tenant2CloudCredential,
                client1Project1Tenant2LocalCredential,
                client1Project1Tenant2PowerShellCredential1,
                client1Project1Tenant2PowerShellCredential2
            });

            Tenant tenant3 = new Tenant();
            tenant3.Reference = "tenant3";

            Credential tenant3CloudCredential = new Credential();
            tenant3CloudCredential.Username = "E2E.Admin@btcorp3.onmicrosoft.com";
            tenant3CloudCredential.Password = "Password3";

            tenant3.PrimaryDomain = "BTCorp3.onmicrosoft.com";

            tenant3.Credentials.AddRange(new[]
            {
                tenant3CloudCredential
            });

            Tenant tenant4 = new Tenant();
            tenant4.Reference = "tenant4";

            Credential tenant4CloudCredential = new Credential();
            tenant4CloudCredential.Username = "C11O365Admin@btcorp11.onmicrosoft.com";
            tenant4CloudCredential.Password = "BinTree123";

            tenant4.PrimaryDomain = "BTCorp11.onmicrosoft.com";

            tenant4.Credentials.AddRange(new[]
            {
                tenant4CloudCredential
            });

            settings.Tenants.AddRange(new[]
            {
                tenant1,
                tenant2,
                tenant3,
                tenant4
            });

            #endregion

            #region Project1
            Project client1Project1 = new Project();
            client1Project1.Reference = "project1";
            client1Project1.Name = "BT-AutoQA1-Project1";
            client1Project1.Source = "tenant1";
            client1Project1.Target = "tenant2";

            #region UserMigrations
            UserMigration client1Project1UserMigration1 = new UserMigration();
            client1Project1UserMigration1.Reference = "entry1";
            client1Project1UserMigration1.Source = "C7O365SML1@btcorp7.onmicrosoft.com";
            client1Project1UserMigration1.Target = "C7O365SML1@btcorp9.onmicrosoft.com";
            client1Project1UserMigration1.Group = "Wave1";
            client1Project1UserMigration1.Profile = "Profile 1";

            UserMigration client1Project1UserMigration2 = new UserMigration();
            client1Project1UserMigration2.Reference = "entry2";
            client1Project1UserMigration2.Source = "C7O365SML2@btcorp7.onmicrosoft.com";
            client1Project1UserMigration2.Target = "C7O365SML2@btcorp9.onmicrosoft.com";
            client1Project1UserMigration2.Group = "Wave1";
            client1Project1UserMigration2.Profile = "Profile 1";

            UserMigration client1Project1UserMigration3 = new UserMigration();
            client1Project1UserMigration3.Reference = "entry3";
            client1Project1UserMigration3.Source = "C7O365SML3@btcorp7.onmicrosoft.com";
            client1Project1UserMigration3.Target = "C7O365SML3@btcorp9.onmicrosoft.com";
            client1Project1UserMigration3.Group = "Wave1";
            client1Project1UserMigration3.Profile = "Profile 1";

            UserMigration client1Project1UserMigration4 = new UserMigration();
            client1Project1UserMigration4.Reference = "entry4";
            client1Project1UserMigration4.Source = "C7O365SML4@btcorp7.onmicrosoft.com";
            client1Project1UserMigration4.Target = "C7O365SML4@btcorp9.onmicrosoft.com";
            client1Project1UserMigration4.Group = "Wave1";
            client1Project1UserMigration4.Profile = "Profile 2";

            UserMigration client1Project1UserMigration5 = new UserMigration();
            client1Project1UserMigration5.Reference = "entry5";
            client1Project1UserMigration5.Source = "C7O365SML5@btcorp7.onmicrosoft.com";
            client1Project1UserMigration5.Target = "C7O365SML5@btcorp9.onmicrosoft.com";
            client1Project1UserMigration5.Group = "Wave2";
            client1Project1UserMigration5.Profile = "Profile 1";

            UserMigration client1Project1UserMigration6 = new UserMigration();
            client1Project1UserMigration6.Reference = "entry6";
            client1Project1UserMigration6.Source = "C7O365SML6@btcorp7.onmicrosoft.com";
            client1Project1UserMigration6.Target = "C7O365SML6@btcorp9.onmicrosoft.com";
            client1Project1UserMigration6.Group = "Wave1";
            client1Project1UserMigration6.Profile = "Profile 1";

            UserMigration client1Project1UserMigration7 = new UserMigration();
            client1Project1UserMigration7.Reference = "entry7";
            client1Project1UserMigration7.Source = "C7O365SML7@btcorp7.onmicrosoft.com";
            client1Project1UserMigration7.Target = "C7O365SML7@btcorp9.onmicrosoft.com";
            client1Project1UserMigration7.Group = "Wave1";
            client1Project1UserMigration7.Profile = "Profile 1";

            UserMigration client1Project1UserMigration8 = new UserMigration();
            client1Project1UserMigration8.Reference = "entry8";
            client1Project1UserMigration8.Source = "C7O365SML8@btcorp7.onmicrosoft.com";
            client1Project1UserMigration8.Target = "C7O365SML8@btcorp9.onmicrosoft.com";
            client1Project1UserMigration8.Group = "Wave1";
            client1Project1UserMigration8.Profile = "Profile 1";

            UserMigration client1Project1UserMigration9 = new UserMigration();
            client1Project1UserMigration9.Reference = "entry9";
            client1Project1UserMigration9.Source = "C7O365SML9@btcorp7.onmicrosoft.com";
            client1Project1UserMigration9.Target = "C7O365SML9@btcorp9.onmicrosoft.com";
            client1Project1UserMigration9.Group = "Wave2";
            client1Project1UserMigration9.Profile = "Profile 1";

            UserMigration client1Project1UserMigration10 = new UserMigration();
            client1Project1UserMigration10.Reference = "entry10";
            client1Project1UserMigration10.Source = "C7O365SML10@btcorp7.onmicrosoft.com";
            client1Project1UserMigration10.Target = "C7O365SML10@btcorp9.onmicrosoft.com";
            client1Project1UserMigration10.Group = "Wave2";
            client1Project1UserMigration10.Profile = "Profile 1";

            client1Project1.UserMigrations.AddRange(new[]{
                client1Project1UserMigration1,
                client1Project1UserMigration2,
                client1Project1UserMigration3,
                client1Project1UserMigration4,
                client1Project1UserMigration5,
                client1Project1UserMigration6,
                client1Project1UserMigration7,
                client1Project1UserMigration8,
                client1Project1UserMigration9,
                client1Project1UserMigration10
            });

            #endregion

            #region Files
            File client1Project1File1 = new File();
            client1Project1File1.Reference = "file1";
            client1Project1File1.Path = "MailOnlyCSVC7toC9SML-Automation-part1.csv";

            File client1Project1File2 = new File();
            client1Project1File2.Reference = "file2";
            client1Project1File2.Path = "MailOnlyCSVC7toC9SML-Automation-part2.csv";

            File client1Project1File3 = new File();
            client1Project1File3.Reference = "file3";
            client1Project1File3.Path = "ThreeTenants.csv";

            File client1Project1File4 = new File();
            client1Project1File4.Reference = "file4";
            client1Project1File4.Path = "MailOnlyCSVC7toC9SML.csv";

            File client1Project1File5 = new File();
            client1Project1File5.Reference = "file5";
            client1Project1File5.Path = "PowershellUsers.csv";

            client1Project1.Files.AddRange(new[]
            {
                client1Project1File1,
                client1Project1File2,
                client1Project1File3,
                client1Project1File4,
                client1Project1File5
            });

            #endregion

            #endregion

            #region Project2
            Project client1Project2 = new Project();
            client1Project2.Reference = "project2";
            client1Project2.Name = "BT-AutoQA1-Project2";
            client1Project2.Source = "tenant3";
            client1Project2.Target = "tenant4";

            UserMigration client1Project2UserMigrationEntry1 = new UserMigration();
            client1Project2UserMigrationEntry1.Reference = "entry5";
            client1Project2UserMigrationEntry1.Source = "C3O365ShareMBX5@btcorp3.onmicrosoft.com";
            client1Project2UserMigrationEntry1.Target = "C3O365ShareMBX5@btcorp11.onmicrosoft.com";
            client1Project2UserMigrationEntry1.Group = "Wave1";
            client1Project2UserMigrationEntry1.Profile = "Profile 1";

            client1Project2.UserMigrations.Add(client1Project2UserMigrationEntry1);

            File client1Project2File1 = new File();
            client1Project2UserMigrationEntry1.Reference = "file1";
            client1Project2File1.Path = "MailOnlyCSVC3toC11ShMBX-Automation.csv";

            client1Project2.Files.Add(client1Project2File1);

            #endregion

            client1.Projects.Add(client1Project1);
            client1.Projects.Add(client1Project2);
            #endregion

            #region Client2

            Client client2 = new Client();
            client2.Reference = "Client2";
            client2.Name = "BT-AutoQA2";

            Credential client2Administrator = new Credential();
            client2Administrator.Reference = "client2.Administrator";
            client2Administrator.Username = "BT-Automation-QA2@btpower365dev.onmicrosoft.com";
            client2Administrator.Password = "BinTree123";

            client2.Administrator = client2Administrator;

            Project client2Project1 = new Project();
            client2Project1.Reference = "project1";
            client2Project1.Source = "tenant1";
            client2Project1.Target = "tenant2";
            #region UserMigrations

            UserMigration client2Project1UserMigration1 = new UserMigration();
            client2Project1UserMigration1.Reference = "entryps1";
            client2Project1UserMigration1.Source = "C7-Automation1@btcorp7.onmicrosoft.com";
            client2Project1UserMigration1.Target = "C7-Automation1@btcorp9.onmicrosoft.com";
            client2Project1UserMigration1.Group = "Wave1";
            client2Project1UserMigration1.Profile = "Profile 1";

            UserMigration client2Project1UserMigration1x500 = new UserMigration();
            client2Project1UserMigration1x500.Reference = "entryps1x500";
            client2Project1UserMigration1x500.Source = "/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=6a84cbd5886341638d0a43ee93319ce0-C7-Automati";
            client2Project1UserMigration1x500.Target = "/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=e4003d7bc00c4f5da1129ae8865ffa66-C7-Automati";
            client2Project1UserMigration1x500.Group = "Wave1";
            client2Project1UserMigration1x500.Profile = "Profile 1";

            UserMigration client2Project1UserMigration2 = new UserMigration();
            client2Project1UserMigration2.Reference = "entryps2";
            client2Project1UserMigration2.Source = "c7-automation2@btcorp7.onmicrosoft.com";
            client2Project1UserMigration2.Target = "c7-automation2@btcorp9.onmicrosoft.com";
            client2Project1UserMigration2.Group = "Wave1";
            client2Project1UserMigration2.Profile = "Profile 1";

            UserMigration client2Project1UserMigration3 = new UserMigration();
            client2Project1UserMigration3.Reference = "entryps3";
            client2Project1UserMigration3.Source = "c7-automation3@btcorp7.onmicrosoft.com";
            client2Project1UserMigration3.Target = "c7-automation3@btcorp9.onmicrosoft.com";
            client2Project1UserMigration3.Group = "Wave1";
            client2Project1UserMigration3.Profile = "Profile 1";

            UserMigration client2Project1UserMigration4 = new UserMigration();
            client2Project1UserMigration4.Reference = "entryps4";
            client2Project1UserMigration4.Source = "c7-automation4@btcorp7.onmicrosoft.com";
            client2Project1UserMigration4.Target = "c7-automation4@btcorp9.onmicrosoft.com";
            client2Project1UserMigration4.Group = "Wave1";
            client2Project1UserMigration4.Profile = "Profile 1";

            UserMigration client2Project1UserMigration5 = new UserMigration();
            client2Project1UserMigration5.Reference = "entry1";
            client2Project1UserMigration5.Source = "C7O365SML20@btcorp7.onmicrosoft.com";
            client2Project1UserMigration5.Target = "C7O365SML20@btcorp9.onmicrosoft.com";

            UserMigration client2Project1UserMigration6 = new UserMigration();
            client2Project1UserMigration6.Reference = "entry2";
            client2Project1UserMigration6.Source = "C7O365SML21@btcorp7.onmicrosoft.com";
            client2Project1UserMigration6.Target = "C7O365SML21@btcorp9.onmicrosoft.com";

            UserMigration client2Project1UserMigration7 = new UserMigration();
            client2Project1UserMigration7.Reference = "entry3";
            client2Project1UserMigration7.Source = "C7O365SML25@btcorp7.onmicrosoft.com";
            client2Project1UserMigration7.Target = "C7O365SML25@btcorp9.onmicrosoft.com";

            UserMigration client2Project1UserMigration8 = new UserMigration();
            client2Project1UserMigration8.Reference = "entry4";
            client2Project1UserMigration8.Source = "C7O365SML26@btcorp7.onmicrosoft.com";
            client2Project1UserMigration8.Target = "C7O365SML26@btcorp9.onmicrosoft.com";

            client2Project1.UserMigrations.AddRange(new[]
            {
                client2Project1UserMigration1,
                client2Project1UserMigration1x500,
                client2Project1UserMigration2,
                client2Project1UserMigration3,
                client2Project1UserMigration4,
                client2Project1UserMigration5,
                client2Project1UserMigration6,
                client2Project1UserMigration7,
                client2Project1UserMigration8
            });

            Group client2Project1Group1 = new Group();
            client2Project1Group1.Reference = "adgroup1";
            client2Project1Group1.Name = "c7toc9smlgrp2";

            client2Project1.Groups.Add(client2Project1Group1);

            client2.Projects.Add(client2Project1);
            #endregion

            settings.Clients.Add(client1);
            settings.Clients.Add(client2);

            #endregion

            var serializer = new XmlSerializer(typeof(Settings));

            using (var ms = new IO.MemoryStream())
            {
                serializer.Serialize(ms, settings);
                ms.Flush();
                ms.Position = 0;

                using (var sr = new IO.StreamReader(ms))
                {
                    var xml = sr.ReadToEnd();
                }

            }

        }


        private Settings GetSettings(string path)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            using (IO.FileStream settingsStream = new IO.FileStream(path, IO.FileMode.Open))
            {
                return (Settings)serializer.Deserialize(settingsStream);
            }
        }
    }
}
