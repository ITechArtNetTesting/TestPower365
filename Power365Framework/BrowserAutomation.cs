using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Workflows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace BinaryTree.Power365.AutomationFramework
{
    public class BrowserAutomation : IDisposable
    {
        public IWebDriver WebDriver { get { return _webDriver; } }

        private readonly IWebDriver _webDriver;
        private readonly Settings _settings;

        public BrowserAutomation(Settings settings)
        {
            _settings = settings;
            _webDriver = GetDriver();
            _webDriver.Manage().Window.Maximize();
        }

        public T Navigate<T>(string url)
            where T : PageBase
        {
            _webDriver.Navigate().GoToUrl(url);
            return (T)Activator.CreateInstance(typeof(T), new[] { _webDriver });
        }

        public TW CreateWorkflow<TW, TP>(string url)
            where TW: WorkflowBase
            where TP: PageBase
        {
            TP rootPage = Navigate<TP>(url);
            return (TW)Activator.CreateInstance(typeof(TW), rootPage, _webDriver);
        }

        public TW CreateWorkflow<TW, TP>(TP rootPage)
            where TW : WorkflowBase
            where TP : PageBase
        {
            return (TW)Activator.CreateInstance(typeof(TW), rootPage, _webDriver);
        }

        public void Dispose()
        {
            if (_webDriver != null)
            {
                _webDriver.Close();
                _webDriver.Quit();
                _webDriver.Dispose();
            }
        }

        protected IWebDriver GetDriver()
        {
            var downloadDirectory = Path.GetFullPath(_settings.DownloadsPath) ?? Directory.GetCurrentDirectory();
            var chromeDriverDirectory = Path.GetFullPath(_settings.ChromeDriverPath) ?? Directory.GetCurrentDirectory();

            switch (_settings.Browser)
            {
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk",
                        "application/octet-stream;application/csv;text/csv;application/vnd.ms-excel;");
                    return new FirefoxDriver(firefoxOptions);
                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
                    chromeOptions.AddUserProfilePreference("download.default_directory", Path.GetFullPath(downloadDirectory));
                    return new ChromeDriver(Path.GetFullPath(chromeDriverDirectory), chromeOptions);
            }
        }
    }
}
