using System;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class Group : Referential
    {
        public string Name { get; set; }

        public override void BuildReferences() { }
    }
}