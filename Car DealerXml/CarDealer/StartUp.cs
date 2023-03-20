using AutoMapper;

using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;

using Microsoft.Extensions.DependencyInjection;

using ProductShop.Utilities;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using (var context = new CarDealerContext())
            {
                string filePath = @"D:\Visual Studio\Projects\EntityFrameWork Exersices\Car DealerXml\CarDealer\Datasets\Parts.xml";
                string inputXml = File.ReadAllText(filePath);

                //Console.WriteLine(ImportSuppliers(context, inputXml));
                Console.WriteLine(ImportParts(context, inputXml));
            }

        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

            IMapper mapper = config.CreateMapper();

            return mapper;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            ImportSupplierDto[] suppliersDto = XmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            Supplier[] suppliers = mapper.ProjectTo<Supplier>(suppliersDto.AsQueryable()).ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            ImportPartDto[] partsDto = XmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

            Part[] parts = partsDto.Where(p => p.SupplierId != null).Select(p => mapper.Map<Part>(p)).ToArray();
            int[] existingSuppliersIds = context.Suppliers
                .Select(s => s.Id).ToArray();

            Part[] filteredParts = parts.Where(p => existingSuppliersIds.Contains(p.SupplierId)).ToArray();

            context.Parts.AddRange(filteredParts);
            context.SaveChanges();

            return $"Successfully imported {filteredParts.Length}";
        }
    }
}