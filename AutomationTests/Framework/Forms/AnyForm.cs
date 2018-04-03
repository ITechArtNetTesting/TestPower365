using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class AnyForm : BaseForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'p365-logo')]");
		
		private readonly Label landingLabel =
			new Label(By.XPath("//p[contains(text(), 'Check out the Power 365 Migration process')]"), "Landing label");

		public AnyForm() : base(TitleLocator, "Any form")
		{
		}

		public bool IsLandingForm()
		{
			return
				Browser.GetDriver()
					.FindElements(By.XPath("//p[contains(text(), 'Check out the Power 365 Migration process')]"))
					.Count > 0;
		}

		
	}
}