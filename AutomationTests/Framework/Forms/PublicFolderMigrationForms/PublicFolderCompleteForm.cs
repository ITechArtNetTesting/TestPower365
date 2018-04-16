using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderCompleteForm : BasePublicFolderWizardForm
	{        

        private static readonly By Titlelocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Ok, so you want to migrate')]");

		public PublicFolderCompleteForm(Guid driverId) : base(Titlelocator, "Complete form",driverId)
		{
            this.driverId = driverId;
		}


	}
}
