using System;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class UserMigration : Referential
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string Group { get; set; }
        public string Profile { get; set; }

        public override void BuildReferences() { }
    }
}