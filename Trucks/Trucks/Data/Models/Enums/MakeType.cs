using System.Xml.Serialization;

namespace Trucks.Data.Models.Enums
{
    [XmlType(IncludeInSchema = true)]
    public enum MakeType
    {
        Daf,
        Man,
        Mercedes,
        Scania,
        Volvo
    }
}
