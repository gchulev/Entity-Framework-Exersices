using System.ComponentModel.DataAnnotations;

using Footballers.Data.Models.Enums;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Footballers.DataProcessor.ExportDto
{
    public class ExportFootballerDto
    {
        [Required]
        [StringLength(40, MinimumLength = 2)]
        [JsonProperty(Order = 1)]
        public string FootballerName { get; set; } = null!;

        //[JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(Order = 5)]
        public string PositionType { get; set; }

        // [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(Order = 4)]
        public string BestSkillType { get; set; }

        [JsonProperty(Order = 2)]
        public DateTime ContractStartDate { get; set; }

        [JsonProperty(Order = 3)]
        public DateTime ContractEndDate { get; set; }
    }
}
