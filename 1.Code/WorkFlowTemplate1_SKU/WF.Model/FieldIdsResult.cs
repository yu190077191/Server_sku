using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("FieldIdsResult")]
    public class FieldIdsResult
    {
        [XmlElement("FieldIds")]

        public string FieldIds
        {
            get;
            set;
        }
    }
}
