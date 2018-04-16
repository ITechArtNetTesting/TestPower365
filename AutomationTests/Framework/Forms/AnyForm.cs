using OpenQA.Selenium;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms
{
    public class AnyForm : BaseForm
    {        

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'p365-logo')]");

        private readonly Label landingLabel ;

        public AnyForm(Guid driverId) : base(TitleLocator, "Any form",driverId)
        {
            this.driverId = driverId;
            landingLabel =new Label(By.XPath("//p[contains(text(), 'Check out the Power 365 Migration process')]"), "Landing label",driverId);
        }

        public bool IsLandingForm()
        {
            //return
            //    Browser.GetDriver()
            //        .FindElements(By.XPath("//p[contains(text(), 'Check out the Power 365 Migration process')]"))
            //        .Count > 0;
            return
    Driver.GetDriver(driverId)
        .FindElements(By.XPath("//p[contains(text(), 'Check out the Power 365 Migration process')]"))
        .Count > 0;
        }


    }
}