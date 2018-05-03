using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Enums
{
    public enum ActionType
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
