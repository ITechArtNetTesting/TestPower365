using System;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class File : Referential
    {
        public string Path { get; set; }

        public override void BuildReferences() { }
    }
}