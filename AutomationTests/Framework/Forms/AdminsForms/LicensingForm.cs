using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;
using Product.Framework.Forms.PublicFolderMigrationForms;

namespace Product.Framework.Forms.AdminsForms
{
	public class LicensingForm : BaseForm
	{        
		private static readonly By TitleLocator = By.XPath("//h4[contains(text(), 'LICENSES')]");

		public LicensingForm(Guid driverId) : base(TitleLocator, "Licensing form",driverId)
		{            
            this.driverId = driverId;
            addLicenseButton = new Button(By.XPath("//a[contains(@href, 'Licensing/Create')]"), "Add license",driverId);
            mailLicenseLabel = new Label(By.XPath("//td[text()='Mail']"), "Mail license label",driverId);
            integrationProLabel = new Label(By.XPath("//td[text()='Integration Pro']"), "Integration Pro label",driverId);
            publicFoldersLabel = new Label(By.XPath("//td[text()='Public Folders']"), "Public folders label",driverId);
            publicFoldersDetailsButton = new Button(By.XPath("//td[text()='Public Folders']/..//a[contains(@data-modal, 'licenseModal')]"), "Public folders details button",driverId);
            publicFoldersHistoryButton = new Button(By.XPath("//td[text()='Public Folders']/..//a[contains(@data-modal, 'History')]"), "Public folders history button",driverId);
            mailDetailsButton = new Button(By.XPath("//td[text()='Mail']/..//a[contains(@data-modal, 'licenseModal')]"), "Mail details button",driverId);
            mailHistoryButton = new Button(By.XPath("//td[text()='Mail']/..//a[contains(@data-modal, 'History')]"), "Mail history button",driverId);
            integrationProDetailsButton = new Button(By.XPath("//td[text()='Integration Pro']/..//a[contains(@data-modal, 'licenseModal')]"), "Integration Pro details button",driverId);
            integratiobProHistoryButton = new Button(By.XPath("//td[text()='Integration Pro']/..//a[contains(@data-modal, 'History')]"), "Integration Pro history button",driverId);
            okButton = new Button(By.XPath("//div[contains(@class, 'modal fade in')]//button[contains(@type, 'submit')]"), "Ok button",driverId);
            licenseTypeDropdownButton = new ComboBox(By.XPath("//div[contains(@class, 'modal fade in')]//select[@id='LicenseTypeId']"), "License type drop down",driverId);
            totalCountTextBox = new TextBox(By.XPath("//div[contains(@c)]//*[@id='TotalCount']"), "Total count textbox",driverId);
            expireTextBox = new TextBox(By.XPath("//div[contains(@class, 'modal fade in')]//*[@id='ExpirationDate']"), "Expire textbox",driverId);
            submitButton = new Button(By.XPath("//div[contains(@class, 'modal fade in')]//button[contains(@type, 'submit')]"), "Submit button",driverId);
            licenseCancelButton = new Button(By.XPath("//div[@id='licenseModal'][contains(@class, 'modal fade in')]//button[contains(@data-dismiss, 'modal')][contains(text(), 'Cancel')]"), "License cancel button",driverId);
            licenseSubTypeComboBox = new ComboBox(By.Id("LicenseSubTypeId"), "License subtype combobox",driverId);
            cancelHistoryButton = new Button(By.XPath("//div[@id='licenseHistoryModal'][contains(@class, 'modal fade in')]//button[contains(text(), 'Cancel')]"), "Cancel history button",driverId);
        }

		private readonly Button addLicenseButton ;
		private readonly Label mailLicenseLabel ;
		private readonly Label integrationProLabel ;
		private readonly Label publicFoldersLabel ;
		private readonly Button publicFoldersDetailsButton ;
		private readonly Button publicFoldersHistoryButton ;
		private readonly Button mailDetailsButton ;
		private readonly Button mailHistoryButton ;
		private readonly Button integrationProDetailsButton ;
		private readonly Button integratiobProHistoryButton ;

		public void OpenPublicFoldersDetails()
		{
			Log.Info("Opening public folders details");
			publicFoldersDetailsButton.Click();
			try
			{
				okButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				publicFoldersDetailsButton.Click();
			}
		}

		public void OpenPublicFoldersHistory()
		{
			Log.Info("Opening public folders history");
			publicFoldersHistoryButton.Click();
			try
			{
				cancelHistoryButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				publicFoldersHistoryButton.Click();
			}
		}

		public void OpenMailHistory()
		{
			Log.Info("Opening mail history");
			mailHistoryButton.Click();
			try
			{
				cancelHistoryButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				mailHistoryButton.Click();
			}
		}

		public void OpenIntegrationProHistory()
		{
			Log.Info("Opening integration pro history");
			integratiobProHistoryButton.Click();
			try
			{
				cancelHistoryButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				integratiobProHistoryButton.Click();
			}
		}

		public void OpenMailDetails()
		{
			Log.Info("Opening mail details");
			mailDetailsButton.Click();
			try
			{
				okButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				mailDetailsButton.Click();
			}
		}

		public void OpenIntegrationProDetails()
		{
			Log.Info("Opening Integration Pro details");
			integrationProDetailsButton.Click();
			try
			{
				okButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				integrationProDetailsButton.Click();
			}
		}

		public void AssertMailLicenseAdded()
		{
			Log.Info("Asserting Mail license is added");
			mailLicenseLabel.WaitForElementPresent();
		}

		public void AssertIntegrationProLicenseAdded()
		{
			Log.Info("Asserting IntegrationPro license is added");
			integrationProLabel.WaitForElementPresent();
		}

		public void AssertPublicFoldersLicenseAdded()
		{
			Log.Info("Asserting public folders license is added");
			publicFoldersLabel.WaitForElementPresent();
		}

		public void AddLicense()
		{
			Log.Info("Adding license");
			addLicenseButton.Click();
		}

		#region[Add license modal]
		private readonly Button okButton ;
		private readonly ComboBox licenseTypeDropdownButton ;
		private readonly TextBox totalCountTextBox ;
		private readonly TextBox expireTextBox ;
		private  readonly Button submitButton ;
		private readonly Button licenseCancelButton ;
		private readonly ComboBox licenseSubTypeComboBox ;

		public void SelectSmall()
		{
			Log.Info("Selecting small");
			licenseSubTypeComboBox.SelectByValue("0");
		}

		public void SelectMedium()
		{
			Log.Info("Selecting medium");
			licenseSubTypeComboBox.SelectByValue("1");
		}

		public void SelectLarge()
		{
			Log.Info("Select large");
			licenseSubTypeComboBox.SelectByValue("2");
		}

		public void SetTotalCount(string count)
		{
			Log.Info("Setting total count");
			totalCountTextBox.ClearSetText(count);
		}

		public void SubmitLicense()
		{
			Log.Info("Submitting license");
			submitButton.Click();
		}

		public void CancelLicense()
		{
			Log.Info("Cancelling");
			licenseCancelButton.Click();
		}

		public void SetExpireDate(string date)
		{
			Log.Info("Setting expire date: "+date);
			expireTextBox.ClearSetText(date);
		}

		public void SelectMail()
		{
			Log.Info("Selecting Mail license type");
			licenseTypeDropdownButton.SelectByValue("0");
		}
		public void SelectIntegration()
		{
			Log.Info("Selecting Integration license type");
			licenseTypeDropdownButton.SelectByValue("1");
		}
		public void SelectIntegrationPro()
		{
			Log.Info("Selecting Integration Pro license type");
			licenseTypeDropdownButton.SelectByValue("2");
		}
		public void SelectPublicFolders()
		{
			Log.Info("Selecting Public Folders license type");
			licenseTypeDropdownButton.SelectByValue("3");
		}


		#endregion

		#region[History modal]
		private readonly Button cancelHistoryButton ;
#endregion
	}
}
