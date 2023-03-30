using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportMailDto
    {
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Sender { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[0-9a-zA-Z\s]+\sstr.$")]
        public string Address { get; set; } = null!;
    }
}
