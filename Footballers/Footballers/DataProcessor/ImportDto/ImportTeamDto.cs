using System.ComponentModel.DataAnnotations;

using Footballers.Data.Models;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-]+$")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; } = null!;

        [Required]
        public int Trophies { get; set; }

        [Required]
        public int[]? Footballers { get; set; }
    }
}
