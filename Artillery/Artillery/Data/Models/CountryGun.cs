using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class CountryGun
    {
        [Required]
        public int CountryId { get; set; }

        [Required]
        public Country Country { get; set; } = null!;

        [Required]
        public int GunId { get; set; }

        [Required]
        public Gun Gun { get; set; } = null!;
    }
}
