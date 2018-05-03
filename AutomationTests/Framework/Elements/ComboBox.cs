using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Product.Framework.Elements
{
    public class ComboBox : BaseElement
    {
        private SelectElement @select;

        IList<IWebElement> options;

        public ComboBox(By locator, string name)
            : base(locator, name)
        {
            options = Browser.GetDriver().FindElements(locator);
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

        public IList<IWebElement> GetOptions()
        {
            return options;
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