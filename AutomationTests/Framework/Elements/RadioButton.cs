using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Product.Framework.Elements
{
	public class RadioButton : BaseElement
	{
		public RadioButton(By locator, string name,Guid driverId) : base(locator, name,driverId)
		{
		}

		public bool IsSelected()
		{
			WaitForElementPresent();
			return GetElement().Selected;
		}

		public void WaitForSelected(int count)
		{
			var wait = new WebDriverWait(Driver.GetDriver(driverId),
				TimeSpan.FromMilliseconds(Convert.ToDouble(count)));
			try
			{
				wait.Until(waiting => IsSelected());
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{GetLocator()}' is not seleced!");
			}
		}

		public void WaitForUnselected(int count)
		{
			var wait = new WebDriverWait(Driver.GetDriver(driverId),
			TimeSpan.FromMilliseconds(Convert.ToDouble(count)));
			try
			{
				wait.Until(waiting => !IsSelected());
			}
			catch (TimeoutException)
			{
				Log.Fatal($"Element with locator: '{GetLocator()}' is not seleced!");
			}
		}
	}
}
