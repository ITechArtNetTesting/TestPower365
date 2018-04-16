﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;
using Product.Framework.Forms.PublicFolderMigrationForms;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class DirectorySyncSettingsForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'You will need the information below')]");

		public DirectorySyncSettingsForm(Guid driverId) : base(TitleLocator, "Directory Sync settings form",driverId)
		{
            this.driverId = driverId;
            accessUrlLabel = new Label(By.XPath("//h4[contains(@data-bind, 'accessUrl')]"), "Public URL label",driverId);
            accessKeyLabel = new Label(By.XPath("//h4[contains(@data-bind, 'accessKey')]"), "Public key label",driverId);
        }
		private readonly Label accessUrlLabel ;
		private readonly Label accessKeyLabel ;

		public void StoreAccessUrl()
		{
			Log.Info("Storing access url");
			Store.AccessUrl = accessUrlLabel.GetText();
		}

		public void StoreAccessKey()
		{
			Log.Info("Storing access key");
			Store.AccessKey = accessKeyLabel.GetText();
		}
	}
}
