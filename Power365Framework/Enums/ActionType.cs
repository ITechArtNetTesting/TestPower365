using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BinaryTree.Power365.AutomationFramework.Enums
{
    public enum ProjectType: ushort
    {
        None,
        Integration,
        EmailWithDiscovery,
        EmailFromFile,
        EmailFromFileOnPrem
    }

    public enum StateType: byte
    {
        [Display(Name = "Ready")]
        [Description("Ready")]
        Ready,
        [Display(Name = "Syncing")]
        [Description("Syncing")]
        Syncing,
        [Display(Name = "sync(s) complete")]//Can be regular expressions maybe?
        [Description("Synced")]
        Synced,
        [Display(Name = "Sync Error")]
        [Description("Sync Error")]
        SyncError,
        [Display(Name = "Finalizing")]
        [Description("Finalizing")]
        Finalizing,
        [Display(Name = "Complete")]
        [Description("Complete")]
        Complete,
        [Display(Name = "Preparing")]
        [Description("Preparing")]
        Preparing,
        [Display(Name = "Prepared")]
        [Description("Prepared")]
        Prepared,
        [Display(Name = "Stopping")]
        [Description("Stopping")]
        Stopping,
        [Display(Name = "No Match")]
        [Description("No Match")]
        NoMatch,
        [Display(Name = "Rollback In Progress")]
        [Description("Rollback In Progress")]
        RollbackInProgress,
        [Display(Name = "Rollback Complete")]
        [Description("Rollback Complete")]
        RollbackCompleted,
        [Display(Name = "1 sync(s) complete")]
        [Description("1 sync(s) complete")]
        Synced1,
        [Display(Name = "2 sync(s) complete")]
        [Description("2 sync(s) complete")]
        Synced2
    }

    public enum ActionType : byte
    {
        [Description("Sync")]
        Sync,
        [Description("Stop")]
        Stop,
        [Description("Prepare")]
        Prepare,
        [Description("Cutover")]
        Cutover,
        [Description("Complete")]
        Complete,
        [Description("Archive")]
        Archive,
        [Description("Add To Wave")]
        AddToWave,
        [Description("Add To Profile")]
        AddToProfile,
        [Description("Rollback")]
        Rollback,
        [Description("Dismiss")]
        Dismiss,
        [Description("Export")]
        Export
    }
}
