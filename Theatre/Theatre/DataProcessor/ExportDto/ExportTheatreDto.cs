using Newtonsoft.Json;

namespace Theatre.DataProcessor.ExportDto
{
    public class ExportTheatreDto
    {
        public string Name { get; set; } = null!;

        [JsonProperty("Halls")]
        public sbyte Halls { get; set; }

        public decimal TotalIncome { get; set; }

        public ExportTicketDto[]? Tickets { get; set; }
    }
}
