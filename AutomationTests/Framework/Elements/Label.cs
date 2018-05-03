using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Product.Framework.Elements
{
	public class Label : BaseElement
	{
		public Label(By locator, string name) : base(locator, name)
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