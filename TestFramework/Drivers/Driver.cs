using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T365.Framework;

namespace TestFramework.Drivers
{
    public static class Driver
    {
        static readonly string BaseWindow=Browser.GetDriver().CurrentWindowHandle;                
        public static void SwitchWindowTo(string window)
        {
            foreach (string defwindow in Browser.GetDriver().WindowHandles)
            {
                if (defwindow == window)
                {
                    Browser.GetDriver().SwitchTo().Window(defwindow);
                    break;
                }
            }
        }
        public static void SwitchToBaseWindow()
        {
            Browser.GetDriver().SwitchTo().Window(BaseWindow);
        }
        public static void TakeScreenShot(string methodName, string pageName)
        {

        }
        public static void TakeScreenShot(string methodName, string pageName, bool beforeExecution)
        {
            string screenshotPath;
            if (beforeExecution)
            {
                screenshotPath = Directory.GetCurrentDirectory() + "\\screenshots\\" + DateTime.Now.Date.ToString().Replace('/', '.').Replace(' ', '_').Replace(':','_') + "-_before_" + methodName + "_at_" + pageName + ".png";
            }
            else
            {
                screenshotPath = Directory.GetCurrentDirectory() + "\\screenshots\\" + DateTime.Now.Date.ToString().Replace('/', '.').Replace(' ', '_').Replace(':','_') + "-_after_" + methodName + "_at_" + pageName+".png";
            }
            ((ITakesScreenshot)Browser.GetDriver()).GetScreenshot().SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
        }
    }
}
