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

        public PublicFolderListForm(Guid driverId) : base(TitleLocator, "PF list form",driverId)
        {
            this.driverId = driverId;
            yesRadioButton = new RadioButton(By.Id("csvFile"), "Yes radiobutton",driverId);
            noRadioButton = new RadioButton(By.Id("manual"), "No radiobutton",driverId);
            yesButton = new Button(By.XPath("//label[contains(@for, 'csvFile')]"), "Yes button",driverId);
            noButton = new Button(By.XPath("//label[contains(@for, 'manual')]"), "No button",driverId);
        }

        private readonly RadioButton yesRadioButton ;
        private readonly RadioButton noRadioButton ;

        private readonly Button yesButton ;
        private readonly Button noButton ;
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
