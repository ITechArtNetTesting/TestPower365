using System.Xml;

namespace ITechArtTestFramework.StaticClasses
{
    public static class XmlConfigurator
    {
        private static readonly XmlDocument xmlDoc = new XmlDocument();
        public static string RunPath = "Resources/run.xml";
        public static string DownloadPath;
        public static string ResourcesPath = "Resources/";

        public static string GetValueByXpath(string path)
        {
            xmlDoc.Load(RunPath);
            return xmlDoc.SelectSingleNode(path).InnerText;
        }

        public static string GetValue(string tag)
        {
            xmlDoc.Load(RunPath); // Load the XML document from the specified file
            var browser = xmlDoc.GetElementsByTagName(tag);
            return browser[0].InnerText;
        }
    }
}
