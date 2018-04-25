using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Product.Framework
{
	/// <summary>
	///     Class Browser.
	/// </summary>
	/// <seealso cref="BaseEntity" />
	public class Browser : BaseEntity
	{
		protected static Browser browser;
		private static IWebDriver driver;
		// Sets up WebDriver Instance.       
		/// <summary>
		///     Gets the instance.
		/// </summary>
		/// <returns>Browser.</returns>
		public static Browser GetInstance()
		{
			driver = BrowserFactory.SetupBrowser();

			return new Browser();
		}

		public static Browser GetInstance(string path)
		{
			driver = BrowserFactory.SetupBrowser(path);

			return new Browser();
		}

		/// <summary>
		///     Gets the driver.
		/// </summary>
		/// <returns>IWebDriver.</returns>
		public static IWebDriver GetDriver()
		{
			return driver;
		}

		/// <summary>
		///     Waits for page to load.
		/// </summary>
		public static void WaitForPageToLoad()
		{
			var wait = new WebDriverWait(GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
			try
			{
				wait.Until(waiting =>
				{
					try
					{
						var result =
							((IJavaScriptExecutor) GetDriver()).ExecuteScript(
								"return document['readyState'] ? 'complete' == document.readyState : true");
						return result is bool && (bool) result;
					}
					catch (Exception)
					{
						return false;
					}
				});
			}
			catch (Exception)
			{
			}
		}
	}
}