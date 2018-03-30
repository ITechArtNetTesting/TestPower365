using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using T365.Framework;

namespace Product.Framework.Elements
{
    public class Element : BaseElement
    {
        public Element(By locator, string name) : base(locator, name)
        {
        }
        public void Move()
        {
            var move = new Actions(Browser.GetDriver());
            move.MoveToElement(GetElement()).Build().Perform();
            Log.Info($"Mouse pointer hover over '{GetName()}'");
        }
    }
}
