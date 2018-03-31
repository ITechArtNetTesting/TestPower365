using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.PageObjects.Interfaces
{
    public interface IPublicFolderMigrationPage
    {
        void SelectAllPublicFolders();
        void SelectArchiveAction();
        void ClickApplyActionButton();
        void ClickBackToDashboardButton();
        void ClickYesSureArchiveButton();
        void ClickActionsDropdown();
    }
}
