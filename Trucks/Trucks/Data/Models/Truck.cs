using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        public Truck()
        {
            this.ClientsTrucks = new List<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        [RegularExpression(@"^[A-Z]{2}\d{4}[A-Z]{2}$")]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        [StringLength(17)]
        public string VinNumber { get; set; } = null!;

        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public CategoryType CategoryType { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public MakeType MakeType { get; set; }

        [Required]
        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId  { get; set; }

        public Despatcher Despatcher { get; set; } = null!;

        public List<ClientTruck> ClientsTrucks { get; set; } = null!;
    }
}
