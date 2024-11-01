using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("ReturnStepResult")]
    public class ReturnStepResult
    {
        [XmlElement("ReturnStepId")]
        public string ReturnStepId
        {
            get;
            set;
        }

        [XmlElement("ReturnStepName")]
        public string ReturnStepName
        {
            get;
            set;
        }
    }
}
