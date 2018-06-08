using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ManageUsersPage : ActionPageBase
    {
        public TableElement UsersTable
        {
            get
            {
                return new TableElement(_usersTable, WebDriver);
            }
        }

        public TableElement WavesTable
        {
            get
            {
                return new TableElement(_wavesTable, WebDriver);
            }
        }

        public Element UsersTab
        {
            get
            {
                return new Element(_usersTab, WebDriver);
            }
        }

        public Element MigrationWavesTab
        {
             get
            {
                  return new Element(_migrationWaveTab, WebDriver);
            }
        }

        public ButtonElement AddWaveButton
        {
            get
            {
                return new ButtonElement(_addWave, WebDriver);
            }
        }

        public InputElement SearchInput
        {
            get
            {
                return new InputElement(_searchInput, WebDriver);
            }
        }

        public const string MIGRATION_WAVE_DELETE = "/ancestor::tr//*[contains(text(), 'Delete')]/ancestor::a";
        private const string USER_MIGRATIONS_FILE_NAME = "user-migrations-*.csv";

        private static readonly By _locator = By.Id("manageUsersContainer");

        private readonly By _selectAllUsersButton = By.XPath("//div[@id='users']//*[contains(@data-bind,'allSelect')]");

        private readonly By _usersTable = By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]");
        private readonly By _wavesTable = By.XPath("//div[contains(@id, 'waves')]//table[contains(@class, 'table-expanded')]");

        private readonly By _migrationWaveTab = By.XPath("//a[contains(@href,'waves')]//span");
        private readonly By _usersTab = By.XPath("//a[contains(@href,'users')]//span");

        private readonly By _addWave = By.XPath("//span[contains(@data-bind,'addWave')]");

        private readonly By _searchInput = By.XPath("//div[contains(@class, 'search-group')]//input"); 
        private readonly By _searchButton = By.XPath("//i[contains(@class,'search')]");
        private readonly By _modalDialogOkButton = By.XPath("//*[contains(@class,'modal-footer')]//span[contains(text(), 'Ok')]");
        //wave
            
        private readonly string _modalDialogWaveNameRadioFormat = "//*[contains(@class,'modal-dialog')]//div[contains(@class, 'radio')]//span[contains(text(), '{0}')]";
        private readonly string _navigationTabFormat = ("//*[contains(@class, 'nav nav-tabs')]/li/a/*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]]");
        
        //  mailOnly
        private readonly By _addOrFixUsersButton = By.XPath("//a[contains(@data-bind, 'uploadUser')]");

        public ManageUsersPage(IWebDriver webDriver)
                : base(LogManager.GetLogger(typeof(ManageUsersPage)), _locator, webDriver) { }

        public bool IsUserState(string user, StateType state, int timeoutInSec = 5, int pollIntervalInSec = 0)
        {
            return UsersTable.RowHasValue(user, state.GetDisplay(), timeoutInSec, pollIntervalInSec);
        }             

        public UserDetailsDialog OpenUserDetails(string user)
        {
            return UsersTable.DoubleClickRowByValue<UserDetailsDialog>(user);
        }

        public void SelectAllUsers()
        {

            var selectAllUsers= FindExistingElement(_selectAllUsersButton, 10);
            selectAllUsers.Click();
        }

        public void SwichToTab(string tabName)
        {
            WaitForLoadComplete();
            var tab = string.Format(_navigationTabFormat, tabName.ToLower());
            ClickElementBy(By.XPath(tab));
        }
       
        public bool CheckMigrationWavesTabOpen()
        {
            return AddWaveButton.IsVisible();
        }

        public bool MigrationWaveTabIsVisible()
        {
            return MigrationWavesTab.IsVisible();
        }

        public EditWavePage clickNewWaveButton()
        {
            return AddWaveButton.Click<EditWavePage>() ;
        }

        public void Search(string query)
        {
            SearchInput.SendKeys(query);
            ClickElementBy(_searchButton);
        }

        public void SelectWave(string waveName)
        {
            var waveNameModalWindow = By.XPath(string.Format(_modalDialogWaveNameRadioFormat, waveName));
            ClickElementBy(waveNameModalWindow);
            ClickElementBy(_modalDialogOkButton);
        }

        public void SelectWave()
        {
            //create
        }
               
        public UploadFileDialog AddOrFixUser()
        {
            return ClickModalElementBy<UploadFileDialog>(_addOrFixUsersButton);
        }
    }
}
