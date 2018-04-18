using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class RegistrationForm : BaseForm
	{
		private static readonly By TitleLocator = By.XPath("//span[contains(text(), 'NEW USER REGISTRATION')]");

		private readonly TextBox AddressCityTextBox = new TextBox(By.Id("AddressCity"), "Address City");

		private readonly Button AddressCountryButton = new Button(By.Id("countrySelector"), "Address Country");

		private readonly TextBox AddressLine1TextBox = new TextBox(By.Id("AddressLine1"), "Address Line1");

		private readonly TextBox AddressStateTextBox = new TextBox(By.Id("AddressState"), "Address State");

		private readonly TextBox AddressZipTextBox = new TextBox(By.Id("AddressZip"), "Address Zip");

		private readonly Button backButton = new Button(By.XPath("//a[contains(@href, 'history.back')]"), "Back button");

		private readonly TextBox CientNameTextBox = new TextBox(By.Id("ClientName"), "Client Name");

		private readonly TextBox ContactEmailTextBox = new TextBox(By.Id("ContactEmail"), "Email");

		private readonly TextBox ContactFirstNameTextBox = new TextBox(By.Id("ContactFirstName"), "First Name");

		private readonly TextBox ContactLastNameTextBox = new TextBox(By.Id("ContactLastName"), "Last Name");

		private readonly TextBox ContactPhoneTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'phoneNumber')]"),
			"Phone");

		private readonly Button expandedDropdownButton =
			new Button(By.XPath("//div[contains(@class, 'open')]//*[@id='countrySelector']"),
				"Expanded country dropdown");

		private readonly Button nextButton =
			new Button(By.XPath("//button[contains(@class, 'pull-right')][not(@disabled='')]//span[contains(text(), 'NEXT')]"), "Next button");

		private readonly Button registerButton = new Button(By.XPath("//button[contains(@type, 'submit')]"), "Register button");

		private readonly Button submitButton =
			new Button(By.XPath("//button[contains(@class, 'pull-right')][not(@disabled='')]//span[contains(text(), 'REGISTER')]"), "Submit button");

		public RegistrationForm() : base(TitleLocator, "Registration Form")
		{
		}


		public void ClickBack()
		{
			backButton.Click();
		}

		public void Submit()
		{
			Log.Info("Submitting");
			submitButton.Click();
		}

		public void GoNext()
		{
			Log.Info("Going next");
			nextButton.Click();
		}

		public void ClickRegister()
		{
			registerButton.Click();
		}

		public void SetClientName(string name)
		{
			Log.Info($"Setting {name} Client name");
			CientNameTextBox.ClearSetText(name);
			Store.ClientName = name;
		}

		public void SetFirstName(string name)
		{
			Log.Info($"Setting {name} First name");
			ContactFirstNameTextBox.ClearSetText(name);
			Store.FirstName = name;
		}

		public void SetLastName(string name)
		{
			Log.Info($"Setting {name} Last name");
			ContactLastNameTextBox.ClearSetText(name);
			Store.LastName = name;
		}

		public void SetPhone(string name)
		{
			Log.Info($"Setting {name} Phone");
			ContactPhoneTextBox.ClearSetText(name);
			Store.Phone = name;
		}

		public void SetEmail(string name)
		{
			Log.Info($"Setting {name} Email");
			ContactEmailTextBox.ClearSetText(name);
			Store.Email = name;
		}

		public void SetAddress(string name)
		{
			Log.Info($"Setting {name} Address");
			AddressLine1TextBox.ClearSetText(name);
			Store.Address = name;
		}

		public void SetCity(string name)
		{
			Log.Info($"Setting {name} City");
			AddressCityTextBox.ClearSetText(name);
			Store.City = name;
		}

		public void SetState(string name)
		{
			Log.Info($"Setting {name} State");
			AddressStateTextBox.ClearSetText(name);
			Store.State = name;
		}

		public void SetZip(string name)
		{
			Log.Info($"Setting {name} Zip");
			AddressZipTextBox.ClearSetText(name);
			Store.Zip = name;
		}

		public void SetCountryDropDown(string name)
		{
			Log.Info($"Setting {name} Country");
			OpenCountryDropDown();
			var countryOptionButton = new Button(By.XPath($"//div[contains(@class, 'open')]//a[text()='{name}']/.."), name + " option");
			countryOptionButton.Click();
			Store.Country = name;
		}

		public void OpenCountryDropDown()
		{
			Log.Info("Opening country dropdown");
			AddressCountryButton.Click();
			try
			{
				expandedDropdownButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				AddressCountryButton.Click();
			}
		}
	}
}