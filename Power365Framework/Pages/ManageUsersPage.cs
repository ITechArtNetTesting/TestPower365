using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;

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

        public void DeleteUserMigrationsJobsLogs(string downloadPath)
        {
            FileInfo[] downloadedFiles = new DirectoryInfo(downloadPath).GetFiles("user-migrations-*.csv");
            foreach (var file in downloadedFiles)
            {
                file.Delete();
            }
        }

        public bool? CheckUserMigrationLogs(string downloadPath, int timeout)
        {
            IWebElement element = FindVisibleElement(_lastPage);
            element.Click();
            WaitForLoadComplete();
            var numberOfRows = (Convert.ToInt32(element.Text) - 1) * 10 + WebDriver.FindElements(_usersRows).Count;
            DirectoryInfo directoryInfo = new DirectoryInfo(downloadPath);
            DefaultWait<DirectoryInfo> wait = new DefaultWait<DirectoryInfo>(directoryInfo);
            wait.Timeout = TimeSpan.FromSeconds(timeout);
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            Func<DirectoryInfo, bool> fileIsDownloaded = new Func<DirectoryInfo, bool>((DirectoryInfo info) =>
            {
                var path = info.GetFiles("user-migrations-*.csv")[0].FullName;
                var test = info.GetFiles("user-migrations-*.csv").Count() >= 1;
                ExcelReader exlRead = new ExcelReader();
                int rowCount = exlRead.GetRowsCount(path);
                return test && rowCount == numberOfRows + 1;
            });
            try
            {
                return wait.Until(fileIsDownloaded);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public InputElement SearchInput
        {
            get
            {
                return new InputElement(_searchInput, WebDriver);
            }
        }

        private readonly By _lastPage = By.XPath("//ul[@class='pagination']//li[child::a[contains(@data-bind,'Number')]][last()]/a");

        private readonly By _usersRows = By.XPath("//div[@id='users']//table[@data-bind]//tbody//tr[child::td]");

        private readonly By _selectAllUsersButton = By.XPath("//div[@id='users']//*[contains(@data-bind,'allSelect')]"); 

        private static readonly By _locator = By.Id("manageUsersContainer");      
        //@@@ REQ:ID
        private readonly By _usersTable = By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]");

          private readonly By _wavesTable = By.XPath("//div[contains(@id, 'waves')]//table[contains(@class, 'table-expanded')]");
        //@@@ REQ:ID      

        private readonly By _migrationWaveTab = By.XPath("//a[contains(@href,'waves')]//span");
        private readonly By _usersTab = By.XPath("//a[contains(@href,'users')]//span");

        private readonly By _addWave = By.XPath("//span[contains(@data-bind,'addWave')]");

        private readonly By _searchInput = By.XPath("//div[contains(@class, 'search-group')]//input"); 
        private readonly By _searchButton = By.XPath("//i[contains(@class,'search')]");
        private readonly By _modalDialogOkButton = By.XPath("//*[contains(@class,'modal-footer')]//span[contains(text(), 'Ok')]");
        //wave
            
        private readonly string _modalDialogWaveNameRadioFormat = "//*[contains(@class,'modal-dialog')]//div[contains(@class, 'radio')]//span[contains(text(), '{0}')]";
        private readonly string _navigationTabFormat = ("//*[contains(@class, 'nav nav-tabs')]/li/a/*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]]");
        private readonly static string USER_MIGRATIONS_FILE_NAME = "user-migrations-*.csv";

        //  mailOnly
        private readonly By _addOrFixUsersButton = By.XPath("//a[contains(@data-bind, 'uploadUser')]");
        public ManageUsersPage(IWebDriver webDriver)
                : base(_locator, webDriver) { }

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

        public void SwichToTab(String tabName)
        {
            WaitForLoadComplete();
            var tab=String.Format(_navigationTabFormat, tabName.ToLower());
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
               
        public UploadFileDialog AddOrFixUserClick()
        {
            return ClickModelElementBy<UploadFileDialog>(_addOrFixUsersButton);
        }

       
    }
}
