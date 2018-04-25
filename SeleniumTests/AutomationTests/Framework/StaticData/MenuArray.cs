using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.StaticData
{
    public static class MenuArray
    {
        public static Hashtable IntegrationProjectMenu = new Hashtable();

         static MenuArray()
        {
            IntegrationProjectMenu.Add("Users", "manageUsersContainer");
            IntegrationProjectMenu.Add("Distribution Groups", "manageGroupsContainer");
            IntegrationProjectMenu.Add("Public folders", "publicFolderContainer");
            IntegrationProjectMenu.Add("Tenants", "tenantsManagementContainer");
            IntegrationProjectMenu.Add("Directory sync", "tenantsManagementContainer");
            IntegrationProjectMenu.Add("Discovery", "tenantsManagementContainer");
            IntegrationProjectMenu.Add("Address Book", "tenantsManagementContainer");
            IntegrationProjectMenu.Add("Calendar Availability", "tenantsManagementContainer");
        }
        //public static string[] IntegrationProjectMenu =
        //{
          //   "Users",
         //"Distribution Groups",
         //"Public folders",
         //"Tenants",
         //"Directory sync",
         //"Discovery",
         //"Address Book",
         //"Calendar Availability"

    };

    }

