using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using GraphTest;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Console
{
    class Program
    {
        static void Main()
        {
            CancellationToken token = CancellationToken.None;

            GraphAuth graphAuth = new GraphAuth("btcloud1");
            AuthenticationResult result = null;

            try
            {
                result = graphAuth.GetAuthResult();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            if (result != null)
            {
            }

            O365Graph graph = new O365Graph(graphAuth);

            int orgCount = 0;
            foreach (var orgs in graph.GetOrganizations(token))
            {
                orgCount += orgs.Count;
            }
            Trace.WriteLine($"Organization count: {orgCount}");

            int userCount = 0;
            foreach (var users in graph.GetUsers(token))
            {
                userCount += users.Count;
            }
            Trace.WriteLine($"User count: {userCount}");

            int dlCount = 0;
            foreach (var distributionGroups in graph.GetDistributionGroups(token))
            {
                dlCount += distributionGroups.Count;
            }
            Trace.WriteLine($"DistributionGroup count: {dlCount}");

            int groupCount = 0;
            foreach (List<Group> groups in graph.GetGroups(token))
            {
                groupCount += groups.Count;
            }
            Trace.WriteLine($"Group count: {groupCount}");

            int securityCount = 0;
            foreach (var securityGroups in graph.GetMailEnabledSecurityGroups(token))
            {
                securityCount += securityGroups.Count;
            }
            Trace.WriteLine($"SecurityGroup count: {securityCount}");

            int unifiedCount = 0;
            foreach (var specialGroups in graph.GetSpecialGroups("Unified", token))
            {
                unifiedCount += specialGroups.Count;
            }
            Trace.WriteLine($"UnifiedGroup count: {unifiedCount}");

            int probeUserCount = 0;
            foreach (var probeUsers in graph.FindUsersStartsWith("DisplayName", "MailProbe", token))
            {
                probeUserCount += probeUsers.Count;
            }
            Trace.WriteLine($"ProbeUserCount count: {probeUserCount}");
        }
    }
}
