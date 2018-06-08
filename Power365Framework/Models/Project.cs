using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class Project : Referential
    {
        [XmlAttribute]
        public string Source { get; set; }
        [XmlAttribute]
        public string Target { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserMigration> UserMigrations { get; set; }
        public List<Group> Groups { get; set; }
        public List<File> Files { get; set; }

        public Project()
        {
            UserMigrations = new List<UserMigration>();
            Groups = new List<Group>();
            Files = new List<File>();
        }

        public override void BuildReferences()
        {
            foreach (var userMigration in UserMigrations)
            {
                AddReference(userMigration);
                userMigration.BuildReferences();
            }

            foreach (var group in Groups)
            {
                AddReference(group);
                group.BuildReferences();
            }

            foreach (var file in Files)
            {
                AddReference(file);
                file.BuildReferences();
            }
        }

    }
}