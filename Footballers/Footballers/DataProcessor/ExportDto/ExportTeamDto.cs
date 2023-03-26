using System.ComponentModel.DataAnnotations;

using Footballers.Data.Models;

namespace Footballers.DataProcessor.ExportDto
{
    public class ExportTeamDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-]+$")]
        public string Name { get; set; } = null!;

        public ExportFootballerDto[]? Footballers { get; set; }
    }
}
