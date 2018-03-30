using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.PageObjects.Interfaces
{
    public interface IProjectGroupsPage
    {
        void SearchGroupByName(string name);
        void ClickTargetGroup();
    }
}
