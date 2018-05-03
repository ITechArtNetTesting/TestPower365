using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;

namespace TP365Framework.Drivers
{
    public class Driver:IDriver
    {
        private string startPage { get; set; }

        private WebBrowsers browser;        

        private Guid driverKey;

        private static readonly IDictionary<Guid, IWebDriver> Drivers = new Dictionary<Guid, IWebDriver>();

        public Driver(WebBrowsers browser,string startPage)
        {
            this.startPage = startPage;
            this.browser = browser;            
            InitBrowser(browser);
            LoadApplication();
        }        

        public IWebDriver GetDriver
        {
            get
            {
                if (Drivers[driverKey] == null)
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method InitBrowser or create class by constructor.");
                return Drivers[driverKey];
            }
            set
            {
                GetDriver = value;
            }
        }

        private void LoadApplication()
        {
            GetDriver.Url = startPage;
        }

        public void GoToUrl(string url)
        {
            GetDriver.Url = url;
        }

        public void InitBrowser(WebBrowsers browserName)
        {
            switch (browserName)
            {
                case WebBrowsers.Firefox:
                    driverKey = Guid.NewGuid();
                    Drivers.Add(driverKey, new FirefoxDriver());
                    break;
                case WebBrowsers.InternetExplorer:
                    driverKey = Guid.NewGuid();
                    Drivers.Add(driverKey, new InternetExplorerDriver());
                    break;
                case WebBrowsers.Chrome:
                    driverKey = Guid.NewGuid();
                    Drivers.Add(driverKey, new ChromeDriver("Resources/Drivers"));
                    break;
                default:
                    driverKey = Guid.NewGuid();
                    Drivers.Add(driverKey, new ChromeDriver("Resources/Drivers"));
                    break;
            }
        }

        public void CloseAllDrivers()
        {
            foreach (var key in Drivers.Keys)
            {
                Drivers[key].Close();
                Drivers[key].Quit();
            }
            Drivers.Clear();
        }

        public void CloseDriver()
        {
            GetDriver.Close();
            GetDriver.Quit();
            Drivers.Remove(driverKey);
        }
    }
    public enum WebBrowsers
    {
        InternetExplorer,
        Firefox,
        Chrome
    }
}
