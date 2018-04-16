using OpenQA.Selenium;
using System;

namespace Product.Framework.Elements
{
	public class Link : BaseElement
	{
		public Link(By locator, string name,Guid driverId) : base(locator, name,driverId)
		{
		}
	}
}