using System.Xml;

namespace WF.Framework.Helper
{
    public sealed class XmlHelper
    {
        private XmlHelper()
        {
        }

        public static XmlDocument LoadXml(XmlReader reader)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            return xmlDoc;
        }
    }
}