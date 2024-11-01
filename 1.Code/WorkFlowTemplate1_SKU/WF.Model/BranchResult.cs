using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("BranchResult")]
    public class BranchResult
    {
        [XmlElement("Branch")]

        public string Branch
        {
            get;
            set;
        }
    }
}
