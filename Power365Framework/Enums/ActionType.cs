using System.ComponentModel;

namespace BinaryTree.Power365.AutomationFramework.Enums
{
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
