using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlRoot("Users")]
    public class ExportUsersDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public ExportUserDto[]? Users { get; set; }
    }
}
