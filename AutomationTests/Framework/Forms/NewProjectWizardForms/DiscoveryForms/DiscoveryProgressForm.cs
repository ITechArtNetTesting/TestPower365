using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class DiscoveryProgressForm
	{
		private Label discoveryIsInProgressLabel = new Label(By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Discovery is in progress')]"), "Discovery is in Progress label");

		public void WaitForDiscoveryIsCompleted()
		{
			discoveryIsInProgressLabel.WaitForElementPresent();
			int counter = 0;
			while (discoveryIsInProgressLabel.IsPresent() && counter<60)
			{
				Thread.Sleep(30000);
				discoveryIsInProgressLabel = new Label(By.XPath("//span[contains(text(), 'Discovery is in progress')]"), "Discovery is in Progress label");
				counter++;
			}
		}
	}
}
