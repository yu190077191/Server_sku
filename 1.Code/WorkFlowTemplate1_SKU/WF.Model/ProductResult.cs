using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("ProductResult")]
    public class ProductResult
    {
        [XmlElement("ProductName")]

        public string ProductName
        {
            get;
            set;
        }
    }
}
