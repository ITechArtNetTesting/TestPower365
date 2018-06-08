using System;
using System.Collections.Generic;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class DistributionGroup : Group
    {
        public string Mail { get; set; }
        public string Owner { get; set; }
        public List<string> Members { get; set; }
    }
}