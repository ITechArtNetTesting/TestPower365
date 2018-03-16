using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Product.Framework.Elements
{
	public abstract class BaseElement : BaseEntity
	{
		private readonly RemoteWebElement element;
		private readonly By locator;
		private readonly string name;

		protected BaseElement(By locator, string name)
		{
			this.name = name;
			this.locator = locator;
		}

		/// <summary>
		///     Gets the text.
		/// </summary>
		/// <returns>System.String.</returns>
		public string GetText()
		{
			WaitForElementPresent();
			return Browser.GetDriver().FindElement(locator).Text;
		}

		/// <summary>
		///     Gets the element.
		/// </summary>
		/// <returns>RemoteWebElement.</returns>
		public RemoteWebElement GetElement()
		{
			WaitForElementPresent();
			return (RemoteWebElement)Browser.GetDriver().FindElement(locator);
		}

		protected string GetName()
		{
			return name;
		}

		protected By GetLocator()
		{
			return locator;
		}


        /// <summary>
        ///     Clicks this instance.
        /// </summary>
        //public void Click()
        //{
        //	WaitForElementPresent();
        //	WaitForElementIsVisible();
        //	bool ready = false;
        //	int counter = 0;
        //	while (!ready && counter<20)
        //	{
        //		try
        //		{
        //			GetElement().Click();
        //			ready = true;
        //			Log.Info(String.Format("{0} :: click", GetName()));
        //		}
        //		catch (Exception e)
        //		{
        //			Log.Info("Element is not ready: "+GetName());
        //			counter++;
        //			if (counter==20)
        //			{
        //				throw e;
        //			}
        //		}
        //	}
        //}

        public void Click()
        {
            WaitForElementPresent();
            WaitForElementIsVisible();
            WaitForElementIsClickable();
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
            wait.Until(waiting =>
            {
                try
                {
                    GetElement().Click();
                    Log.Info($"{GetName()} :: click");
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public void DoubleClick()
		{
			Actions action = new Actions(Browser.GetDriver());
			action.DoubleClick(GetElement()).Build().Perform();
			Log.Info("Double clicking element: "+GetName());
		}


		/// <summary>
		///     Gets the attribyte value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public string GetAttribyteValue(string value)
		{
			return GetElement().GetAttribute(value);
		}


		/// <summary>
		///     Determines whether this instance is present.
		/// </summary>
		/// <returns>Boolean.</returns>
		public bool IsPresent()
		{
			return Browser.GetDriver().FindElements(locator).Count > 0;
		}

		public bool IsPresent(int count)
		{
			return Browser.GetDriver().FindElements(locator).Count == count;
		}

		/// <summary>
		///     Determines whether this instance is displayed.
		/// </summary>
		/// <returns>Boolean</returns>
		public bool IsDisplayed()
		{
			return GetElement().Displayed;
		}

		/// <summary>
		///     Gets the point.
		/// </summary>
		/// <returns>Point</returns>
		public Point GetPoint()
		{
			return GetElement().Location;
		}

		/// <summary>
		///     Waits for element present.
		/// </summary>
		public void WaitForElementPresent()
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
			try
			{
				wait.Until(waiting =>
				{
					var webElements = Browser.GetDriver().FindElements(locator);
					return webElements.Count != 0;
				});
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{locator}' does not exists!");
			}
		}
		public void WaitForSeveralElementsPresent(int count)
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
			try
			{
				wait.Until(waiting =>
				{
					var webElements = Browser.GetDriver().FindElements(locator);
					return webElements.Count == count;
				});
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{locator}' does not exists!");
			}
		}
		public void WaitForSeveralElementsPresent(int count, int timeout)
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(timeout)));
			try
			{
				wait.Until(waiting =>
				{
					var webElements = Browser.GetDriver().FindElements(locator);
					return webElements.Count == count;
				});
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{locator}' does not exist!");
			}
		}

		public void WaitForElementPresent(int timeout)
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(timeout)));
			try
			{
				wait.Until(waiting =>
				{
					var webElements = Browser.GetDriver().FindElements(locator);
					return webElements.Count != 0;
				});
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{locator}' does not exists!");
			}
		}
		/// <summary>
		///     Waits for element present.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <param name="name">The name.</param>
		public static void WaitForElementPresent(By locator, string name)
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
			try
			{
				wait.Until(waiting =>
				{
					try
					{
						var webElements = Browser.GetDriver().FindElements(locator);
						return webElements.Count != 0;
					}
					catch (Exception)
					{
						return false;
					}
				});
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{locator}' does not exists!");
			}
		}

		public static void WaitForDOM()
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
			try
			{
				wait.Until(waiting =>
				{
					try
					{
						return
							((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("return document.readyState")
								.Equals("complete");
					}
					catch (Exception)
					{
						return false;
					}
				});
			}
			catch (TimeoutException)
			{
			}
		}

		/// <summary>
		///     Waits for element is visible.
		/// </summary>
		public void WaitForElementIsVisible()
		{
			var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(30));
			wait.Until(ExpectedConditions.ElementIsVisible(locator));
		}

		/// <summary>
		///     Waits for element is clickable.
		/// </summary>
		public void WaitForElementIsClickable()
		{
			var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(10));
			wait.Until(ExpectedConditions.ElementToBeClickable(locator));
		}

		/// <summary>
		///     Waits for element is clickable.
		/// </summary>
		/// <param name="locator">The locator.</param>
		public static void WaitForElementIsClickable(By locator)
		{
			var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(10));
			wait.Until(ExpectedConditions.ElementToBeClickable(locator));
		}

		/// <summary>
		///     Waits for element disappear.
		/// </summary>
		public void WaitForElementDisappear()
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
			try
			{
				wait.Until(waiting =>
				{
					var webElements = Browser.GetDriver().FindElements(locator);
					return webElements.Count == 0;
				});
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{locator}' still exists!");
			}
		}
		public void WaitForElementDisappear(int timeout)
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(timeout)));
			try
			{
				wait.Until(waiting =>
				{
					var webElements = Browser.GetDriver().FindElements(locator);
					return webElements.Count == 0;
				});
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{locator}' still exists!");
			}
		}

		/// <summary>
		///     Waits for element disappear.
		/// </summary>
		/// <param name="locator">The locator.</param>
		public static void WaitForElementDisappear(By locator)
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
			try
			{
				wait.Until(waiting =>
				{
					var webElements = Browser.GetDriver().FindElements(locator);
					return webElements.Count == 0;
				});
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{locator}' still exists!");
			}
		}

		public void ScrollTillVisible()
		{
			((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView();", GetElement());
		}
	}
}