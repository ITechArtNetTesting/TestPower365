﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderConflictsForm : BasePublicFolderWizardForm
	{        

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How do you want to resolve conflicts')]");

		public PublicFolderConflictsForm(Guid driverId) : base(TitleLocator, "Conflicts form",driverId)
		{
            this.driverId = driverId;
            lastUpdatedButton = new Button(By.XPath("//label[contains(@for, 'useLastUpdated')]"), "Last updated button",driverId);
            alwaysOverwritesButton = new Button(By.XPath("//label[contains(@for, 'sourceAlwaysOverwritesTarget')]"), "Always overwrites button",driverId);
        }

		private readonly Button lastUpdatedButton ;
        private readonly Button alwaysOverwritesButton;


		public void LastUpdated()
		{
			Log.Info("Last updated selecting");
			lastUpdatedButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				lastUpdatedButton.Click();
			}
		}

		public void Overwrites()
		{
			Log.Info("Always overwrutes selecting");
			alwaysOverwritesButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				alwaysOverwritesButton.Click();
			}
		}
	}
}
