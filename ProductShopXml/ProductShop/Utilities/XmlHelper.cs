using ProductShop.DTOs.Import;
using System.Xml.Serialization;

namespace ProductShop.Utilities
{
    internal class XmlHelper
    {
        public static T Deserialize<T>(string inputXml, string rootElm)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootElm);

            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);

            using (StreamReader reader = new StreamReader(inputXml))
            {
                T serializedObj = (T)serializer.Deserialize(reader)!;

                return serializedObj;
            }
        }
    }
}
