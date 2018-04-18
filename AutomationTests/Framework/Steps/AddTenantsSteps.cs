using System;

namespace Product.Framework.Steps
{
	public class AddTenantsSteps : BaseEntity
	{
        private Guid driverId;

        RunConfigurator configurator;

        Store store;

        public AddTenantsSteps(Guid driverId,RunConfigurator configurator,Store store)
        {
            this.store = store;
            this.configurator = configurator;
            this.driverId = driverId;
            User = new UserSteps(driverId, configurator,store);
        }

        private readonly UserSteps User;

		public void PerformTwoTenantsAdding()
		{
			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(configurator.GetTenantValue("T1->T2", "source", "user"), configurator.GetTenantValue("T1->T2", "source", "password"));

			Driver.GetDriver(driverId).SwitchTo().Window(store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(configurator.GetTenantValue("T1->T2", "target", "user"), configurator.GetTenantValue("T1->T2", "target", "password"));

			Driver.GetDriver(driverId).SwitchTo().Window(store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
		}

		public void PerformTwoTenantsAdding(string sourceTenant, string sourcePassword, string targetTenant,
			string targetPassword)
		{
			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(sourceTenant, sourcePassword);
            
			Driver.GetDriver(driverId).SwitchTo().Window(store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(1);

            User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(targetTenant, targetPassword);

			Driver.GetDriver(driverId).SwitchTo().Window(store.MainHandle);
			User.AtAddTenantsForm().WaitForTenantAdded(2);
		}

		public void PerformOneTenantAdding(string tenant, string password)
		{
			User.AtAddTenantsForm().OpenOffice365LoginFormPopup();
            Office365TenantAuthorization(tenant, password);
            
			Driver.GetDriver(driverId).SwitchTo().Window(store.MainHandle);
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