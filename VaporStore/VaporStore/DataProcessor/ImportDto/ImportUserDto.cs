using System.ComponentModel.DataAnnotations;

using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportUserDto
    {
        [Required]
        [RegularExpression(@"^[A-Z][a-z]+\s[A-Z][a-z]+$")]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        [Required]
        public ImportCardDto[] Cards { get; set; } = null!;
    }
}
