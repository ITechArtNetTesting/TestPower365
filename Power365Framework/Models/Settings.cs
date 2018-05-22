using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BinaryTree.Power365.AutomationFramework
{

    public enum ProjectType: ushort
    {
        None = 0,
        Integration = 1,
        EmailWithDiscovery = 2,
        EmailByFile = 3,
        OnPremEmailByFile = 4
    }

    public enum UserListOption: ushort
    {
        None = 0,
        UploadFile = 1,
        KeepExistings = 2
    }
    
    public enum DiscoveryMethod: ushort
    {
        None = 0,
        All = 1,
        Group = 2
    }

    public enum SyncIntervalBy: ushort
    {
        None = 0,
        Hours = 1,
        Days = 2,
        Weeks = 3
    }

    public enum AddressBookSyncOption: ushort
    {
        None = 0,
        SourceToTarget = 1,
        TargetToSource = 2,
        BothDirections = 3
    }

    public enum FreeBusySyncOption: ushort
    {
        None = 0,
        AllUsers = 1,
        Group = 2
    }

    public enum PublicFolderSyncOption: ushort
    {
        None = 0,
        UploadFile = 1,
        Manual = 2,
        KeepExisting
    }

    [Serializable]
    public class MigrationWave
    {
        public string Name { get; set; }
        public string Group { get; set; }
    }

    [Serializable]
    public class SyncSchedule
    {
        public DateTime StartOnDateTime { get; set; }
        public uint Interval { get; set; }
        public uint MaxSyncCount { get; set; }
        public SyncIntervalBy IntervalBy { get; set; }
    }

    [Serializable]
    public class WorkflowSettings : Referential
    {
        public override void BuildReferences()
        {
        }
    }

    [Serializable]
    public class EditProjectWorkflowSettings: WorkflowSettings
    {
        public ProjectType ProjectType { get; set; }

        public List<Tenant> Tenants { get; set; }
        public List<MigrationWave> MigrationWaves { get; set; }
        public List<SyncSchedule> SyncSchedules { get; set; }

        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string UsersDiscoveryGroup { get; set; }
        public string AddressBookSyncDiscoveryGroup { get; set; }
        public string FreeBusyDiscoveryGroup { get; set; }

        public UserListOption UserListOption { get; set; }
        public AddressBookSyncOption AddressBookSyncOption { get; set; }
        public FreeBusySyncOption FreeBusySyncOption { get; set; }
        public PublicFolderSyncOption PublicFolderSyncOption { get; set; }
        public DiscoveryMethod UsersDiscoveryMethod { get; set; }
        public DiscoveryMethod GroupsDiscoveryMethod { get; set; }

        public bool CreateUsers { get; set; }
        public bool CreateGroups { get; set; }

        public EditProjectWorkflowSettings()
        {
            Tenants = new List<Tenant>();
            MigrationWaves = new List<MigrationWave>();
            SyncSchedules = new List<SyncSchedule>();
        }
    }

    public abstract class ReferenceStore
    {
        private Dictionary<string, object> _referenceLookup = new Dictionary<string, object>();

        public abstract void BuildReferences();

        internal void AddReference(Referential referential)
        {
            if (referential == null || referential.Reference == null)
                return;

            if (_referenceLookup.ContainsKey(referential.Reference))
                throw new Exception(string.Format("Reference '{0}' already exists.", referential.Reference));
            _referenceLookup.Add(referential.Reference, referential);
        }

        public T GetByReference<T>(string reference)
            where T : Referential
        {
            if (!_referenceLookup.ContainsKey(reference))
                throw new Exception(string.Format("Reference '{0}' not found.", reference));
            return (T)_referenceLookup[reference];
        }
    }

    [Serializable]
    public abstract class Referential : ReferenceStore
    {
        [XmlAttribute]
        public string Reference { get; set; }
    }

    [Serializable]
    public class Credential : Referential
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override void BuildReferences() { }
    }

    [Serializable]
    public class Tenant : Referential
    {
        public List<Credential> Credentials { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        public string PrimaryDomain { get; set; }
        public string SecondaryDomain { get; set; }
        public string AzureADSyncServer { get; set; }
        public string ExchangePowerShellUrl { get; set; }

        public Tenant()
        {
            Credentials = new List<Credential>();
        }

        public override void BuildReferences()
        {
            foreach (var credential in Credentials)
            {
                AddReference(credential);
                credential.BuildReferences();
            }
        }

    }

    [Serializable]
    public class UserMigration : Referential
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string Group { get; set; }
        public string Profile { get; set; }

        public override void BuildReferences() { }
    }

    [Serializable]
    public class Group : Referential
    {
        public string Name { get; set; }

        public override void BuildReferences() { }
    }

    [Serializable]
    public class Database : Referential
    {
        public string InitialCatalog { get; set; }
        public Credential Credential { get; set; }

        public override void BuildReferences()
        {
            if (Credential != null)
            {
                AddReference(Credential);
                Credential.BuildReferences();
            }

        }

    }

    [Serializable]
    public class DistributionGroup : Group
    {
        public string Mail { get; set; }
        public string Owner { get; set; }
        public List<string> Members { get; set; }
    }

    [Serializable]
    public class File : Referential
    {
        public string Path { get; set; }

        public override void BuildReferences() { }
    }

    [Serializable]
    public class Project : Referential
    {
        [XmlAttribute]
        public string Source { get; set; }
        [XmlAttribute]
        public string Target { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserMigration> UserMigrations { get; set; }
        public List<Group> Groups { get; set; }
        public List<File> Files { get; set; }

        public Project()
        {
            UserMigrations = new List<UserMigration>();
            Groups = new List<Group>();
            Files = new List<File>();
        }

        public override void BuildReferences()
        {
            foreach (var userMigration in UserMigrations)
            {
                AddReference(userMigration);
                userMigration.BuildReferences();
            }

            foreach (var group in Groups)
            {
                AddReference(group);
                group.BuildReferences();
            }

            foreach (var file in Files)
            {
                AddReference(file);
                file.BuildReferences();
            }
        }

    }

    [Serializable]
    public class Client : Referential
    {
        public string Name { get; set; }
        public Credential Administrator { get; set; }
        public List<Project> Projects { get; set; }

        public Client()
        {

            Projects = new List<Project>();
        }

        public override void BuildReferences()
        {
            if (Administrator != null)
            {
                AddReference(Administrator);
                Administrator.BuildReferences();
            }

            foreach (var project in Projects)
            {
                AddReference(project);
                project.BuildReferences();
            }
        }
    }

    [Serializable]
    public class Settings : ReferenceStore
    {
        public List<Database> Databases { get; set; }
        public List<Client> Clients { get; set; }
        public List<Tenant> Tenants { get; set; }

        public string BaseUrl { get; set; }
        public string DownloadsPath { get; set; }
        public string ChromeDriverPath { get; set; }
        public string O365PowerShellUrl { get; set; }
        public string MSOLConnectArgs { get; set; }
        public int TimeoutSec { get; set; }
        public string Browser { get; set; }
        public string Bitness { get; set; }
        public string CorrectHelpURL { get; set; }

        public Settings()
        {
            Databases = new List<Database>();
            Clients = new List<Client>();
            Tenants = new List<Tenant>();
        }

        public override void BuildReferences()
        {
            foreach (var db in Databases)
            {
                AddReference(db);
                db.BuildReferences();
            }

            foreach (var client in Clients)
            {
                AddReference(client);
                client.BuildReferences();
            }

            foreach (var tenant in Tenants)
            {
                AddReference(tenant);
                tenant.BuildReferences();
            }
        }
    }

}
