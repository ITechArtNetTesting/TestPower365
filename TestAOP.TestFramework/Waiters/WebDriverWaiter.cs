﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using T365Framework;

namespace TestFramework.Waiters
{
    public static class WebDriverWaiter
    {
        public static void WaitForElementIsClickable(IWebElement element)
        {
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromMinutes(1));
            wait.Until(ExpectedConditions.ElementToBeClickable(element));            
        }
        public static void WaitForJSLoad()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Browser.GetDriver();
            WebDriverWait wait = new WebDriverWait(Browser.GetDriver(),TimeSpan.FromSeconds(15));
            wait.Until(wd => (string)js.ExecuteScript("return document.readyState") == "complete");
        }
    }
}
