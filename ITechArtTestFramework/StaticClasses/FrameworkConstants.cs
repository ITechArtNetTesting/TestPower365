using OpenQA.Selenium;
using System;
using System.Reflection;
using TP365Framework.PageObjects;
using TP365Framework.Steps;

namespace TP365Framework.StaticClasses
{
    public static class FrameworkConstants
    {        			        
        public static readonly Type WebElementType = typeof(IWebElement);
        public static readonly Type PageObjectType = typeof(PageObject);
        public static readonly Type BaseStepsType = typeof(BaseSteps);        
        public static readonly BindingFlags BindingFlags = BindingFlags.Instance |
                 BindingFlags.NonPublic |
                 BindingFlags.Public;
    }
}
