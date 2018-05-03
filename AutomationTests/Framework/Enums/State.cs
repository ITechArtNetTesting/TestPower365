using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Enums
{
    public enum State
    {
        [Description("Syncing")]
        Syncing,
        [Description("Synced")]
        Synced,
        [Description("Finalizing")]
        Finalizing,
        [Description("Complete")]
        Complete,
        [Description("Preparing")]
        Preparing,
        [Description("Prepared")]
        Prepared,
        [Description("Stopping")]
        Stopping,
        [Description("No Match")]
        NoMatch,
        [Description("Rollback In Progress")]
        RollbackInProgress,
        [Description("Rollback Complete")]
        RollbackCompleted,
        [Description("Provisioning")]
        Provisioning,
        [Description("1 sync(s) complete")]
        Synced1,
        [Description("2 sync(s) complete")]
        Synced2,
        [Description("Matched")]
        Matched,
        [Description("Stopped")]
        Stopped,
        [Description("Sync Error")]
        SyncError

    }
}
