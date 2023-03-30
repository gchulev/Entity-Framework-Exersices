using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using SoftJail.Data.Models.Enums;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string FullName { get; set; } = null!;

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement("Money")]
        [Required]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        [Required]
        public string Position { get; set; } = null!;

        [XmlElement("Weapon")]
        [Required]
        public string Weapon { get; set; } = null!;

        [XmlElement("DepartmentId")]
        [Required]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportPrisonerIdDto[] Prisoners { get; set; } = null!;
    }
}
