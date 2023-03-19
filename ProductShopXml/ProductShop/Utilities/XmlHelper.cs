using ProductShop.DTOs.Import;

using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Utilities
{
    internal class XmlHelper
    {
        public static T Deserialize<T>(string inputXml, string rootElm)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootElm);

            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);

            using (StringReader reader = new StringReader(inputXml))
            {
                T serializedObj = (T)serializer.Deserialize(reader)!;

                return serializedObj;
            }
        }

        public static string Serialize<T>(T inputObj, string rootElm)
        {
            var sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootElm);

            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);

            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, inputObj, nameSpaces);

                return sb.ToString().TrimEnd();
            }
        }
    }
}
