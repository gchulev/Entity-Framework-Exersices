﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(20)]
        [MinLength(10)]
        public string Name { get; set; } = null!;

        [XmlElement("Rating")]
        [Required]
        [Range(1, 10.00)]
        public double Rating { get; set; }

        [XmlElement("YearPublished")]
        [Required]
        [Range(2018, 2023)]
        public int YearPublished { get; set; }

        [XmlElement("CategoryType")]
        [Required]
        public string CategoryType { get; set; } = null!;

        [XmlElement("Mechanics")]
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}
