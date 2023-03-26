using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models
{
    public class Team
    {
        public Team()
        {
            this.TeamsFootballers = new HashSet<TeamFootballer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-]+$")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; } = null!;

        [Required]
        public int Trophies { get; set; }

        public ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}
