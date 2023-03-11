using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProductShop.Models;

namespace ProductShop.DTOs.Export
{
    public class ExportUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public ICollection<ExportBoughtProductDto>? SoldProducts { get; set; }
    }
}
