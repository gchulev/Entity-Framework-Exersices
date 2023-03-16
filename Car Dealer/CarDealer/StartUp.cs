using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

using AutoMapper;

using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using (var context = new CarDealerContext())
            {
                string inputJson = File.ReadAllText("D:\\Visual Studio\\Projects\\EntityFrameWork Exersices\\Car Dealer\\CarDealer\\Datasets\\sales.json");

                //ImportSuppliers(context, inputJson);
                //ImportParts(context, inputJson);
                //Console.WriteLine(ImportCars(context, inputJson));
                //ImportCustomers(context, inputJson);
                //ImportSales(context, inputJson);
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
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = ProvideMapper();

            ImportCarDto[] importCars = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson)!;

            foreach (var importCar in importCars)
            {
                var car = new Car()
                {
                    Make = importCar.Make,
                    Model = importCar.Model,
                    TravelledDistance = importCar.TraveledDistance
                };

                context.Cars.Add(car);
                context.SaveChanges();

                foreach (int partId in importCar.PartsId!)
                {
                    var part = context.Parts.AsNoTracking().FirstOrDefault(p => p.Id == partId);
                    if (part != null)
                    {
                        if (!context.Cars.Any(c => c.PartsCars.Any(pc => pc.PartId == partId && pc.CarId == car.Id)))
                        {
                            var partCar = new PartCar
                            {
                                CarId = car.Id,
                                PartId = partId
                            };
                            context.PartsCars.Add(partCar);
                            context.SaveChanges();
                        }
                    }
                }
            }

            //Car[] cars = importCars.Select(c => new Car()
            //{
            //    Make = c.Make,
            //    Model = c.Model,
            //    TravelledDistance = c.TraveledDistance
            //})
            //.ToArray();

            //context.Cars.AddRange(cars);

            return $"Successfully imported {importCars.Length}.";
        }
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = ProvideMapper();
            List<ImportCustomerDto> customersDtoColl = JsonConvert.DeserializeObject<List<ImportCustomerDto>>(inputJson)!;

            Customer[] customers = customersDtoColl.Select( c => mapper.Map<Customer>(c)).ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = ProvideMapper();

            ImportSaleDto[] salesDto = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson)!;
            Sale[] sales = salesDto.Select(s => mapper.Map<Sale>(s)).ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }
        #endregion

        #region Data Export
        //public static string GetOrderedCustomers(CarDealerContext context)
        //{

        //}
        #endregion
    }
}