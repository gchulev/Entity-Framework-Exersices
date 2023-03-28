using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string FullName { get; set; } = null!;

        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]
        [RegularExpression(@"^\+44-\d+-\d+-\d+$")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId  { get; set; }

        [Required]
        public Play Play { get; set; } = null!;
    }
}
