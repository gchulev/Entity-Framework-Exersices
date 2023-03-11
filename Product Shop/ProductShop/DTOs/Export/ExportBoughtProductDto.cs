using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Export
{
    public class ExportBoughtProductDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string BuyerFirstName { get; set; } = null!;
        public string BuyerLastName { get; set; } = null!;
    }
}
