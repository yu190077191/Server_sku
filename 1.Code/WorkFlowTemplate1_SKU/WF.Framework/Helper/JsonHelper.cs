using System.Web.Script.Serialization;
using System.Xml;
using System.Collections.Generic;

namespace WF.Framework.Helper
{
    public sealed class JsonHelper
    {
        private JsonHelper()
        {
        }

        public static string SerializeJson(object model)
        {
            var jss = new JavaScriptSerializer();

            return jss.Serialize(model);
        }

        public static T DeserializeJson<T>(string value)
        {
            var jss = new JavaScriptSerializer();
            return jss.Deserialize<T>(value);
        }

        public static string GetXmlString(string sJson)
        {
            XmlDocument doc = new XmlDocument();
            if(sJson.IndexOf("[{") == 0)
            {
                doc = Json2XmlArray(sJson);
            }
            else
            {
                doc = Json2Xml(sJson);
            }    

            string xml = doc.OuterXml;
            int p = xml.IndexOf("?>");
            if(p > 0)
            {
                xml = xml.Substring(p + 2);
            }

            return xml;
        }

        public static XmlDocument Json2Xml(string sJson)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            Dictionary<string, object> Dic = (Dictionary<string, object>)oSerializer.DeserializeObject(sJson);
            XmlDocument doc = new XmlDocument();
            //XmlDeclaration xmlDec;
            //xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            //doc.InsertBefore(xmlDec, doc.DocumentElement);
            XmlElement nRoot = doc.CreateElement("root");
            doc.AppendChild(nRoot);
            foreach (KeyValuePair<string, object> item in Dic)
            {
                XmlElement element = doc.CreateElement(item.Key);
                KeyValue2Xml(element, item);
                nRoot.AppendChild(element);
            }

            return doc;
        }

        public static XmlDocument Json2XmlArray(string sJson)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            XmlDocument doc = new XmlDocument();
            string[] array = sJson.Split(new string[] { "}" }, System.StringSplitOptions.None);
            XmlElement nRoot = doc.CreateElement("root");
            doc.AppendChild(nRoot);
            foreach(string s in array)
            {
                string str = s.Replace("[{", "{").Replace(",{", "{") + "}";
                if(str.Contains(":"))
                {
                    XmlElement sub = doc.CreateElement("item");
                    nRoot.AppendChild(sub);

                    Dictionary<string, object> Dic = (Dictionary<string, object>)oSerializer.DeserializeObject(str);
                    foreach (KeyValuePair<string, object> item in Dic)
                    {
                        XmlElement element = doc.CreateElement(item.Key);
                        KeyValue2Xml(element, item);
                        sub.AppendChild(element);
                    }
                }
            }

            return doc;
        }

        private static void KeyValue2Xml(XmlElement node, KeyValuePair<string, object> Source)
        {
            object kValue = Source.Value;
            if (kValue.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> item in kValue as Dictionary<string, object>)
                {
                    XmlElement element = node.OwnerDocument.CreateElement(item.Key);
                    KeyValue2Xml(element, item);
                    node.AppendChild(element);
                }
            }
            else if (kValue.GetType() == typeof(object[]))
            {
                object[] o = kValue as object[];
                for (int i = 0; i < o.Length; i++)
                {
                    XmlElement xitem = node.OwnerDocument.CreateElement("Item");
                    KeyValuePair<string, object> item = new KeyValuePair<string, object>("Item", o[i]);
                    KeyValue2Xml(xitem, item);
                    node.AppendChild(xitem);
                }

            }
            else
            {
                XmlText text = node.OwnerDocument.CreateTextNode(kValue.ToString());
                node.AppendChild(text);
            }
        }
    }
}