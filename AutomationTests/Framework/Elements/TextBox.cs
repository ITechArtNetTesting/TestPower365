using System;
using OpenQA.Selenium;

namespace Product.Framework.Elements
{
	public class TextBox : BaseElement
	{
		public TextBox(By locator, string name) : base(locator, name)
		{
		}


		/// <summary>
		///     Sets the text.
		/// </summary>
		/// <param name="text">The text.</param>
		public void SetText(string text)
		{
			WaitForElementPresent();
			GetElement().Click();
			GetElement().SendKeys(text);
			Log.Info($"{GetName()} :: type text '{text}'");
		}


		/// <summary>
		///     Clears field and sets text.
		/// </summary>
		/// <param name="text">The text.</param>
		public void ClearSetText(string text)
		{
			WaitForElementPresent();
			WaitForElementIsVisible();
			bool ready = false;
			int counter = 0;
			while (!ready && counter < 20)
			{
				try
				{
					GetElement().Click();
					ready = true;
					Log.Info($"{GetName()} :: click");
				}
				catch (Exception e)
				{
					Log.Info("Element is not ready: " + GetName());
					counter++;
					if (counter == 20)
					{
						throw e;
					}
				}
			}
			GetElement().Clear();
			GetElement().SendKeys(text);
			Log.Info($"{GetName()} :: type text '{text}'");
		}

		/// <summary>
		///     Presses the enter button.
		/// </summary>
		public void PressEnter()
		{
			WaitForElementPresent();
			WaitForElementIsVisible();
			GetElement().SendKeys(Keys.Enter);
			Log.Info($"{GetName()} :: Press Enter'");
		}
	}
}