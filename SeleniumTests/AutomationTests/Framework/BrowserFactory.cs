using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Product.Framework
{
	/// <summary>
	///     Class BrowserFactory.
	/// </summary>
	/// <seealso cref="BaseEntity" />
	public class BrowserFactory : BaseEntity
	{
		//public static string DriverPath = RunConfigurator.ResourcesPath;
		public static string DriverPath = "resources/";
		/// <summary>
		///     setup webdriver. chromedriver is a default value
		/// </summary>
		/// <returns>driver</returns>
		public static IWebDriver SetupBrowser()
		{
			var browserName = RunConfigurator.GetValue("browser");
			if (browserName == "chrome")
			{
				var options = new ChromeOptions();
				options.AddUserProfilePreference("safebrowsing.enabled", true);
				options.AddUserProfilePreference("download.default_directory", Path.GetFullPath("C:\\SeleniumTools\\download"));
				return new ChromeDriver(Path.GetFullPath(DriverPath), options);
			}
			if (browserName == "firefox")
			{
				var profile = new FirefoxProfile();
				profile.SetPreference("browser.helperApps.neverAsk.saveToDisk",
					"application/octet-stream;application/csv;text/csv;application/vnd.ms-excel;");
				return new FirefoxDriver(profile);
			}
			return new ChromeDriver(Path.GetFullPath(DriverPath));
		}

		public static IWebDriver SetupBrowser(string path)
		{
			var browserName = RunConfigurator.GetValue("browser");
			if (browserName == "chrome")
			{
				var options = new ChromeOptions();
				options.AddUserProfilePreference("safebrowsing.enabled", true);
				options.AddUserProfilePreference("download.default_directory", Path.GetFullPath(path));
				return new ChromeDriver(Path.GetFullPath(DriverPath), options);
			}
			if (browserName == "firefox")
			{
				var profile = new FirefoxProfile();
				profile.SetPreference("browser.helperApps.neverAsk.saveToDisk",
					"application/octet-stream;application/csv;text/csv;application/vnd.ms-excel;");
				return new FirefoxDriver(profile);
			}
			return new ChromeDriver(Path.GetFullPath(DriverPath));
		}
	}
}