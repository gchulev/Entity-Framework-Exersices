namespace CarDealer.DTOs.Export
{
    using System.Collections.Generic;

    using CarDealer.Models;
    public class ExportCar2ndDto
    {
        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }
    }
}
