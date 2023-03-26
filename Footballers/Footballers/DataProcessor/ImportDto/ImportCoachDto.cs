using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [XmlElement("Name")]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [XmlElement("Nationality")]
        public string Nationality { get; set; } = null!;

        [XmlArray("Footballers")]
        public ImportFootballerDto[]? Footballers { get; set; }
    }
}
