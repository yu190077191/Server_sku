using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("MsgInfo")]
    public class MsgInfo
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Message")]
        public string Message { get; set; }

        [XmlElement("Path")]
        public string Path { get; set; }
    }
}
