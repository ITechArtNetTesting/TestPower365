using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Steps.Interfaces
{
    public interface IProjectSteps
    {
        void AddNewEmailFromFileProject(string testName, string sourceTenant, string sourcePassword, string targetTenant, string targetPassword, string fileName);
        void AddNewEmailWithDiscoveryProject(string projectName,string sourceUserLogin,string sourceUserPassword,string targetUserLogin,string targetUserPassword);
        void ViewGroups(string projectName,string groupName);
    }
}
