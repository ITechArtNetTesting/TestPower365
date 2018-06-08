using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BinaryTree.Power365.AutomationFramework.Enums
{
    public enum StateType: byte
    {
        [Display(Name = "None")]
        [Description("None")]
        None,
        [Display(Name = "Ready")]
        [Description("Ready")]
        Ready,
        [Display(Name = "Matched")]
        [Description("Matched")]
        Matched,
        [Display(Name = "No Match")]
        [Description("No Match")]
        NoMatch,
        [Display(Name = "Multiple Matches")]
        [Description("Multiple Matches")]
        MultipleMatches,
        [Display(Name = "Preparing")]
        [Description("Preparing")]
        Preparing,
        [Display(Name = "Prepared")]
        [Description("Prepared")]
        Prepared,
        [Display(Name = "Syncing")]
        [Description("Syncing")]
        Syncing,
        [Display(Name = "sync(s) complete")]//Can be regular expressions maybe?
        [Description("Synced")]
        Synced,
        [Display(Name = "Stopping")]
        [Description("Stopping")]
        Stopping,
        [Display(Name = "Finalizing")]
        [Description("Finalizing")]
        Finalizing,
        [Display(Name = "Complete")]
        [Description("Complete")]
        Complete,
        [Display(Name = "Prepare Error")]
        [Description("Prepare Error")]
        PrepareError,
        [Display(Name = "Sync Error")]
        [Description("Sync Error")]
        SyncError,
        [Display(Name = "Cutover Error")]
        [Description("Cutover Error")]
        CutoverError,
        [Display(Name = "Wave Error")]
        [Description("Wave Error")]
        WaveError,
        [Display(Name = "Rollback In Progress")]
        [Description("Rollback In Progress")]
        RollbackInProgress,
        [Display(Name = "Rollback Complete")]
        [Description("Rollback Complete")]
        RollbackCompleted,
        [Display(Name = "Rollback Error")]
        [Description("Rollback Error")]
        RollbackError,
        [Display(Name = "Moving")]
        [Description("Moving")]
        Moving,
        [Display(Name = "Move Error")]
        [Description("Move Error")]
        MoveError,
        [Display(Name = "Moved")]
        [Description("Moved")]
        Moved,
        [Display(Name = "Matching")]
        [Description("Matching")]
        Matching,

        [Display(Name = "1 sync(s) complete")]
        [Description("1 sync(s) complete")]
        Synced1,
        [Display(Name = "2 sync(s) complete")]
        [Description("2 sync(s) complete")]
        Synced2
    }
}