using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace GraphTest
{
    public class O365Graph
    {
        private readonly IGraphServiceClient client;
        private const string _accessResourceBaseUrl = "https://graph.microsoft.com/beta/";
        private readonly static Type[] messageOnlyExceptions = { typeof(ServiceException) };

        public O365Graph(GraphAuth auth)
        {
            AuthProvider authProvider = new AuthProvider(auth);

            try
            {
                client = new GraphServiceClient(_accessResourceBaseUrl, authProvider);
            }
            catch (ServiceException se)
            {
                Trace.WriteLine(se.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    //You should implement retry and back-off logic per the guidance given here:http://msdn.microsoft.com/en-us/library/dn168916.aspx
                    //InnerException Message will contain the HTTP error status codes mentioned in the link above
                }
            }
        }

        public async Task<User> FindUserByUpn(string upn, CancellationToken token)
        {
            //string request = client.BaseUrl + $"/users/{upn}";
            IUserRequestBuilder urb = client.Users[upn]; //new UserRequestBuilder(request, client);
            try
            {
                User user = await urb.Request().GetAsync(token);
                return user;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<List<SubscribedSku>> GetSubscribedSkus(CancellationToken token)
        {
            IGraphServiceSubscribedSkusCollectionPage page = null;
            try
            {
                page = client.SubscribedSkus.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<Organization>> GetOrganizations(CancellationToken token)
        {
            IGraphServiceOrganizationCollectionPage page = null;
            try
            {
                page = client.Organization.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "GetOrganizations");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<Drive>> GetDrives(CancellationToken token)
        {
            IGraphServiceDrivesCollectionPage page = null;
            try
            {
                page = client.Drives.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<User>> GetUsers(CancellationToken token)
        {
            IGraphServiceUsersCollectionPage page = null;
            try
            {
                page = client.Users.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<Group>> GetSpecialGroups(string groupType, CancellationToken token)
        {
            IGraphServiceGroupsCollectionPage page = null;
            try
            {
                page = client.Groups.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.Where(x => x.GroupTypes.Contains(groupType, StringComparer.InvariantCultureIgnoreCase)).ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<Group>> GetGroups(CancellationToken token)
        {
            IGraphServiceGroupsCollectionPage page = null;
            try
            {
                page = client.Groups.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<Group>> GetDistributionGroups(CancellationToken token)
        {
            // We will let Graph filter what it can, but functionality seems to be limited for now.
            // https://msdn.microsoft.com/en-us/library/azure/ad/graph/howto/azure-ad-graph-api-supported-queries-filters-and-paging-options

            IGraphServiceGroupsCollectionPage page = null;

            try
            {
                page = client.Groups.Request()
                    .Filter("securityEnabled eq false") // Non-security groups
                    .GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<Group>> GetMailEnabledSecurityGroups(CancellationToken token)
        {
            // We will let Graph filter what it can, but functionality seems to be limited for now.
            // https://msdn.microsoft.com/en-us/library/azure/ad/graph/howto/azure-ad-graph-api-supported-queries-filters-and-paging-options

            // Get all security groups
            // mailEnabled is not available for filtering
            // See https://msdn.microsoft.com/en-us/library/azure/ad/graph/api/entity-and-complex-type-reference#group-entity for details
            IGraphServiceGroupsCollectionPage page = null;

            try
            {
                page = client.Groups.Request()
                    .Filter("securityEnabled eq true")
                    .GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<User>> FindUsersStartsWith(string field, string text, CancellationToken token)
        {
            IGraphServiceUsersCollectionPage page = null;

            try
            {
                page = client.Users.Request()
                    .Filter($"startswith({field}, '{text}')")
                    .GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public async Task<User> FindUserById(string id, CancellationToken token)
        {
            IUserRequestBuilder urb = client.Users[id];
            try
            {
                User user = await urb.Request().GetAsync(token);
                return user;
            }
            catch (Exception)
            {
                // expected if we are expanding a group and looking for users but this is a group
            }

            return default(User);
        }

        public async Task<Group> FindGroupById(string id, CancellationToken token)
        {
            IGroupRequestBuilder grb = new GroupRequestBuilder(client.BaseUrl + $"/groups/{id}", client);
            try
            {
                Group group = await grb.Request().GetAsync(token);
                return group;
            }
            catch (Exception)
            {
                // expected if we are expanding a group and looking for sub-groups
            }

            return default(Group);
        }

        public async Task<Group> FindGroupByDisplayNameOrMail(string value, CancellationToken token)
        {
            try
            {
                IGraphServiceGroupsCollectionPage groups = await client.Groups.Request()
                    .Filter($"displayName eq '{value}' or mail eq '{value}'")
                    .GetAsync(token);

                return groups.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, $"Error locating group by displayName or mail using value {value}.");
            }

            return null;
        }

        public async Task<Organization> FindOrganizationById(string id, CancellationToken token)
        {
            IOrganizationRequestBuilder orb = new OrganizationRequestBuilder(client.BaseUrl + $"/organization/{id}", client);
            try
            {
                Organization org = await orb.Request().GetAsync(token);
                return org;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Could not find organization: {id}. Reason={ex.Message}");
            }

            return default(Organization);
        }

        public async Task<Drive> FindDriveById(string id, CancellationToken token)
        {
            IDriveRequestBuilder orb = new DriveRequestBuilder(client.BaseUrl + $"/drives/{id}", client);
            try
            {
                Drive drive = await orb.Request().GetAsync(token);
                return drive;
            }
            catch (Exception)
            {
                Trace.WriteLine($"Could not find drive: {id}");
            }

            return default(Drive);
        }

        public IEnumerable<List<Device>> GetDevices(CancellationToken token)
        {
            IGraphServiceDevicesCollectionPage page = null;
            try
            {
                page = client.Devices.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetGroupMembers(Group group, CancellationToken token)
        {
            IGroupMembersCollectionWithReferencesPage page = null;

            try
            {
                page = client.Groups[group.Id].Members.Request().GetAsync(token).Result;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public async Task<DirectoryObject> FindDirectoryObjectById(string id, CancellationToken token)
        {
            DirectoryObjectRequestBuilder dorb = new DirectoryObjectRequestBuilder(client.BaseUrl + $"/directoryobjects/{id}", client);
            try
            {
                return await dorb.Request().GetAsync(token);
            }
            catch (Exception)
            {
                // expected if we are expanding a group and looking for sub-groups
            }

            return null;
        }

        public IEnumerable<List<DirectoryObject>> GetMemberOf(User user, CancellationToken token)
        {
            IUserMemberOfCollectionWithReferencesPage page = null;

            try
            {
                page = client.Users[user.Id].MemberOf.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get member of for user: {user.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetMemberOf(Group group, CancellationToken token)
        {
            //string request = client.BaseUrl + $"/groups/{group.Id}/memberOf";
            IGroupMemberOfCollectionWithReferencesPage page = null;
            try
            {
                page = client.Groups[group.Id].MemberOf.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get member of for group: {group.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetDirectReports(User user, CancellationToken token)
        {
            //string request = client.BaseUrl + $"/users/{user.Id}/directReports";
            IUserDirectReportsCollectionWithReferencesPage page = null;
            try
            {
                page = client.Users[user.Id].DirectReports.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get direct reports for user: {user.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetCreatedObjects(User user, CancellationToken token)
        {
            //string request = client.BaseUrl + $"/users/{user.Id}/createdObjects";
            IUserCreatedObjectsCollectionWithReferencesPage page = null;
            try
            {
                page = client.Users[user.Id].CreatedObjects.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get created objects for user: {user.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetOwnedObjects(User user, CancellationToken token)
        {
            //string request = client.BaseUrl + $"/users/{user.Id}/ownedObjects";
            IUserOwnedObjectsCollectionWithReferencesPage page = null;
            try
            {
                page = client.Users[user.Id].OwnedObjects.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get owned objects for user: {user.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetOwnedDevices(User user, CancellationToken token)
        {
            //string request = client.BaseUrl + $"/users/{user.Id}/ownedDevices";
            IUserOwnedDevicesCollectionWithReferencesPage page = null;
            try
            {
                page = client.Users[user.Id].OwnedDevices.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get owned devices for user: {user.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetRegisteredDevices(User user, CancellationToken token)
        {
            //string request = client.BaseUrl + $"/users/{user.Id}/registeredDevices";
            IUserRegisteredDevicesCollectionWithReferencesPage page = null;
            try
            {
                page = client.Users[user.Id].RegisteredDevices.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get registered devices for user: {user.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DriveItem>> GetDriveSpecial(Drive drive, CancellationToken token)
        {
            IDriveSpecialCollectionPage page = null;
            try
            {
                page = client.Drives[drive.Id].Special.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get special items for drive: {drive.DriveType ?? "unknown"}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public async Task<DriveItem> GetRoot(Drive drive, CancellationToken token)
        {
            IDriveItemRequestBuilder rb = client.Drives[drive.Id].Root;
            try
            {
                return await rb.Request().GetAsync(token);
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get root for drive: {drive.DriveType ?? "unknown"}");
            }

            return default(DriveItem);
        }

        public IEnumerable<List<Permission>> GetDriveItemPermissions(Drive drive, DriveItem driveItem, CancellationToken token)
        {
            IDriveItemPermissionsCollectionPage page = null;
            try
            {
                page = client.Drives[drive.Id].Items[driveItem.Id].Permissions.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get permissions for drive item: {driveItem.Name ?? "unknown"}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DriveItem>> GetDriveItemChildren(Drive drive, DriveItem driveItem, CancellationToken token)
        {
            IDriveItemChildrenCollectionPage page = null;
            try
            {
                page = client.Drives[drive.Id].Items[driveItem.Id].Children.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get children for drive item: {driveItem.Name ?? "unknown"}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<ThumbnailSet>> GetThumbnails(Drive drive, DriveItem driveItem, CancellationToken token)
        {
            IDriveItemThumbnailsCollectionPage page = null;
            try
            {
                page = client.Drives[drive.Id].Items[driveItem.Id].Thumbnails.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get thumbnails for drive item: {driveItem.Name ?? "unknown"}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetAcceptedSenders(Group group, CancellationToken token)
        {
            IGroupAcceptedSendersCollectionPage page = null;
            try
            {
                page = client.Groups[group.Id].AcceptedSenders.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get accepted senders for group: {group.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetRejectedSenders(Group group, CancellationToken token)
        {
            IGroupRejectedSendersCollectionPage page = null;
            try
            {
                page = client.Groups[group.Id].RejectedSenders.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get rejected senders for group: {group.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetOwners(Group group, CancellationToken token)
        {
            IGroupOwnersCollectionWithReferencesPage page = null;
            try
            {
                page = client.Groups[group.Id].Owners.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get owners for group: {group.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetRegisteredUsers(Device device, CancellationToken token)
        {
            IDeviceRegisteredUsersCollectionWithReferencesPage page = null;
            try
            {
                page = client.Devices[device.Id].RegisteredUsers.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get registered users for device: {device.DisplayName ?? "unknown"}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryObject>> GetRegisteredOwners(Device device, CancellationToken token)
        {
            IDeviceRegisteredOwnersCollectionWithReferencesPage page = null;
            try
            {
                page = client.Devices[device.Id].RegisteredOwners.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get registered owners for device: {device.DisplayName ?? "unknown"}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        private void HandleException(Exception ex, Type[] ignoreTypes, Type[] messageOnlyTypes, string context)
        {
            Type t = ex.GetType();
            string messageText = null;
            StringBuilder sb = new StringBuilder();

            if (IsSet(ignoreTypes, t))
            {
                return;
            }

            if (IsSet(messageOnlyTypes, t))
            {
                messageText = GetExceptionText(ex);
            }

            Exception n1 = ex;
            do
            {
                sb.AppendLine(n1.Message);
                n1 = n1.InnerException;

            }
            while (n1 != null);

            AggregateException ae = ex as AggregateException;
            if (ae != null)
            {
                foreach (Exception ie in ae.InnerExceptions)
                {
                    t = ie.GetType();

                    Exception n2 = ie;
                    do
                    {
                        sb.AppendLine(n2.Message);
                        n2 = n2.InnerException;

                    }
                    while (n2 != null);

                    if (IsSet(ignoreTypes, t))
                    {
                        return;
                    }

                    if (IsSet(messageOnlyTypes, t) && messageText == null)
                    {
                        messageText = GetExceptionText(ie);
                    }
                }
            }

            sb.AppendLine(messageText);

            //string allMessages = sb.ToString().ToLower();
            //bool isMalformed = allMessages.Contains("formed");
            //bool badCreds = allMessages.Contains("credentials");
            //bool notFound = allMessages.Contains("not found");
            //bool isInvalid = allMessages.Contains("id is invalid");

            if (messageText != null)
            {
                if (messageText.Length < 256)
                {
                    Trace.WriteLine($"{context}: {messageText}");
                }
            }
            else
            {
                Trace.WriteLine($"{context}: {ex}");
            }
        }

        private string GetExceptionText(Exception ex)
        {
            ServiceException se = ex as ServiceException;
            if (se != null)
            {
                return $"{se.Error.Code}:{se.Error.Message}";
            }

            return ex.Message;
        }

        private bool IsSet(Type[] types, Type test)
        {
            if (types == null) return false;
            return types.Contains(test);
        }

        public IEnumerable<List<DriveItem>> GetDriveItems(Drive drive, CancellationToken token)
        {
            // this might never work
            IDriveItemsCollectionPage page = null;
            try
            {
                page = client.Drives[drive.Id].Items.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get items for drive: {drive.DriveType ?? "unknown"}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public async Task<Drive> GetUserDrive(User user, CancellationToken token)
        {
            IDriveRequestBuilder rb = client.Users[user.Id].Drive;
            try
            {
                return await rb.Request().GetAsync(token);
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get drive for user: {user.DisplayName}");
            }

            return default(Drive);
        }

        public async Task<Drive> GetGroupDrive(Group group, CancellationToken token)
        {
            IDriveRequestBuilder rb = client.Groups[group.Id].Drive;
            try
            {
                return await rb.Request().GetAsync(token);
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get drive for group: {group.DisplayName}");
            }

            return default(Drive);
        }

        public async Task<bool> DeleteUser(string userUpn, CancellationToken token)
        {
            try
            {
                IUserRequest userRequest = client.Users[userUpn].Request();
                await userRequest.DeleteAsync(token);
                return true;
            }
            catch (ServiceException se)
            {
                if (se.Error.Message.ToLower().Contains("does not exist"))
                {
                    return true;
                }

                Trace.WriteLine(se.ToString());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return false;
        }

        public async Task<User> CreateUser(string userUpn, string userPass, CancellationToken token)
        {
            string[] parts = userUpn.Split('@');
            string adminName = parts[0];

            var user = new User()
            {
                OnPremisesImmutableId = Guid.NewGuid().ToString(),
                MailNickname = adminName,
                DisplayName = adminName,
                AccountEnabled = true,
                UserPrincipalName = userUpn,
                UsageLocation = "US",
                PasswordPolicies = "DisablePasswordExpiration",
                PasswordProfile = new PasswordProfile()
                {
                    ForceChangePasswordNextSignIn = false,
                    Password = userPass,
                },
            };

            try
            {
                var request = client.Users.Request();
                user = await request.AddAsync(user, token);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, $"Error creating {userUpn}");
                user = null;
            }

            return user;
        }

        public async Task<bool> AddRolesToUser(User user, string roleNames, CancellationToken token)
        {
            string[] names = roleNames.Split(',');

            bool userAddedToRole = true;

            foreach (string name in names)
            {
                string roleName = name.Trim();

                try
                {
                    DirectoryRole role = await FindRoleByName(roleName, token);

                    if (role == null)
                    {
                        DirectoryRoleTemplate drt = await FindRoleTemplateByName(roleName, token);
                        if (drt == null)
                        {
                            Trace.WriteLine($"Could not find role template '{roleName}'");
                            return false;
                        }

                        await EnableRole(drt.Id, token);
                        role = await FindRoleByName(roleName, token);

                        if (role == default(DirectoryRole))
                        {
                            Trace.WriteLine($"Could not find role '{roleName}'");
                            return false;
                        }
                    }

                    userAddedToRole &= await AddRoleMember(role, user, token);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex, $"Could not add role(s) '{roleNames}' to PowerShell user {user.UserPrincipalName}");
                    return false;
                }
            }

            return userAddedToRole;
        }

        public async Task<bool> HasRoles(User user, string roleNames, CancellationToken token)
        {
            string[] names = roleNames.Split(',');

            bool userHasRole = true;

            foreach (string name in names)
            {
                string roleName = name.Trim();

                try
                {
                    DirectoryRole role = await FindRoleByName(roleName, token);

                    if (role == null)
                    {
                        DirectoryRoleTemplate drt = await FindRoleTemplateByName(roleName, token);
                        if (drt == default(DirectoryRoleTemplate))
                        {
                            Trace.WriteLine($"Could not find role template '{roleName}'");
                            return false;
                        }

                        await EnableRole(drt.Id, token);
                        role = await FindRoleByName(roleName, token);

                        if (role == default(DirectoryRole))
                        {
                            Trace.WriteLine($"Could not find role '{roleName}'");
                            return false;
                        }
                    }

                    DirectoryObject member = await GetRoleMember(role, user, token);
                    userHasRole &= (member != default(DirectoryObject));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex, $"Could not add role(s) '{roleNames}' to PowerShell user {user.UserPrincipalName}");
                    return false;
                }
            }

            return userHasRole;
        }

        public IEnumerable<List<DirectoryRole>> GetRoles(CancellationToken token)
        {
            IGraphServiceDirectoryRolesCollectionPage page = null;
            try
            {
                page = client.DirectoryRoles.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public async Task<DirectoryRole> FindRoleByName(string name, CancellationToken token)
        {
            IGraphServiceDirectoryRolesCollectionPage page = await client.DirectoryRoles.Request()
                .Filter($"displayname eq {name}")
                .GetAsync(token);

            return page.FirstOrDefault();
        }

        public async Task<DirectoryRole> FindRoleById(string id, CancellationToken token)
        {
            IDirectoryRoleRequestBuilder rb = client.DirectoryRoles[id];
            return await rb.Request().GetAsync(token);
        }

        public async Task<bool> AddRoleMember(DirectoryRole role, User user, CancellationToken token)
        {
            IDirectoryRoleMembersCollectionWithReferencesRequestBuilder rb = client.DirectoryRoles[role.Id].Members;
            try
            {
                await rb.References.Request().AddAsync(user, token);
                return true;
            }
            catch (ServiceException se)
            {
                if (se.Error.Message.ToLower().Contains("already exist"))
                {
                    return true;
                }

                Trace.WriteLine(se.ToString());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return false;
        }

        public async Task<DirectoryObject> GetRoleMember(DirectoryRole role, User user, CancellationToken token)
        {
            IDirectoryObjectWithReferenceRequestBuilder rb = client.DirectoryRoles[role.Id].Members[user.Id];
            try
            {
                return await rb.Request().GetAsync(token);
            }
            catch (ServiceException se)
            {
                Trace.WriteLine(se.ToString());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return default(DirectoryObject);
        }

        public IEnumerable<List<DirectoryObject>> GetRoleMembers(DirectoryRole group, CancellationToken token)
        {
            IDirectoryRoleMembersCollectionWithReferencesPage page = null;
            try
            {
                page = client.DirectoryRoles[group.Id].Members.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, $"Get group members for group: {group.DisplayName}");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public IEnumerable<List<DirectoryRoleTemplate>> GetRoleTemplates(CancellationToken token)
        {
            IGraphServiceDirectoryRoleTemplatesCollectionPage page = null;
            try
            {
                page = client.DirectoryRoleTemplates.Request().GetAsync(token).Result;
            }
            catch (Exception ex)
            {
                HandleException(ex, null, messageOnlyExceptions, "");
            }

            while (page != null)
            {
                yield return page.ToList();
                if (page.NextPageRequest == null) break;
                page = page.NextPageRequest.GetAsync(token).Result;
            }
        }

        public async Task<DirectoryRoleTemplate> FindRoleTemplateByName(string name, CancellationToken token)
        {
            IGraphServiceDirectoryRoleTemplatesCollectionPage page = await client.DirectoryRoleTemplates.Request()
                .Filter($"DisplayName eq {name}")
                .GetAsync(token);

            return page.FirstOrDefault();
        }

        public async Task<DirectoryRoleTemplate> FindRoleTemplateById(string id, CancellationToken token)
        {
            IDirectoryRoleTemplateRequestBuilder rb = client.DirectoryRoleTemplates[id];
            return await rb.Request().GetAsync(token);
        }

        public async Task<bool> EnableRole(string templateId, CancellationToken token)
        {
            string url = client.BaseUrl + "/DirectoryRoles";

            HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Post, url);

            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("roleTemplateId", templateId);
            string text = JsonConvert.SerializeObject(body);

            hrm.Content = new StringContent(text, Encoding.UTF8, "application/json");

            await client.AuthenticationProvider.AuthenticateRequestAsync(hrm);

            try
            {
                HttpResponseMessage x = await client.HttpProvider.SendAsync(hrm, HttpCompletionOption.ResponseContentRead, token);
                return x.IsSuccessStatusCode;
            }
            catch (ServiceException se)
            {
                if (se.Error.Message.ToLower().Contains("conflicting"))
                {
                    return true;
                }

                Trace.WriteLine(se.ToString());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return false;
        }
    }
}