using BinaryTree.Power365.AutomationFramework.Enums;
using System;
using System.Collections.Generic;

namespace BinaryTree.Power365.AutomationFramework
{
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
}