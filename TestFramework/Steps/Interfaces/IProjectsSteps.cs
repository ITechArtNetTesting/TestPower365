using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Steps.Interfaces
{
    public interface IProjectSteps
    {
        void AddNewProject(string testName, string sourceTenant, string sourcePassword, string targetTenant, string targetPassword, string fileName);
    }
}
