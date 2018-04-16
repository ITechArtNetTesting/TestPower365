using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class CoexSettingsForm : BaseForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@id, 'breadcrumbsContainer')]//strong[contains(text(), 'Configure')]");

		public CoexSettingsForm(Guid driverId) : base(TitleLocator, "COEX settings form",driverId)
		{
            this.driverId = driverId;
            directorySyncAccordianButton = new Button(By.XPath("//span[contains(text(), 'Directory Sync Status')]"), "Directory sync accordian",driverId);
            expandedDirectorySyncAccordianButton = new Button(By.XPath("//span[contains(text(), 'Directory Sync Status')]/../../../..//div[contains(@class, 'collapse in')]"), "Expanded directory sync accordian",driverId);
            addressBooksSyncAccordianButton = new Button(By.XPath("//span[contains(text(), 'Address Book Sync')]"), "Address books sync accordian",driverId);
            expandedBooksSyncAccordianButton = new Button(By.XPath("//span[contains(text(), 'Address Book Sync')]/../../../..//div[contains(@class, 'collapse in')]"), "Expanded address books accordian",driverId);
            freeBusyAccordianButton = new Button(By.XPath("//span[contains(text(), 'Free\\Busy')]"), "Free\\Busy accordian",driverId);
            expandedFreeBusyAccordianButton = new Button(By.XPath("//span[contains(text(), 'Free\\Busy')]/../../../..//div[contains(@class, 'collapse in')]"), "Expanded Free\\Busy accordian",driverId);
            emailRewrittingAccordianButton = new Button(By.XPath("//span[contains(text(), 'E-mail Rewriting')]"), "E-mail Rewriting accordian",driverId);
            expandedEmailRewrittingAccordianButton = new Button(By.XPath("//span[contains(text(), 'E-mail Rewriting')]/../../../..//div[contains(@class, 'collapse in')]"), "Expanded E-mail Rewriting accordian",driverId);
            finishAccordianButton = new Button(By.XPath("//span[contains(text(), 'FINISH')]"), "FINISH accordian",driverId);
            expandedFinishAccordianButton = new Button(By.XPath("//span[contains(text(), 'FINISH')]/../../../..//div[contains(@class, 'collapse in')]"), "Expanded FINISH Rewriting accordian",driverId);
        }

		private readonly Button directorySyncAccordianButton ;
		private readonly Button expandedDirectorySyncAccordianButton ;
		private readonly Button addressBooksSyncAccordianButton ;
		private readonly Button expandedBooksSyncAccordianButton ;
		private readonly Button freeBusyAccordianButton ;
		private readonly Button expandedFreeBusyAccordianButton ;
		private readonly Button emailRewrittingAccordianButton ;
		private readonly Button expandedEmailRewrittingAccordianButton ;
		private readonly Button finishAccordianButton ;
		private readonly Button expandedFinishAccordianButton ;
		public void ExpandDirectorySyncAccordian()
		{
			Log.Info("Expanding directory sync accordian");
			directorySyncAccordianButton.ScrollTillVisible();
			directorySyncAccordianButton.Click();
			try
			{
				expandedDirectorySyncAccordianButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Accordian button is not ready");
				directorySyncAccordianButton.Click();
			}
		}

		public void ExpandAddressBooksAccordian()
		{
			Log.Info("Expanding address books accordian");
			addressBooksSyncAccordianButton.ScrollTillVisible();
			addressBooksSyncAccordianButton.Click();
			try
			{
				expandedBooksSyncAccordianButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Accordian is not ready");
				addressBooksSyncAccordianButton.Click();
			}
		}

		public void ExpandFreeBusyAccordian()
		{
			Log.Info("Expanding Free\\Busy accordian");
			freeBusyAccordianButton.ScrollTillVisible();
			freeBusyAccordianButton.Click();
			try
			{
				expandedFreeBusyAccordianButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Accordian is not ready");
				freeBusyAccordianButton.Click();
			}
		}

		public void ExpandEmailRewrittingAccordian()
		{
			Log.Info("Expanding e-mail rewritting accordian");
			emailRewrittingAccordianButton.ScrollTillVisible();
			emailRewrittingAccordianButton.Click();
			try
			{
				expandedEmailRewrittingAccordianButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Accordian is not ready");
				emailRewrittingAccordianButton.Click();
			}
		}

		public void ExpandFinishAccordian()
		{
			Log.Info("Expanding Finish accordian");
			finishAccordianButton.ScrollTillVisible();
			finishAccordianButton.Click();
			try
			{
				expandedFinishAccordianButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Accordian is not ready");
				finishAccordianButton.Click();
			}
		}
	}
}
