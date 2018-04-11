using System;
using log4net;
using log4net.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Product.Framework
{
	/// <summary>
	///     Class BaseEntity.
	/// </summary>
	public class BaseEntity
	{
		protected static ILog Log;

		protected BaseEntity()
		{
			XmlConfigurator.Configure();
			Log = LogManager.GetLogger(typeof(BaseEntity));
		}

		// Just logs current step number and name.
		/// <summary>
		///     Logs the step.
		/// </summary>
		/// <param name="step">The step.</param>
		/// <param name="message">The message.</param>
		public void LogStep(int step, string message)
		{
			Log.Info($"----------[ Step {step} ]: {message}");
		}

		// Logs Test Case name.
		/// <summary>
		///     Logs the case.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogCase(string message)
		{
			Log.Info("              ");
			Log.Info(string.Format(message));
			Log.Info("              ");
		}

	    public void LogHtml(string message)
	    {
	        Console.WriteLine(" ");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine(" ");
	        Console.WriteLine(message);
	        Console.WriteLine(" ");
	        Console.WriteLine("----------------------------------------------------");
        }


        protected void ClickElementBy(By by, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            var clickableElement = FindClickableElement(by, timeoutInSec, pollIntervalSec);
            clickableElement.Click();
        }

        protected IWebElement FindVisibleElement(By by, int timeoutInSeconds = 5, int pollIntervalSec = 0)
        {
            return EvaluateElement(by, ExpectedConditions.ElementIsVisible(by), timeoutInSeconds, pollIntervalSec);
        }

        protected IWebElement FindClickableElement(By by, int timeoutInSeconds = 5, int pollIntervalSec = 0)
        {
            return EvaluateElement(by, ExpectedConditions.ElementToBeClickable(by), timeoutInSeconds, pollIntervalSec);
        }

        protected bool IsElementTextPresentInValue(By by, string value, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            try
            {
                return EvaluateElement(by, ExpectedConditions.TextToBePresentInElementValue(by, value), timeoutInSec, pollIntervalSec);
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsElementExists(By by, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            try
            {
                return EvaluateElement(by, ExpectedConditions.ElementExists(by), timeoutInSec, pollIntervalSec) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsElementVisible(By by, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            try
            {
                return EvaluateElement(by, ExpectedConditions.ElementIsVisible(by), timeoutInSec, pollIntervalSec) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsElementClickable(By by, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            try
            {
                return EvaluateElement(by, ExpectedConditions.ElementToBeClickable(by), timeoutInSec, pollIntervalSec) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsElementSelectedState(By by, bool selected, int timeoutInSeconds = 5, int pollIntervalSec = 0)
        {
            try
            {
                return EvaluateElement(by, ExpectedConditions.ElementSelectionStateToBe(by, selected), timeoutInSeconds, pollIntervalSec);
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected T EvaluateElement<T>(By by, Func<IWebDriver, T> condition, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            if (timeoutInSec > 0 || pollIntervalSec > 0)
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(timeoutInSec));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                if (pollIntervalSec > 0)
                {
                    wait.PollingInterval = TimeSpan.FromSeconds(pollIntervalSec);
                    return wait.Until((webDriver) =>
                    {
                        webDriver.Navigate().Refresh();

                        if (!IsDocumentReady())
                            throw new Exception("Page failed to reach ready state in time.");
                        if (!IsAjaxActive())
                            throw new Exception("AJAX failed to completed in time.");

                        return condition(webDriver);
                    });
                }
                return wait.Until(condition);
            }
            return condition(Browser.GetDriver());
        }

        protected T EvaluateScript<T>(string script, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            if (timeoutInSec > 0 || pollIntervalSec > 0)
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(timeoutInSec));

                if (pollIntervalSec > 0)
                    wait.PollingInterval = TimeSpan.FromSeconds(pollIntervalSec);

                return wait.Until((webDriver) =>
                {
                    return (T)(Browser.GetDriver() as IJavaScriptExecutor).ExecuteScript(script);
                });
            }
            return (T)(Browser.GetDriver() as IJavaScriptExecutor).ExecuteScript(script);
        }

        protected bool IsAjaxActive(int timeoutInSec = 10)
        {
            try
            {
                return EvaluateScript<bool>("return jQuery.active == 0", timeoutInSec);
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsDocumentReady(int timeoutInSec = 10)
        {
            try
            {
                return EvaluateScript<bool>("return document.readyState == 'complete'", timeoutInSec);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}