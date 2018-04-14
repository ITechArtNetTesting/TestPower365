using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Product.Framework.Elements
{
	public class Button : BaseElement
	{        

		public Button(By locator, string name,Guid driverId) : base(locator, name,driverId)
		{
            this.driverId = driverId;
		}

		/// <summary>
		///     Moves this instance.
		/// </summary>
		public void Move()
		{
            //var move = new Actions(Browser.GetDriver());
            var move = new Actions(Driver.GetDriver(driverId));
            move.MoveToElement(GetElement()).Build().Perform();
			Log.Info($"Mouse pointer hover over '{GetName()}'");
		}
	}
}