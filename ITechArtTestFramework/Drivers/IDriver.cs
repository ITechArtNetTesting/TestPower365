using OpenQA.Selenium;

namespace ITechArtTestFramework.Drivers
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
