using System;
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

		public GroupsMigrationForm() : base(TitleLocator, "Groups migration form")
		{
		}
		private readonly Button actionsDropdownButton =
			new Button(
				By.XPath(
					"//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"),
				"Actions dropdown");
		public new void SyncUserByLocator(string locator)
		{
			ScrollToTop();
			Log.Info("Syncing group by locator: " + locator);
			Thread.Sleep(2000);
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
