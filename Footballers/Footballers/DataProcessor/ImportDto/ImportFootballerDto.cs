using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using Footballers.Data.Models.Enums;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [XmlElement("Name")]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; }

        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; }

        [XmlElement("BestSkillType")]
        public BestSkillType BestSkillType { get; set; }

        [XmlElement("PositionType")]
        public PositionType PositionType { get; set; }
    }
}
