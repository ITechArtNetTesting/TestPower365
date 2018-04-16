using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class RegistrationForm : BaseForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//span[contains(text(), 'NEW USER REGISTRATION')]");

		private readonly TextBox AddressCityTextBox ;

		private readonly Button AddressCountryButton;

		private readonly TextBox AddressLine1TextBox ;

		private readonly TextBox AddressStateTextBox ;

		private readonly TextBox AddressZipTextBox;

		private readonly Button backButton ;

		private readonly TextBox CientNameTextBox;

		private readonly TextBox ContactEmailTextBox;

		private readonly TextBox ContactFirstNameTextBox;

		private readonly TextBox ContactLastNameTextBox;

		private readonly TextBox ContactPhoneTextBox;

		private readonly Button expandedDropdownButton;

		private readonly Button nextButton ;

		private readonly Button registerButton;

		private readonly Button submitButton ;

		public RegistrationForm(Guid driverId) : base(TitleLocator, "Registration Form",driverId)
		{
            this.driverId = driverId;
            AddressCityTextBox= new TextBox(By.Id("AddressCity"), "Address City",driverId);
            AddressCountryButton = new Button(By.Id("countrySelector"), "Address Country", driverId);
            AddressLine1TextBox = new TextBox(By.Id("AddressLine1"), "Address Line1", driverId);
            AddressStateTextBox = new TextBox(By.Id("AddressState"), "Address State",driverId);
            AddressZipTextBox = new TextBox(By.Id("AddressZip"), "Address Zip",driverId);
            backButton = new Button(By.XPath("//a[contains(@href, 'history.back')]"), "Back button",driverId);
            CientNameTextBox = new TextBox(By.Id("ClientName"), "Client Name",driverId);
            ContactEmailTextBox = new TextBox(By.Id("ContactEmail"), "Email",driverId);
            ContactFirstNameTextBox = new TextBox(By.Id("ContactFirstName"), "First Name",driverId);
            ContactLastNameTextBox = new TextBox(By.Id("ContactLastName"), "Last Name",driverId);
            ContactPhoneTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'phoneNumber')]"),"Phone",driverId);
            expandedDropdownButton =new Button(By.XPath("//div[contains(@class, 'open')]//*[@id='countrySelector']"),"Expanded country dropdown",driverId);
            nextButton =new Button(By.XPath("//button[contains(@class, 'pull-right')][not(@disabled='')]//span[contains(text(), 'NEXT')]"), "Next button",driverId);
            registerButton = new Button(By.XPath("//button[contains(@type, 'submit')]"), "Register button",driverId);
            submitButton =new Button(By.XPath("//button[contains(@class, 'pull-right')][not(@disabled='')]//span[contains(text(), 'REGISTER')]"), "Submit button",driverId);
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
			var countryOptionButton = new Button(By.XPath($"//div[contains(@class, 'open')]//a[text()='{name}']/.."), name + " option",driverId);
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