using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T365Framework;

namespace TestFramework.Driver
{
    public static class SwitchDriver
    {
        static readonly string BaseWindow=Browser.GetDriver().CurrentWindowHandle;                
        public static void ToWindow(string window)
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
        public static void ToBaseWindow()
        {
            Browser.GetDriver().SwitchTo().Window(BaseWindow);
        }
    }
}
