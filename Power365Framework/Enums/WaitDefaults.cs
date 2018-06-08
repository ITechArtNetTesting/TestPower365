using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Enums
{
    public static class WaitDefaults
    {
        public const int STATE_PREPARING_TIMEOUT_SEC = 15;
        public const int STATE_PREPARED_TIMEOUT_SEC = 40 * 60;

        public const int STATE_SYNCING_TIMEOUT_SEC = 20 * 60; // Integration project >18 min. Maybe create STATE_INTEGRAT_SYNCING_TIMEOUT_SEC ?
        public const int STATE_SYNCED_TIMEOUT_SEC = 25 * 60;

        public const int STATE_ROLLINGBACK_TIMEOUT_SEC = 15;
        public const int STATE_ROLLEDBACK_TIMEOUT_SEC = 20 * 60;

        public const int FILE_DOWNLOAD_TIMEOUT_SEC = 30;
    }
}
