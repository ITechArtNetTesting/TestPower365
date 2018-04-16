﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderMigrtationListForm : BasePublicFolderWizardForm
	{        

        private static readonly By TitleLocator = By.XPath("//h2[contains(text(), 'Do you have a list')]");

		public PublicFolderMigrtationListForm(Guid driverId) : base(TitleLocator, "Do you have a list form",driverId)
		{
            this.driverId = driverId;
            createNewButton = new Button(By.XPath("//label[@for='manual']"), "Create new public folder migration button",driverId);
            uploadListButton = new Button(By.XPath("//label[@for='csvFile']"), "Upload list button",driverId);
        }

		private readonly Button createNewButton ;
		private readonly Button uploadListButton ;
		public void CreatePublicMigration()
		{
			Log.Info("Creating public folder migration");
			createNewButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				createNewButton.Click();
			}
		}

		public void UploadList()
		{
			Log.Info("Upload list selecting");
			uploadListButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				uploadListButton.Click();
			}
		}
	}
}
