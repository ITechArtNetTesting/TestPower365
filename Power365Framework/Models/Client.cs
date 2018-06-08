using System;
using System.Collections.Generic;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class Client : Referential
    {
        public string Name { get; set; }
        public Credential Administrator { get; set; }
        public List<Project> Projects { get; set; }

        public Client()
        {
            Projects = new List<Project>();
        }

        public override void BuildReferences()
        {
            if (Administrator != null)
            {
                AddReference(Administrator);
                Administrator.BuildReferences();
            }

            foreach (var project in Projects)
            {
                AddReference(project);
                project.BuildReferences();
            }
        }
    }
}