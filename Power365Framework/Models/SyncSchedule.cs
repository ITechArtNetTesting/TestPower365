using BinaryTree.Power365.AutomationFramework.Enums;
using System;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class SyncSchedule
    {
        public DateTime StartOnDateTime { get; set; }
        public uint Interval { get; set; }
        public uint MaxSyncCount { get; set; }
        public SyncIntervalBy IntervalBy { get; set; }
    }
}