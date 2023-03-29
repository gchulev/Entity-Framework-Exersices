using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportCardDto
    {
        [Required]
        [RegularExpression(@"^\d{4}\s\d{4}\s\d{4}\s\d{4}$")]
        public string Number { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{3}$")]
        [MinLength(3)]
        [MaxLength(3)]
        public string Cvc { get; set; } = null!;

        [Required]
        public CardType Type { get; set; }
    }
}
