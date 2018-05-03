using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GraphTest
{
    public class AuthProvider : IAuthenticationProvider
    {
        private readonly GraphAuth graphAuth;
        private readonly int maxRetries = 5;

        public AuthProvider(GraphAuth auth)
        {
            graphAuth = auth;
        }

        public Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            try
            {
                int retries = 0;
                do
                {
                    AuthenticationResult authResult = graphAuth.GetAuthResult();
                    string accessToken = authResult.AccessToken;
                    if (accessToken != null && accessToken.Length > 20)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        return Task.FromResult(0);
                    }

                    Thread.Sleep(retries * 50);
                    retries++;
                }
                while (retries < maxRetries);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"AuthTokenHandler:{ex}");
            }

            return Task.FromResult(0);
        }
    }
}