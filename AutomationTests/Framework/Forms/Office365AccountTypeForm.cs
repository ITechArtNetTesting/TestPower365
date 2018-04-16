using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class Office365AccountTypeForm : BaseForm
	{        

        private static readonly By TitleLocator = By.XPath("//div[contains(text(), 'Work or school account')]");
		private readonly Button workAccountButton ;
		public Office365AccountTypeForm(Guid driverId) : base(TitleLocator, "Office 365 account type form",driverId)
		{
            this.driverId = driverId;
            workAccountButton = new Button(By.XPath("//div[contains(text(), 'Work or school account')]"), "Work account button",driverId);
        }

		public void SelectWorkAccount()
		{
			Log.Info("Selecting work account");
			workAccountButton.Click();
		}
	}
}
