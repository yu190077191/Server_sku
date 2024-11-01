using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace WF.Model
{
    public class PackagingJson
    {
        public int RequestID { get; set; }

        // public PackagingDetails[] PackagingList { get; set; }
        public List<PackagingDetails> PackagingList { get; set; }
    }

    [Serializable]
    [XmlType("PackagingDetails")]
    public class PackagingDetails
    {
        #region Model
        private int _requestid;
        private string _denominator;
        private string _auom;
        private string _numerator;
        private string _buom;
        private string _length;
        private string _width;
        private string _height;
        private string _uomunit;
        private string _grossweight;
        private string _grossunit;
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("RequestID")]
        public int RequestID
        {
            set { _requestid = value; }
            get { return _requestid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Denominator")]
        public string Denominator
        {
            set { _denominator = value; }
            get { return _denominator; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("AUoM")]
        public string AUoM
        {
            set { _auom = value; }
            get { return _auom; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Numerator")]
        public string Numerator
        {
            set { _numerator = value; }
            get { return _numerator; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("BUoM")]
        public string BUoM
        {
            set { _buom = value; }
            get { return _buom; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Length")]
        public string Length
        {
            set { _length = value; }
            get { return _length; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Width")]
        public string Width
        {
            set { _width = value; }
            get { return _width; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Height")]
        public string Height
        {
            set { _height = value; }
            get { return _height; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("UOMUnit")]
        public string UOMUnit
        {
            set { _uomunit = value; }
            get { return _uomunit; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("GrossWeight")]
        public string GrossWeight
        {
            set { _grossweight = value; }
            get { return _grossweight; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("GrossUnit")]
        public string GrossUnit
        {
            set { _grossunit = value; }
            get { return _grossunit; }
        }
        #endregion Model

    }
}

