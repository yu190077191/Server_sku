using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Framework.Helper
{
    public sealed class SerializationHelper
    {
        private SerializationHelper()
        {
        }

        public static T Deserialize<T>(string value)
        {
            using (var sr = new StringReader(value))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
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