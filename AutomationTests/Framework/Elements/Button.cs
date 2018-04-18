using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Product.Framework.Elements
{
	public class Button : BaseElement
	{
		public Button(By locator, string name) : base(locator, name)
		{
		}

		/// <summary>
		///     Moves this instance.
		/// </summary>
		public void Move()
		{
			var move = new Actions(Browser.GetDriver());
			move.MoveToElement(GetElement()).Build().Perform();
			Log.Info($"Mouse pointer hover over '{GetName()}'");
		}
	}
}