using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Export
{
    public class ExportCateogryByProductDto
    {
        public string? Category { get; set; } = null!;
        public int ProductsCount { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
