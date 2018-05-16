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
        [Display(Name = "Syncing")]
        [Description("Syncing")]
        Syncing,
        [Display(Name = "sync(s) complete")]//Can be regular expressions maybe?
        [Description("Synced")]
        Synced,
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
        RollbackCompleted
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
        Rollback
    }
}
