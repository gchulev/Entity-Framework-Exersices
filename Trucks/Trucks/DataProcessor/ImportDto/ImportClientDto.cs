namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientDto
    {
        public string Name { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int[] Trucks { get; set; } = null!;
    }
}
