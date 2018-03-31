using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.PageObjects.Interfaces
{
    public interface IProjectsListPage
    {
        void ClickNewProjectButton();
        void ChooseProjectByName(string projectName);
        void ClickArchiveProjectByName(string projectName);
        void ClickYesToArchiveButton();
    }
}
