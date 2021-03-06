﻿using System;
using System.Collections.Generic;
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

		public By GetLocator()
		{
			return locator;
		}

        public IList<IWebElement> GetElements()
        {
            return Browser.GetDriver().FindElements(locator);
        }

        /// <summary>
        ///     Clicks this instance.
        /// </summary>
        public void Click()
        {
            WaitForElementIsVisible();
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(Browser.GetDriver());
            fluentWait.Timeout = TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout()));
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.Until(x =>
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

        public bool IsElementVisible()
        {
            try
            {
                var iv = Browser.GetDriver().FindElement(locator).Displayed;
                if (iv == true) { return true; } else { return false; }
            }
            catch (NoSuchElementException) { return false; }
        }        

        /// <summary>
        ///     Gets the point.
        /// </summary>
        /// <returns>Point</returns>
        public Point GetPoint()
		{
			return GetElement().Location;
		}

        public void WaitForElementVisible(int timeoutSec = 5, int pollIntervalSec = 0)
        {
            if (!IsElementVisible(locator, timeoutSec, pollIntervalSec))
                throw new Exception("Could not find visible element.");
        }

		/// <summary>
		///     Waits for element present.
		/// </summary>
		public bool WaitForElementPresent()
		{
            Boolean result = true;
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
                result = false;
			}
            return result;
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

		public void WaitForDOM()
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
        public bool WaitForElementIsVisible()
		{
           
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(50));
            wait.Timeout = TimeSpan.FromMinutes(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                return true;
            }

            catch (TimeoutException)
            {
                Log.Fatal($"Element with locator: '{locator}' does not visible!");
                return false;
            }


        }

        /// <summary>
        ///     Waits for element is clickable.
        /// </summary>
        public void WaitForElementIsClickable()
		{
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(50));
            wait.Timeout = TimeSpan.FromMinutes(1);
            wait.IgnoreExceptionTypes(typeof(NotFoundException));
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
		public bool WaitForElementDisappear()
		{
			var wait = new WebDriverWait(Browser.GetDriver(),
				TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
			try
			{
				wait.Until(waiting =>
				{
					var webElements = Browser.GetDriver().FindElements(locator); /// check: Disappear - isDisplayed()= false ? 
					return webElements.Count == 0;
				});
			}
			catch (TimeoutException)
			{                
                Log.Fatal($"Element with locator: '{locator}' still exists!");
                return false;
            }
            return true;
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
        public void WaitForAjaxLoad()
        {
            if (jQueryExists())
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromMinutes(1));
                wait.PollingInterval = TimeSpan.FromMilliseconds(100);
                wait.Until(wd => (bool)(Browser.GetDriver() as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
            }
        }
        static bool jQueryExists()
        {
            try
            {
                (Browser.GetDriver() as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ScrollTillVisible()
		{
			((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView();", GetElement());
		}
	}
}