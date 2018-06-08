using System;

namespace BinaryTree.Power365.Test
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    sealed class TestResourceAttribute : Attribute
    {
        public string Client { get; set; }
        public string Project { get; set; }
        public string Entry { get; set; }


        public TestResourceAttribute(string client, string project, string entry)
        {
            Client = client;
            Project = project;
            Entry = entry;
        }
    }
}