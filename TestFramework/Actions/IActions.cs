using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Actions
{
    public interface IActions
    {
        void Click(IWebElement element);
        void SendKeys(IWebElement element, string keys);
        void Click(IWebElement element,int delay);
        void SendKeys(IWebElement element, string keys,int delay);
    }
}
