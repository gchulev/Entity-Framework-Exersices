﻿namespace CarDealer.DTOs.Export
{
    public class ExportCustomerDto
    {
        public string Name { get; set; } = null!;

        public string BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}
