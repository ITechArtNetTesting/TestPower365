using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    /// <summary>
    /// Automatically switches context back to previous window when disposed
    /// </summary>
    /// <typeparam name="T">Page object</typeparam>
    public class DisposablePopupPage<T> : IDisposable
        where T: PageBase
    {
        public T Page { get; private set; }

        private readonly string _previousWindowHandle;
        private readonly IWebDriver _webDriver;

        public DisposablePopupPage(string previousWindowHandle, IWebDriver webDriver)
        {
            _previousWindowHandle = previousWindowHandle;
            _webDriver = webDriver;

            Page = (T)Activator.CreateInstance(typeof(T), _webDriver);
        }

        public void Dispose()
        {
            _webDriver.SwitchTo().Window(_previousWindowHandle);
        }
    }
}
