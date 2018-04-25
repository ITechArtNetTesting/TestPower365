using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
    public class PublicFolderListForm : BasePublicFolderWizardForm
    {
        private static readonly By TitleLocator = By.XPath(
                "//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Do you already have a list of public folders that you would like to sync')]");

        public PublicFolderListForm() : base(TitleLocator, "PF list form")
        {
        }

        private readonly RadioButton yesRadioButton = new RadioButton(By.Id("csvFile"), "Yes radiobutton");
        private readonly RadioButton noRadioButton = new RadioButton(By.Id("manual"), "No radiobutton");

        private readonly Button yesButton = new Button(By.XPath("//label[contains(@for, 'csvFile')]"), "Yes button");
        private readonly Button noButton = new Button(By.XPath("//label[contains(@for, 'manual')]"), "No button");
        public void SelectNo()
        {
            Log.Info("Selecting keep existing users");
            noButton.Click();
            try
            {
                noRadioButton.WaitForSelected(5000);
            }
            catch (Exception)
            {
                Log.Info("Radiobutton is not ready");
                noRadioButton.Click();
            }
        }
    }
}
