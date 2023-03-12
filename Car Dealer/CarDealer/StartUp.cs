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
                string inputJson = File.ReadAllText("D:\\Visual Studio\\Projects\\EntityFrameWork Exersices\\Car Dealer\\CarDealer\\Datasets\\suppliers.json");

                ImportSuppliers(context, inputJson);
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
        #endregion

        #region Data Export

        #endregion
    }
}