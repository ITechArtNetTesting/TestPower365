using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Product.Framework.Elements
{
	public class ComboBox : BaseElement
	{
		private SelectElement @select;

		public ComboBox(By locator, string name)
			: base(locator, name)
		{
		}


		/// <summary>
		///     Selects the by label.
		/// </summary>
		/// <param name="label">The label.</param>
		public void SelectByLabel(string label)
		{
			WaitForElementPresent();
			Log.Info($"selecting option by text '{label}'");
			@select = new SelectElement(GetElement());
			@select.SelectByText(label);
		}

		/// <summary>
		///     Selects the by value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void SelectByValue(string value)
		{
			WaitForElementPresent();
			Log.Info($"selecting option by value '{value}'");
			@select = new SelectElement(GetElement());
			@select.SelectByValue(value);
		}


		/// <summary>
		///     Selects the index of the by.
		/// </summary>
		/// <param name="index">The index.</param>
		public void SelectByIndex(int index)
		{
			WaitForElementPresent();
			Log.Info($"selecting option by index '{index}'");
			@select = new SelectElement(GetElement());
			@select.SelectByIndex(index);
		}
	}
}