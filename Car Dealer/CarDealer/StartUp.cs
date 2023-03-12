using System.Runtime.Versioning;

using AutoMapper;

using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;

using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using (var context = new CarDealerContext())
            {
                string inputJson = File.ReadAllText("D:\\Visual Studio\\Projects\\EntityFrameWork Exersices\\Car Dealer\\CarDealer\\Datasets\\parts.json");

                //ImportSuppliers(context, inputJson);
                ImportParts(context, inputJson);

            }
        }
        private static IMapper ProvideMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = config.CreateMapper();

            return mapper;
        }

        #region Data Import
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var mapper = ProvideMapper();

            ImportSupplierDto[] suppliersDto = JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson)!;

            Supplier[] suppliers = suppliersDto.Select(s => mapper.Map<Supplier>(s)).ToArray();

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = ProvideMapper();

            ImportPartDto[] importPartsDto = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson)!;

            Part[] parts = importPartsDto.Where(p => context.Suppliers.Find(p.SupplierId) != null)
                .Select(p => mapper.Map<Part>(p))
                .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}.";
        }
        #endregion

        #region Data Export

        #endregion
    }
}