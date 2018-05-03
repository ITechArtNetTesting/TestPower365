namespace Product.Framework.Steps
{
	public class AddTenantsSteps : BaseEntity
	{
		private readonly UserSteps User = new UserSteps();

		public void PerformTwoTenantsAdding()
		{
			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T1->T2", "source", "user"), RunConfigurator.GetTenantValue("T1->T2", "source", "password"));

			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(RunConfigurator.GetTenantValue("T1->T2", "target", "user"), RunConfigurator.GetTenantValue("T1->T2", "target", "password"));

			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
		}

		public void PerformTwoTenantsAdding(string sourceTenant, string sourcePassword, string targetTenant,
			string targetPassword)
		{
			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(sourceTenant, sourcePassword);
            
			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(targetTenant, targetPassword);

			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
		}

		public void PerformOneTenantAdding(string tenant, string password)
		{
			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(tenant, password);
            
			Browser.GetDriver().SwitchTo().Window(Store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);
		}

        protected void Office365Login(string login, string password)
        {
            User.AtOffice365LoginForm().SetLogin(login);
            User.AtOffice365LoginForm().NextClick();
            User.AtOffice365LoginForm().SetPassword(password);
            User.AtOffice365LoginForm().NextClick();
        }

        protected void Office365TenantAuthorization(string login, string password)
        {
            User.AtOffice365LoginForm().UseAnotherAccountClick();
            Office365Login(login, password);
            User.AtOffice365LoginForm().NextClick();
        }
    }
}