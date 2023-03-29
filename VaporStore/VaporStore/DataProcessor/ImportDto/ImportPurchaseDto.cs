using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.ImportDto
{
    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [XmlAttribute("title")]
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        [XmlElement("Type")]
        public PurchaseType Type { get; set; }

        [XmlElement("Key")]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        [Required]
        public string Key { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{4}\s\d{4}\s\d{4}\s\d{4}$")]
        [XmlElement("Card")]
        public string Card { get; set; } = null!;

        [Required]
        public string Date { get; set; } = null!;
    }
}
