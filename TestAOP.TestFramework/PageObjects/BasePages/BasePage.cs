using OpenQA.Selenium.Support.PageObjects;
using T365Framework;

namespace TestFramework.PageObjects.BasePages
{
    public abstract class BasePage
    {
        public BasePage()
        {
            PageFactory.InitElements(Browser.GetDriver(), this);
        }
    }
}
