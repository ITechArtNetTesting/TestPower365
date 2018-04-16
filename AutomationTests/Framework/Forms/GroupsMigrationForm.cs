﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using OpenQA.Selenium;
using Product.Framework.Elements;
using Product.Framework.Enums;

namespace Product.Framework.Forms
{
	public class GroupsMigrationForm : UsersForm
	{        

        private static readonly By TitleLocator = By.XPath("//a[contains(@data-bind, 'GroupMigrationsDialog')]");

		public GroupsMigrationForm(Guid driverId) : base(TitleLocator, "Groups migration form",driverId)
		{
            this.driverId = driverId;
            actionsDropdownButton =new Button(By.XPath("//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"),"Actions dropdown",driverId);
        }
		private readonly Button actionsDropdownButton;

        public new void SyncUserByLocator(string locator)
		{
            WaitForAjaxLoad();
            ScrollToTop();
			Log.Info("Syncing group by locator: " + locator);
            WaitForAjaxLoad();
			SelectEntryBylocator(locator);
			SelectAction(ActionType.Sync);
			Apply();
		}

		public new void SelectAction(ActionType type)
		{
			OpenActionsDropdown();
			ChooseAction(type.GetValue());
		}

		public new void OpenActionsDropdown()
		{
			Log.Info("Opening Actions dropdown");
			ScrollToElement(actionsDropdownButton.GetElement());
			actionsDropdownButton.Click();
			try
			{
				expandedActionsDropdownButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Actions dropdown is not ready");
				actionsDropdownButton.Click();
			}
		}
	}
}
