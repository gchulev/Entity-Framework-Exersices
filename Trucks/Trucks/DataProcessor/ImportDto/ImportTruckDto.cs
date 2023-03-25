using System.Xml.Serialization;

using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [XmlElement("RegistrationNumber")]
        public string RegistrationNumber { get; set; } = null!;

        [XmlElement("VinNumber")]
        public string VinNumber { get; set; } = null!;

        [XmlElement("TankCapacity")]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [XmlElement("MakeType")]
        public int MakeType { get; set; }
    }
}
