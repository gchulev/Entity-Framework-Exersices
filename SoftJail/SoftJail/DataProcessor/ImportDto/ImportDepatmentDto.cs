using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepatmentDto
    {
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        public string Name { get; set; } = null!;

        [Required]
        public ImportCellDto[] Cells { get; set; } = null!;
    }
}
