using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("SelectOption")]
    public class SelectOption
    {
        [XmlElement("Value")]
        public string Value
        {
            get;
            set;
        }

        [XmlElement("Text")]
        public string Text
        {
            get;
            set;
        }

        [XmlElement("Checked")]
        public bool Checked
        {
            get;
            set;
        }
    }
}