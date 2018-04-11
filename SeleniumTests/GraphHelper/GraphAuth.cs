using System.Configuration;
using System.Threading;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GraphTest
{
    public class GraphAuth
    {
        private const string aadInstance = "https://login.microsoftonline.com/common";
        private const string resourceId = "https://graph.microsoft.com";
        private readonly string tenantUid;
        private readonly string appId;
        private readonly string user;
        private readonly string pwd;

        public GraphAuth(string tenantName)
        {
            tenantUid = ConfigurationManager.AppSettings[$"{tenantName}:TenantId"];
            appId = ConfigurationManager.AppSettings[$"{tenantName}:AppId"];
            user = ConfigurationManager.AppSettings[$"{tenantName}:User"];
            pwd = ConfigurationManager.AppSettings[$"{tenantName}:Pwd"];
        }

        public GraphAuth(string tenantUid, string appId, string user, string pwd)
        {
            this.tenantUid = tenantUid;
            this.appId = appId;
            this.user = user;
            this.pwd = pwd;
        }

        private AuthenticationContext authContext;
        private AuthenticationContext AuthContext
        {
            get { return authContext ?? (authContext = new AuthenticationContext($@"{aadInstance}/{tenantUid}", true)); }
        }

        public AuthenticationResult GetAuthResult()
        {
            int retryCount = 3;
            int retryCountLoop = 0;
            bool retry = false;
            AuthenticationResult result = null;

            do
            {
                try
                {
                    // ADAL includes an in memory cache, so this call will only send a message to the server if the cached token is expired.
                    //ClientCredential clientCredential = new ClientCredential(appId, client_secret);
                    //result = AuthContext.AcquireTokenAsync(resourceId, clientCredential).Result;

                    UserPasswordCredential uc = new UserPasswordCredential(user, pwd);
                    result = AuthContext.AcquireTokenAsync(resourceId, appId, uc).Result;
                }
                catch (AdalException ex)
                {
                    if (ex.ErrorCode == "temporarily_unavailable" && retryCountLoop < retryCount)
                    {
                        retry = true;
                        retryCount++;
                        const int timeoutMilliseconds = 3000;
                        Thread.Sleep(timeoutMilliseconds);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            while ((retry) && (retryCountLoop < retryCount));

            return result;
        }

    }
}