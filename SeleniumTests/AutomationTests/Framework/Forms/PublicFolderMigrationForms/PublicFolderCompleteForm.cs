using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderCompleteForm : BasePublicFolderWizardForm
	{
		private static readonly By Titlelocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Ok, so you want to migrate')]");

        private readonly Button addAnotherPublicFolderMigration = new Button(By.XPath("//span[@data-translation='AddAnotherPublicFolderMigration']"), "Add another public folder migration button");

        public PublicFolderCompleteForm() : base(Titlelocator, "Complete form")
		{
		}

        public void ClickAddAnotherPublicFolderMigration()
        {
            //addAnotherPublicFolderMigration.WaitForElementIsClickable();
            addAnotherPublicFolderMigration.Click();
        }
	}
}
