using System.Xml.Serialization;

using Footballers.Data.Models.Enums;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Footballer")]
    public class ExportShortFootballerDto
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string Position { get; set; }
    }
}
