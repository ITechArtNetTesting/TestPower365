using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Product.Framework.Elements;
using System;
using System.Collections.Generic;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
    public class HowToMatchUsersForm : BaseWizardStepForm
    {
        string[] ValidAttributes = { "UserPrincipalName", "Mail", "ExtensionAttribute" };
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How would you like to match')]");

        ComboBox sourceDropDownsAttributes = new ComboBox(By.XPath("//div[contains(@class,'dropdown inline')]//a[contains(@data-bind,'source')]"), "Source attributes");

        ComboBox targetDropDownsAttributes = new ComboBox(By.XPath("//div[contains(@class,'dropdown inline')]//a[contains(@data-bind,'target')]"), "Target attributes"); 
        

        Button sourceDropDownButton = new Button(By.XPath("//button[child::span[contains(@data-bind,'source')]]"), "Source dropdown button");

        Button targetDropDownButton = new Button(By.XPath("//button[child::span[contains(@data-bind,'target')]]"), "Target dropdown button");

        public HowToMatchUsersForm() : base(TitleLocator, "How to match users form")
        {
        }

        public HowToMatchUsersForm(By _TitleLocator, string name) : base(_TitleLocator, name)
        {
        }       

        bool InvalidAtrsExistInSourceDropDown()
        {
            bool result = false;
            sourceDropDownButton.Click();
            foreach (IWebElement dropDown in sourceDropDownsAttributes.GetOptions())
            {
                if (!ArrayContains(ValidAttributes, dropDown.Text))
                {                    
                    result= true;
                    break;
                }
            }
            return result;
        }

        bool InvalidAtrsExistInTargetDropDown()
        {
            bool result = false;
            targetDropDownButton.Click();
            foreach (IWebElement dropDown in targetDropDownsAttributes.GetOptions())
            {
                if (!ArrayContains(ValidAttributes,dropDown.Text))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        bool ArrayContains(string[] array, string value)
        {
            bool result = false;

            foreach (string ele in array)
            {
                if (value.Contains(ele))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void CheckAttributes()
        {
            Assert.IsFalse(InvalidAtrsExistInTargetDropDown());
            Assert.IsFalse(InvalidAtrsExistInSourceDropDown());
        }
    }
}