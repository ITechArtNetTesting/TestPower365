using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public abstract class WorkflowBase : ElementBase
    {
        protected readonly PageBase RootPage;
        protected PageBase CurrentPage;

        protected WorkflowBase(PageBase page, IWebDriver webDriver)
            : base(page.Locator, webDriver)
        {
            RootPage = CurrentPage = page;
        }

        public T GetPage<T>()
            where T : PageBase
        {
            return GetCurrentPage<T>(false);
        }

        protected T GetCurrentPage<T>(bool throwOnError = true, string castError = null)
            where T : PageBase
        {
            if (CurrentPage == null)
            {
                if (throwOnError)
                    throw new Exception("Current Page has not been set.");
                return null;
            }

            var castedPage = CurrentPage as T;
            if (castedPage == null && throwOnError)
                throw new Exception(castError ?? string.Format("Current Page must be '{0}'.", typeof(T).Name));
            return castedPage;
        }
    }
}
