using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace KontrolService
{
    public class XMLSerializer
    {
        public static string Serialize(object obj, string defaultNamespace = null, bool indent = true)
        {
            var serializer = defaultNamespace == null
                ? new XmlSerializer(obj.GetType())
                : new XmlSerializer(obj.GetType(), defaultNamespace);

            var stringBuilder = new StringBuilder();
            using (var stringWriter = new System.IO.StringWriter(stringBuilder))
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = indent }))
                {
                    serializer.Serialize(xmlWriter, obj);
                }
            }
            return stringBuilder.ToString();
        }

        public static T Deserialize<T>(string xmlString)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(xmlString))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}
