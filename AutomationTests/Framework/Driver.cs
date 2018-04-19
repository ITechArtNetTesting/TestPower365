using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework
{
    public class Driver
    {
        public static readonly string DriverPath = @"\\resources";

        string BaseWindow;

        private WebBrowsers browser;

        private Guid driverKey;

        private static readonly IDictionary<Guid, IWebDriver> Drivers = new Dictionary<Guid, IWebDriver>();

        public Driver(WebBrowsers browser)
        {
            this.browser = browser;
            InitBrowser(browser);
            BaseWindow = GetDriver().CurrentWindowHandle;
            //LoadApplication()
        }

        public Guid GetDriverKey()
        {
            return driverKey;
        }

        public IWebDriver GetDriver()
        {
            CheckDriver(driverKey);
            return Drivers[driverKey];
        }

        public static IWebDriver GetDriver(Guid key)
        {
            CheckDriver(key);
            return Drivers[key];
        }

        public void SetDriver(IWebDriver driver)
        {
            CheckDriver(driverKey);
            Drivers[driverKey] = driver;
        }

        public void SetDriver(IWebDriver driver, Guid key)
        {
            CheckDriver(key);
            Drivers[key] = driver;
        }

        public void GoToUrl(string url)
        {
            GetDriver().Url = url;
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
                    Drivers.Add(driverKey, new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory + DriverPath));
                    break;
                default:
                    driverKey = Guid.NewGuid();
                    Drivers.Add(driverKey, new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory + DriverPath));
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
            GetDriver().Close();
            GetDriver().Quit();
            Drivers.Remove(driverKey);
        }
        public void SwitchWindowTo(string window)
        {
            foreach (string defwindow in GetDriver().WindowHandles)
            {
                if (defwindow == window)
                {
                    GetDriver().SwitchTo().Window(defwindow);
                    break;
                }
            }
        }

        public static void SwitchWindowTo(string window, Guid key)
        {
            foreach (string defwindow in GetDriver(key).WindowHandles)
            {
                if (defwindow == window)
                {
                    GetDriver(key).SwitchTo().Window(defwindow);
                    break;
                }
            }
        }

        public void SwitchToBaseWindow()
        {
            GetDriver().SwitchTo().Window(BaseWindow);
        }
        public void TakeScreenShot(string methodName, string pageName, bool beforeExecution)
        {
            string screenshotPath;
            if (beforeExecution)
            {
                screenshotPath = Directory.GetCurrentDirectory() + "\\screenshots\\" + DateTime.Now.Date.ToString().Replace('/', '.').Replace(' ', '_').Replace(':', '_') + "-_before_" + methodName + "_at_" + pageName + ".png";
            }
            else
            {
                screenshotPath = Directory.GetCurrentDirectory() + "\\screenshots\\" + DateTime.Now.Date.ToString().Replace('/', '.').Replace(' ', '_').Replace(':', '_') + "-_after_" + methodName + "_at_" + pageName + ".png";
            }
            ((ITakesScreenshot)GetDriver()).GetScreenshot().SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
        }
        private static void CheckDriver(Guid Key)
        {
            if (Drivers[Key] == null)
            {
                throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method InitBrowser or create class by constructor.");
            }
        }
    }
    public enum WebBrowsers
    {
        InternetExplorer,
        Firefox,
        Chrome
    }
}