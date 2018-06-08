using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;

namespace BinaryTree.Power365.AutomationFramework.Services
{
    public class BrowserAutomation : IDisposable
    {
        public IWebDriver WebDriver { get { return _webDriver; } }

        private readonly IWebDriver _webDriver;
        private readonly Settings _settings;
        private readonly object _context;

        private Uri _remoteWebDriverClusterUri;
        private string _testSpecificDownloadsFolder;

        private ILog _logger = LogManager.GetLogger(typeof(BrowserAutomation));

        public BrowserAutomation(Settings settings, object context = null)
        {
            _context = context;
            _settings = settings;
            _webDriver = GetDriver();

            //RemoteWebDriver does not support Maximize command
            if (_webDriver is RemoteWebDriver)
                _webDriver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            else
                _webDriver.Manage().Window.Maximize();
        }

        public T Navigate<T>(string url)
            where T : PageBase
        {
            _webDriver.Navigate().GoToUrl(url);
            return (T)Activator.CreateInstance(typeof(T), new[] { _webDriver });
        }
        
        public bool IsFileDownloaded(string searchPattern, int timeoutInSec = 5, int pollIntervalInSec = 0)
        {
            var directoryInfo = new DirectoryInfo(_testSpecificDownloadsFolder);

            var wait = new DefaultWait<DirectoryInfo>(directoryInfo);
            wait.Timeout = TimeSpan.FromSeconds(timeoutInSec);
            wait.PollingInterval = TimeSpan.FromSeconds(pollIntervalInSec);

            try
            {
                return wait.Until((DirectoryInfo info) =>
                {
                    if (WebDriver is RemoteWebDriver)
                        TransferFilesFromRemoteWebDriver();

                    return directoryInfo.GetFiles(searchPattern).Count() >= 1; ;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public FileStream GetDownloadedFileStream(string fileName)
        {
            if (WebDriver is RemoteWebDriver)
                TransferFilesFromRemoteWebDriver();

            var filePath = Path.Combine(_testSpecificDownloadsFolder, fileName);

            return new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 2048, FileOptions.DeleteOnClose);
        }

        public void Dispose()
        {
            if (_webDriver != null)
            {
                _webDriver.Close();
                _webDriver.Quit();
                _webDriver.Dispose();
            }

            RemoveDownloadsDirectory();
        }
        
        protected IWebDriver GetDriver()
        {
            var testDirectory = CleanDirectoryName((string)_context);
            _testSpecificDownloadsFolder = Path.Combine(_settings.DownloadsPath, testDirectory);
            
            var chromeDriverDirectory = Path.GetFullPath(_settings.ChromeDriverPath) ?? Directory.GetCurrentDirectory();
            //@@@work in progress
            string browser = string.Empty;
            string settings = string.Empty;
            if(_settings.Browser.Contains("remote"))
            {
                browser = "remote";
                //remote|uri=http://blah|browser=chrome|version=66.0|capabilities=enableVNC,true;screenResolution,1920x1080x24;sessionTimeout=1000"
                //var parts = _settings.Browser.Split('|');

                //browser = parts[0];
                //settings = parts[1];
            }
            else
            {
                browser = _settings.Browser;
            }

            switch (browser)
            {
                case "remote":
                    _remoteWebDriverClusterUri = new Uri("http://10.1.17.229:4444/wd/hub");

                    var capabilities = new DesiredCapabilities("chrome", "66.0", new Platform(PlatformType.Any));
                    capabilities.SetCapability("enableVNC", true);
                    capabilities.SetCapability("screenResolution", "1920x1080x24");
                    capabilities.SetCapability("sessionTimeout", 20 * 60);

                    if (_context != null && _context is string)
                        capabilities.SetCapability("name", _context);

                    return new RemoteWebDriver(_remoteWebDriverClusterUri, capabilities, TimeSpan.FromMinutes(10));
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk",
                        "application/octet-stream;application/csv;text/csv;application/vnd.ms-excel;");
                    return new FirefoxDriver(firefoxOptions);
                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
                    chromeOptions.AddUserProfilePreference("download.default_directory", Path.GetFullPath(_testSpecificDownloadsFolder));
                    return new ChromeDriver(Path.GetFullPath(chromeDriverDirectory), chromeOptions);
            }
        }
        
        private void TransferFilesFromRemoteWebDriver()
        {
            if (WebDriver is RemoteWebDriver)
            {
                //Clear and rebuild, we will download all files again.
                RemoveDownloadsDirectory();
                CreateDownloadsDirectory();

                var remoteWebDriver = WebDriver as RemoteWebDriver;
                var sessionId = remoteWebDriver.SessionId.ToString();

                var downloadPath = string.Format("{0}://{1}/download/{2}/", _remoteWebDriverClusterUri.Scheme, _remoteWebDriverClusterUri.Authority, sessionId);

                using (WebClient client = new WebClient())
                {
                    var downloadsData = client.DownloadString(downloadPath);

                    var fileNames = XElement.Parse(downloadsData)
                       .Descendants("a")
                       .Select(x => x.Attribute("href").Value).ToArray();

                    
                    foreach (var file in fileNames)
                    {
                        if (string.IsNullOrWhiteSpace(file))
                            continue;

                        _logger.DebugFormat("Transfering file from remote host: {0}", file);
                        var localFileLocation = Path.Combine(_testSpecificDownloadsFolder, file);
                        client.DownloadFile(string.Format("{0}{1}", downloadPath, file), localFileLocation);
                    }
                }
            }
            else
            {
                throw new Exception("WebDriver is not a RemoteWebDriver");
            }
        }
        
        private void CreateDownloadsDirectory()
        {
            _logger.DebugFormat("Creating Downloads Directory: {0}", _testSpecificDownloadsFolder);
            if (!Directory.Exists(_testSpecificDownloadsFolder))
                Directory.CreateDirectory(_testSpecificDownloadsFolder);               
          
        }

        private void RemoveDownloadsDirectory()
        {
            _logger.DebugFormat("Removing Downloads Directory: {0}", _testSpecificDownloadsFolder);
            if (Directory.Exists(_testSpecificDownloadsFolder))
                Directory.Delete(_testSpecificDownloadsFolder, true);
        }

        private string CleanDirectoryName(string directoryName)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                directoryName = directoryName.Replace(c.ToString(), "");
            }
            return directoryName;
        }
    }
}
