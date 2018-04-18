using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Elements
{
    public class Element : BaseElement
    {
        public Element(By locator, string name) : base(locator, name)
        {
        }
    }
}
