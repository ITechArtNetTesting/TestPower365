using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Enums
{
    public enum ContentType
    {
        [Description("mail")] Email,
        [Description("calendar")] Calendar,
        [Description("contacts")] Contacts,
        [Description("tasks")] Tasks,
        [Description("notes")] Notes
    }
}
