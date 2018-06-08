using System;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class Credential : Referential
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override void BuildReferences() { }
    }
}