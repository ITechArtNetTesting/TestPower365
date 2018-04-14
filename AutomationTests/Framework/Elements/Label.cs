using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Product.Framework.Elements
{
	public class Label : BaseElement
	{        
		public Label(By locator, string name,Guid driverId) : base(locator, name,driverId)
		{            
		}

		public void Move()
		{
            //var move = new Actions(Browser.GetDriver());
            var move = new Actions(Driver.GetDriver(driverId));
            move.MoveToElement(GetElement()).Build().Perform();
			Log.Info($"Mouse pointer hover over '{GetName()}'");
		}            
    }
}