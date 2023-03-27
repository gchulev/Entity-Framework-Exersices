using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Country")]
    public class ExportCuntryDto
    {
        [XmlAttribute("Country")]
        public string? Country { get; set; }

        [XmlAttribute("ArmySize")]
        public string? ArmySize { get; set; }
    }
}
