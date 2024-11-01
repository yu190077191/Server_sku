using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("ExcelUpdateCMD")]
    public class ExcelUpdateCMD
    {
        [XmlElement("FilePath")]
        public string FilePath { get; set; }

        [XmlElement("CmdText")]
        public string CmdText { get; set; }

        [XmlElement("Parameters")]
        public string Parameters { get; set; }
    }
}
