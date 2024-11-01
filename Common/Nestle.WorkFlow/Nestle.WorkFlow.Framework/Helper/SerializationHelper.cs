using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Framework.Helper
{
    public sealed class SerializationHelper
    {
        private SerializationHelper()
        {
        }

        public static bool IsW3CCompliant(char c)
        {
            int charInt = Convert.ToInt32(c);
            return charInt == 9 || charInt == 10 || charInt == 13 || (charInt >= 32 && charInt <= 55295) || (charInt >= 57344 && charInt <= 65533) || (charInt >= 65536 && charInt <= 1114111);
        }

        public static string ScrubString(string xmlStr)
        {
            if (xmlStr == string.Empty || xmlStr == null)
            {
                return xmlStr;
            }

            string pattern = @"[^\w\.@-]";
            System.Text.StringBuilder strB = new System.Text.StringBuilder(xmlStr.Length);

            //-- If there are no special chars just return the original (99%)
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            if (!regex.Match(xmlStr).Success)
            {
                return xmlStr;
            }

            char[] charArray = xmlStr.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                if (IsW3CCompliant(charArray[i]))
                {
                    strB.Append(charArray[i]);
                }
            }

            return strB.ToString();
        }

        public static T Deserialize<T>(string value)
        {
            try
            {
                using (var sr = new StringReader(value))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(sr);
                }
            }
            catch
            {
                value = ScrubString(value);
                value = value.Replace("&#xB;", string.Empty);
                using (var sr = new StringReader(value))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(sr);
                }
            }
        }

        public static T Deserialize<T>(string value, XmlAttributeOverrides overrides)
        {
            using (var sr = new StringReader(value))
            {
                var serializer = new XmlSerializer(typeof(T), overrides);
                return (T)serializer.Deserialize(sr);
            }
        }

        public static string Serialize(object model)
        {
            if (model == null)
            {
                return string.Empty;
            }

            using (var stream = new MemoryStream())
            {
                var doc = new XmlDocument();
                var serializer = new XmlSerializer(model.GetType());
                serializer.Serialize(stream, model);
                stream.Seek(0, SeekOrigin.Begin);
                doc.Load(stream);

                return doc.OuterXml;
            }
        }

        public static string Serialize(object model, XmlAttributeOverrides overrides)
        {
            using (var stream = new MemoryStream())
            {
                var doc = new XmlDocument();
                var serializer = new XmlSerializer(model.GetType(), overrides);
                serializer.Serialize(stream, model);
                stream.Seek(0, SeekOrigin.Begin);
                doc.Load(stream);

                return doc.OuterXml;
            }
        }

        public static XmlAttributeOverrides GetEnumClassFilter(List<Type> typeList)
        {
            var overrides = new XmlAttributeOverrides();
            foreach (Type type in typeList)
            {
                string[] names = Enum.GetNames(type);
                Array values = Enum.GetValues(type);
                for (int i = 0; i < names.Length; i++)
                {
                    var attrs = new XmlAttributes();
                    var xmlEnum = new XmlEnumAttribute();
                    xmlEnum.Name = ((int)values.GetValue(i)).ToString(Constants.CurrentCulture);
                    attrs.XmlEnum = xmlEnum;
                    overrides.Add(type, names[i], attrs);
                }
            }

            return overrides;
        }
    }
}