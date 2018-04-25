using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Product.Framework.Elements;
using System.Collections.Generic;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
    public class HowToMatchUsersForm : BaseWizardStepForm
    {
        string[] ValidAttributes = { "UserPrincipalName", "Mail", "ExtensionAttribute" };
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How would you like to match')]");

        IList<IWebElement> DropDownsAttributes = Browser.GetDriver().FindElements(By.XPath("//div[@class='dropdown inline m-t-sm']//li//a"));

        IList<IWebElement> DropDownButtons = Browser.GetDriver().FindElements(By.XPath("//div[@class='dropdown inline m-t-sm']//button"));


        public HowToMatchUsersForm() : base(TitleLocator, "How to match users form")
        {
        }

        public HowToMatchUsersForm(By _TitleLocator, string name) : base(_TitleLocator, name)
        {
        }

        bool InvalidAtrsExist()
        {
            bool result = false;
            for (int i = 1; i <= DropDownButtons.Count; i++)
            {
                DropDownButtons[i - 1].Click();
                for (int z = ((DropDownsAttributes.Count / 2 * i) - ((DropDownsAttributes.Count / 2) - 1)) - 1; z < (DropDownsAttributes.Count / 2 * i); z++)
                {
                    bool AtributeIsValide = false;
                    foreach (var validatr in ValidAttributes)
                    {
                        if (DropDownsAttributes[z].Text.Contains(validatr))
                        {
                            AtributeIsValide = true;
                        }
                    }
                    if (!AtributeIsValide)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public void CheckAttributes()
        {
            Assert.IsFalse(InvalidAtrsExist());
        }
    }
}