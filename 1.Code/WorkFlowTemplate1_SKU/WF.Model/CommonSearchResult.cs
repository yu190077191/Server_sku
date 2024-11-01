using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("CommonSearchResult")]
    public class CommonSearchResult
    {
        [XmlElement("Id")]
        public int Id
        {
            get;
            set;
        }

        [XmlElement("Value")]
        public string Value
        {
            get;
            set;
        }

        [XmlElement("Name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("ParentName")]
        public string ParentName
        {
            get;
            set;
        }

        [XmlElement("Description")]
        public string Description
        {
            get;
            set;
        }
    }
}