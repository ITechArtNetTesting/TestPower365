using log4net;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class EditWavePage : PageBase
    {
        private static By _locator = By.XPath("//div[contains(@class, 'modal in')]//*[(@class='wizard')]");

        private readonly By _migrationWaveNameText = By.XPath("//*[contains(@class, 'modal-dialog')]//input[@id='waveName'][contains(@data-bind, 'waveName')]");
        private readonly By _migrationWaveGroupSelectorDropdownText = By.Id("groupSelector");
        private readonly By _discoveryGroupDropdownText = By.Id("groupSelector");

        private readonly By _firstTenantMatchGroup = By.XPath("//div[contains(@class, 'modal-body')]//*[contains(@name, 'tenantMatchGroup')]");
        private readonly By _nextButton = By.XPath("//*[contains(@class, 'modal-body')]//*[contains(@class, 'btn-next')]");
        private string selectGroup = "//a[contains(@data-bind, 'selectGroup')][contains(text(), '{0}')]";

        public EditWavePage(IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(EditWavePage)), _locator, webDriver) { }

        public EditWavePage AddMigrationWave(string migrationWave)
        {
            var waveName = FindVisibleElement(_migrationWaveNameText);
            waveName.SendKeys(migrationWave);
            ClickNext();
            return this;
        }

        public EditWavePage SelectTenantMatchGroup()
        {
            var firstMachGroup = FindExistingElement(_firstTenantMatchGroup,5);
            firstMachGroup.Click();
            ClickNext();
            return this;
        }

        public ManageUsersPage AddADGroupName(String groupName)
        {
            var discoveryGroupDropdownText = FindVisibleElement(_discoveryGroupDropdownText);
            discoveryGroupDropdownText.SendKeys(groupName);
            ClickElementBy(_discoveryGroupDropdownText);
            IWebElement ele = FindVisibleElement(By.XPath(String.Format(selectGroup, groupName)));
          //  ClickElementBy(By.XPath(String.Format(selectGroup, groupName)));
            ele.Click();
          
            return ClickElementBy<ManageUsersPage>(_nextButton); 
        }
        
        protected void ClickNext()
        {
            var nextButtonElement = FindClickableElement(_nextButton);
            nextButtonElement.Click();
        }
    }
}
