using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public abstract class MutliPageWorkflow : WorkflowBase
    {
        protected readonly PageBase RootPage;
        protected PageBase CurrentPage;

        protected MutliPageWorkflow(ILog logger, PageBase page, IWebDriver webDriver)
            : base(logger, page.Locator, webDriver)
        {
            RootPage = CurrentPage = page;
        }

        public override T GetPage<T>()
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
