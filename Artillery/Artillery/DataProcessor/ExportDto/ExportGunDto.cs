using System.Xml.Serialization;

using Artillery.Data.Models;
using Artillery.Data.Models.Enums;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunDto
    {
        [XmlAttribute("Manufacturer")]
        public string? Manufacturer { get; set; } = null!;

        [XmlAttribute("GunType")]
        public GunType GunType { get; set; }

        [XmlAttribute("GunWeight")]
        public int GunWeight { get; set; }

        [XmlAttribute("BarrelLength")]
        public double BarrelLength { get; set; }

        [XmlAttribute("Range")]
        public int Range { get; set; }

        [XmlArray("Countries")]
        public ExportCuntryDto[]? Countries { get; set; }
    }
}
