using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class ExportBoardgameDto
    {
        [XmlElement("BoardgameName")]
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        [XmlElement("BoardgameYearPublished")]
        [Required]
        public int YearPublished { get; set; }
    }
}
