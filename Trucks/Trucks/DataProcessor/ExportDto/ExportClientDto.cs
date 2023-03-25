using Trucks.Data.Models;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportClientDto
    {
        public string Name { get; set; } = null!;
        public ExportTruckDto[] Trucks { get; set; }
    }
}
