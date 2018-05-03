using OpenQA.Selenium;

namespace TP365Framework.Drivers
{
    public interface IDriver
    {
        void InitBrowser(WebBrowsers browser);
        IWebDriver GetDriver { get; set; }
        void GoToUrl(string url);
        void CloseDriver();
        void CloseAllDrivers();
    }
}
