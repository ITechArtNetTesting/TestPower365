using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using UI = OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public abstract class ElementBase
    {
        internal By Locator { get { return _locator; } }
        protected IWebDriver WebDriver { get; private set; }
        protected UI.PopupWindowFinder PopupFinder { get { return _popupWindowFinder ?? (_popupWindowFinder = new UI.PopupWindowFinder(WebDriver)); } }

        protected readonly string LowerCaseTextLocatorFormat = "//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]]";

        private readonly By _locator;
        private UI.PopupWindowFinder _popupWindowFinder;

        protected ElementBase(By locator, IWebDriver webDriver)
        {
            WebDriver = webDriver;
            _locator = locator;
            Validate();
        }

        public void Validate()
        {
            FindVisibleElement(_locator);
        }

        protected DisposablePopupPage<T> ClickPopupElementBy<T>(By by, int timeoutInSec = 5, int pollIntervalSec = 0)
            where T : PageBase
        {
            var element = FindClickableElement(by, timeoutInSec, pollIntervalSec);
            var currentWindowHandle = WebDriver.CurrentWindowHandle;
            var popupHandle = PopupFinder.Click(element);
            WebDriver.SwitchTo().Window(popupHandle);

            return new DisposablePopupPage<T>(currentWindowHandle, WebDriver);
        }
        
        protected void ClickElementBy(By by, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            var clickableElement = FindClickableElement(by, timeoutInSec, pollIntervalSec);
            clickableElement.Click();
        }

        protected T ClickElementBy<T>(By by, int timeoutInSec = 5, int pollIntervalSec = 0)
            where T : PageBase
        {
            ClickElementBy(by, timeoutInSec, pollIntervalSec);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }
        
        protected IWebElement FindExistingElement(By by, int timeoutInSeconds = 5, int pollIntervalSec = 0)
        {
            return EvaluateElement(by, ExpectedConditions.ElementExists(by), timeoutInSeconds, pollIntervalSec);
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
            return EvaluateElement(by, condition, () => WebDriver.Navigate().Refresh(), timeoutInSec, pollIntervalSec);
        }

        protected T EvaluateElement<T>(By by, Func<IWebDriver, T> condition, Action refreshAction, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            if (timeoutInSec > 0 || pollIntervalSec > 0)
            {
                var wait = new UI.WebDriverWait(WebDriver, TimeSpan.FromSeconds(timeoutInSec));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                if (pollIntervalSec > 0)
                {
                    wait.PollingInterval = TimeSpan.FromSeconds(pollIntervalSec);
                    return wait.Until((webDriver) =>
                    {
                        refreshAction();

                        if (!IsDocumentReady())
                            throw new Exception("Page failed to reach ready state in time.");
                        if (!IsAjaxActive())
                            throw new Exception("AJAX failed to completed in time.");

                        return condition(webDriver);
                    });
                }

                if (!IsDocumentReady())
                    throw new Exception("Page failed to reach ready state in time.");

                return wait.Until(condition);
            }

            if (!IsDocumentReady())
                throw new Exception("Page failed to reach ready state in time.");

            return condition(WebDriver);
        }

        protected T EvaluateScript<T>(string script, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            if (timeoutInSec > 0 || pollIntervalSec > 0)
            {
                var wait = new UI.WebDriverWait(WebDriver, TimeSpan.FromSeconds(timeoutInSec));

                if (pollIntervalSec > 0)
                    wait.PollingInterval = TimeSpan.FromSeconds(pollIntervalSec);

                return wait.Until((webDriver) =>
                {
                    return (T)(WebDriver as IJavaScriptExecutor).ExecuteScript(script);
                });
            }
            return (T)(WebDriver as IJavaScriptExecutor).ExecuteScript(script);
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

        private void WaitForLoadComplete()
        {
            if (!IsDocumentReady())
                throw new Exception("Page failed to reach ready state in time.");
            if (!IsAjaxActive())
                throw new Exception("AJAX failed to completed in time.");
        }
    }
}
