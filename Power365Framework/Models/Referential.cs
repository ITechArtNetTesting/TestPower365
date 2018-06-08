using System;
using System.Xml.Serialization;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public abstract class Referential : ReferenceStore
    {
        [XmlAttribute]
        public string Reference { get; set; }
    }
}