using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using UI = OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using OpenQA.Selenium.Interactions;
using System.Linq;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using System.IO;
using System.Collections.Generic;
using log4net;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public abstract class ElementBase
    {
        internal By Locator { get { return _locator; } }

        protected ILog Logger { get; set; }
        protected IWebDriver WebDriver { get; private set; }
        protected UI.PopupWindowFinder PopupFinder { get { return _popupWindowFinder ?? (_popupWindowFinder = new UI.PopupWindowFinder(WebDriver)); } }

        protected readonly string LowerCaseTextLocatorFormat = "//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]]";

        private readonly By _locator;
        private UI.PopupWindowFinder _popupWindowFinder;

        protected ElementBase(ILog logger, By locator, IWebDriver webDriver)
        {
            Logger = logger;
            WebDriver = webDriver;
            _locator = locator;
            Validate();
        }

        public void Validate()
        {
            FindVisibleElement(_locator);
        }

        protected DisposablePopupPage<T> ClickPopupElementBy<T>(By by, int timeoutInSec = 10, int pollIntervalInSec = 0)
            where T : PageBase
        {
            var element = FindClickableElement(by, timeoutInSec, pollIntervalInSec);
            var currentWindowHandle = WebDriver.CurrentWindowHandle;
            var popupHandle = PopupFinder.Click(element);
            WebDriver.SwitchTo().Window(popupHandle);

            return new DisposablePopupPage<T>(currentWindowHandle, WebDriver);
        }            

        protected T ClickModalElementBy<T>(By by, int timeoutInSec = 10, int pollIntervalInSec = 0)
            where T : ModalDialogBase
        {
            Logger.DebugFormat("ClickModalElementBy<T>: {0}", by.ToString());
            var element = FindClickableElement(by, timeoutInSec, pollIntervalInSec);
            element.Click();
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }
        
        protected void ClickElementBy(By by, int timeoutInSec = 10, int pollIntervalInSec = 0)
        {
            Logger.DebugFormat("ClickElementBy: {0}", by.ToString());
            var clickableElement = FindClickableElement(by, timeoutInSec, pollIntervalInSec);
            clickableElement.Click();
        }

        protected T ClickElementBy<T>(By by, int timeoutInSec = 10, int pollIntervalInSec = 0)
            where T : PageBase
        {
            Logger.DebugFormat("ClickElementBy<T>: {0}", by.ToString());
            ClickElementBy(by, timeoutInSec, pollIntervalInSec);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }

        protected void DoubleClickElementBy(By by, int timeoutInSec = 10, int pollIntervalInSec = 0)
        {
            Logger.DebugFormat("DoubleClickElementBy: {0}", by.ToString());
            var clickableElement = FindClickableElement(by, timeoutInSec, pollIntervalInSec);
            new Actions(WebDriver).DoubleClick(clickableElement).Build().Perform();
        }

        protected T DoubleClickElementBy<T>(By by, int timeoutInSec = 10, int pollIntervalInSec = 0)
             where T : PageBase
        {
            Logger.DebugFormat("DoubleClickElementBy<T>: {0}", by.ToString());
            DoubleClickElementBy(by, timeoutInSec, pollIntervalInSec);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
        }

        protected void HoverElement(By by, int timeoutInSec = 10, int pollIntervalInSec = 0)
        {
            Logger.DebugFormat("HoverElement: {0}", by.ToString());
            var element = FindExistingElement(by, timeoutInSec, pollIntervalInSec);
            new Actions(WebDriver).MoveToElement(element).Build().Perform();
        }

        protected IEnumerable<IWebElement> FindAllElements(By by, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            Logger.DebugFormat("FindAllElements: {0}", by.ToString());
            return EvaluateElement(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by), timeoutInSec, pollIntervalInSec, refreshAction);
        }

        protected IWebElement FindExistingElement(By by, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            Logger.DebugFormat("FindExistingElement: {0}", by.ToString());
            return EvaluateElement(ExpectedConditions.ElementExists(by), timeoutInSec, pollIntervalInSec, refreshAction);
        }
        
        protected IWebElement FindVisibleElement(By by, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            Logger.DebugFormat("FindVisibleElement: {0}", by.ToString());
            return EvaluateElement(ExpectedConditions.ElementIsVisible(by), timeoutInSec, pollIntervalInSec, refreshAction);
        }

        protected IWebElement FindClickableElement(By by, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            Logger.DebugFormat("FindClickableElement: {0}", by.ToString());
            return EvaluateElement(ExpectedConditions.ElementToBeClickable(by), timeoutInSec, pollIntervalInSec, refreshAction);
        }

        protected bool IsElementTextPresentInValue(By by, string value, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            try
            {
                Logger.DebugFormat("IsElementTextPresentInValue: {0}", by.ToString());
                return EvaluateElement(ExpectedConditions.TextToBePresentInElementValue(by, value), timeoutInSec, pollIntervalInSec, refreshAction);
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsElementExists(By by, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            try
            {
                Logger.DebugFormat("IsElementExists: {0}", by.ToString());
                return EvaluateElement(ExpectedConditions.ElementExists(by), timeoutInSec, pollIntervalInSec, refreshAction) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

       
        protected bool IsAnyElementExists(By[] bys, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            try
            {
                return EvaluateElement((driver) => {
                    return bys.Any(b =>
                    {
                        try
                        {
                            Logger.DebugFormat("IsAnyElementExists: {0}", b.ToString());
                            return driver.FindElement(b) != null;
                        }
                        catch (NoSuchElementException)
                        {
                            return false;
                        }
                    });
                },
                timeoutInSec, pollIntervalInSec, refreshAction);
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsElementVisible(By by, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            try
            {
                Logger.DebugFormat("IsElementVisible: {0}", by.ToString());
                return EvaluateElement(ExpectedConditions.ElementIsVisible(by), timeoutInSec, pollIntervalInSec, refreshAction) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsElementClickable(By by, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            try
            {
                Logger.DebugFormat("IsElementClickable: {0}", by.ToString());
                return EvaluateElement(ExpectedConditions.ElementToBeClickable(by), timeoutInSec, pollIntervalInSec, refreshAction) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsElementSelectedState(By by, bool selected, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            try
            {
                Logger.DebugFormat("IsElementSelectedState: {0}", by.ToString());
                return EvaluateElement(ExpectedConditions.ElementSelectionStateToBe(by, selected), timeoutInSec, pollIntervalInSec, refreshAction);
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        protected T EvaluateElement<T>(Func<IWebDriver, T> condition, int timeoutInSec = 10, int pollIntervalInSec = 0, Action refreshAction = null)
        {
            Logger.DebugFormat("EvaluateElement: timeoutInSec = {0}, pollIntervalInSec = {1}, refreshAction = {2}", timeoutInSec, pollIntervalInSec, refreshAction != null ? refreshAction.ToString() : "DEFAULT");
            if (timeoutInSec > 0 || pollIntervalInSec > 0)
            {
                var wait = new UI.WebDriverWait(WebDriver, TimeSpan.FromSeconds(timeoutInSec));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                if (pollIntervalInSec > 0)
                {
                    wait.PollingInterval = TimeSpan.FromSeconds(pollIntervalInSec);
                    bool shouldRefresh = false;

                    return wait.Until((webDriver) =>
                    {
                        if(shouldRefresh)
                        {
                            Logger.Info("Refreshing...");
                            if (refreshAction != null)
                                refreshAction();
                            else
                                WebDriver.Navigate().Refresh();
                        }

                        if (!IsDocumentReady())
                            throw new Exception("Page failed to reach ready state in time.");
                        if (!IsAjaxActive())
                            throw new Exception("AJAX failed to completed in time.");

                        shouldRefresh = true;

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

        protected T EvaluateScript<T>(string script, int timeoutInSec = 10, int pollIntervalInSec = 0)
        {
            if (timeoutInSec > 0 || pollIntervalInSec > 0)
            {
                var wait = new UI.WebDriverWait(WebDriver, TimeSpan.FromSeconds(timeoutInSec));

                if (pollIntervalInSec > 0)
                    wait.PollingInterval = TimeSpan.FromSeconds(pollIntervalInSec);

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

        public void WaitForLoadComplete()
        {
            if (!IsDocumentReady())
                throw new Exception("Page failed to reach ready state in time.");
            if (!IsAjaxActive())
                throw new Exception("AJAX failed to completed in time.");
        }
    }
}
