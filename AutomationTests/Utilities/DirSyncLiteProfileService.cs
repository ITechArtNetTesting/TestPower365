using BTCodex.DataModel;
using System.Linq;
using System.Xml.Linq;

namespace Product.Utilities
{
    public class DirSyncLiteProfileService
    {
        private readonly string _sqlConnectionString;

        public DirSyncLiteProfileService(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }


        public void SetTenantId(string profileName, int tenantId)
        {
            using (var db = new BTCodexDataClassesDataContext(_sqlConnectionString))
            {
                var configs = db.BT_Configs.ToList();
                foreach (var item in configs)
                {
                    var xml = item.ConfigXml.Value;
                    XDocument xmlDoc = XDocument.Parse(xml);

                    var syncName = xmlDoc.Descendants().Where(e => e.Name.LocalName == "SyncName").FirstOrDefault();
                    if (syncName.Value != profileName)
                        continue;

                    var sourceContainerDefaults = xmlDoc.Descendants().Where(e => e.Name.LocalName == "SourceContainerDefaults").FirstOrDefault();
                    var element = sourceContainerDefaults.Descendants().Where(e => e.Name.LocalName == "TenantId").FirstOrDefault();
                    element.Value = tenantId.ToString();

                    var result = xmlDoc.ToString(SaveOptions.DisableFormatting);
                    item.ConfigXml = new XElement("Value", result);
                    db.SubmitChanges();
                }
            }
        }
    }
}