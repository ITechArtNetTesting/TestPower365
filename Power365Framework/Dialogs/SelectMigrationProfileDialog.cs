using System;
using OpenQA.Selenium;
using log4net;

namespace BinaryTree.Power365.AutomationFramework.Dialogs
{
    public class SelectMigrationProfileDialog : ModalDialogBase
    {
        public SelectMigrationProfileDialog(IWebDriver webDriver) 
            : base(LogManager.GetLogger(typeof(SelectMigrationProfileDialog)), webDriver) { }

        private readonly string _profileLabelFormat = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]//label";
        private readonly string _profileRadioFormat = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]//input";
        private readonly string _profileActionFormat = "//div[contains(@class, 'modal in')]//tr[.//*[contains(text(), '{0}')]]//td[.//*[contains(text(), '{1}')]]";
        
        public bool  IsProfileActionVisible(string profileName, string actionName)
        {
            var _profileAction = By.XPath(String.Format(_profileActionFormat, profileName, actionName));
            return IsElementVisible(_profileAction);
        }
    }
}
