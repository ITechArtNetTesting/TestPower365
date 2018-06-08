using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.Services
{
    public class RemoteCacheService
    {
        private static ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("10.1.17.229:6379");

        private const string RESOURCE_IN_USE = "ResourceInUse";

        private const string ResourceFormatString = "{0}|{1}|{0}";

        public RemoteCacheService() { }

        public void SetResourceInUse(string client, string project, string entry)
        {
            var db = GetDatabase();
            db.SetAdd(RESOURCE_IN_USE, BuildResource(client, project, entry));
        }

        public void SetResourceFree(string client, string project, string entry)
        {
            var db = GetDatabase();
            db.SetRemove(RESOURCE_IN_USE, BuildResource(client, project, entry));
        }

        public bool IsResourceInUse(string client, string project, string entry)
        {
            var db = GetDatabase();
            return db.SetContains(RESOURCE_IN_USE, BuildResource(client, project, entry));
        }

        private IDatabase GetDatabase()
        {
            return _redis.GetDatabase();
        }

        private string BuildResource(string client, string project, string entry)
        {
            return string.Format("{0}|{1}|{2}", client, project, entry);
        }
    }
}
