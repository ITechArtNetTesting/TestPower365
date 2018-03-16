using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;

namespace Product.Tests.ClientAdministrationTests
{
	[TestClass]
	public class ClientRegistration : BaseTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("ClientAdministration")]
		public void AddRegisteredClient()
		{

		    try
		    {
		        User.AtMainForm().GoToClientRegistration();
		        //Fill in First name
		        User.AtRegistrationForm().SetFirstName(StringRandomazer.MakeRandomFirstName());
		        //Fill in Last name
		        User.AtRegistrationForm().SetLastName(StringRandomazer.MakeRandomLastName());
		        //Fill in Phone
		        User.AtRegistrationForm().SetPhone(StringRandomazer.MakeRandomPhone());
		        //Fill in Email
		        User.AtRegistrationForm().SetEmail(RunConfigurator.GetValue("client.usermail"));
		        User.AtRegistrationForm().GoNext();
		        //Fill in Random Company Name
		        User.AtRegistrationForm().SetClientName(StringRandomazer.MakeRandomClientName());
		        //Fill in Address
		        User.AtRegistrationForm().SetAddress(StringRandomazer.MakeRandomAddress());
		        //Fill in City 
		        User.AtRegistrationForm().SetCity(RunConfigurator.GetValue("client.city"));
		        //Fill in State
		        User.AtRegistrationForm().SetState(RunConfigurator.GetValue("client.state"));
		        //Fill in Country
		        User.AtRegistrationForm().SetCountryDropDown(RunConfigurator.GetValue("client.country"));
		        //Fill in zip
		        User.AtRegistrationForm().SetZip(StringRandomazer.MakeRandomZip());
		        //Procced to Register
		        User.AtRegistrationForm().Submit();
		        User.AtOffice365LoginForm().SetLogin(RunConfigurator.GetValue("client.usermail"));
		        User.AtOffice365LoginForm().SetPassword(RunConfigurator.GetValue("client.password"));
                User.AtOffice365LoginForm().NextClick();
		        //Validate Tenant Project List View is displayed with no data.
		        User.AtTenantRestructuringForm().AssertNoDataForNewClient();
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}
