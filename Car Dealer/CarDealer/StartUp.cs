using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

using AutoMapper;

using CarDealer.Data;
using CarDealer.DTOs.Export;
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

                //Console.WriteLine(GetOrderedCustomers(context));
                //Console.WriteLine(GetCarsFromMakeToyota(context));
                //Console.WriteLine(GetLocalSuppliers(context));
                Console.WriteLine(GetCarsWithTheirListOfParts(context));
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
                    TraveledDistance = importCar.TraveledDistance
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

            Customer[] customers = customersDtoColl.Select(c => mapper.Map<Customer>(c)).ToArray();

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
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            IMapper mapper = ProvideMapper();

            Customer[] customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ToArray();

            ExportCustomerDto[] customersDto = customers.Select(c => mapper.Map<ExportCustomerDto>(c)).ToArray();

            string customersJson = JsonConvert.SerializeObject(customersDto, Formatting.Indented);

            return customersJson;
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            IMapper mapper = ProvideMapper();

            Car[] cars = context.Cars.
                Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToArray();

            ExportCarDto[] carsDto = cars.Select(c => mapper.Map<ExportCarDto>(c)).ToArray();

            string jsonResult = JsonConvert.SerializeObject(carsDto, Formatting.Indented);

            return jsonResult;
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = ProvideMapper();

            Supplier[] suppliers = context.Suppliers
                .AsNoTracking()
                .Include(s => s.Parts)
                .Where(s => s.IsImporter == false)
                .ToArray();

            ExportSupplierDto[] suppliersDto = suppliers.Select(s => mapper.Map<ExportSupplierDto>(s)).ToArray();

            string jsonResult = JsonConvert.SerializeObject(suppliersDto, Formatting.Indented);

            return jsonResult;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = ProvideMapper();

            var cars = context.Cars
                .Include(c => c.PartsCars)
                .Select(c => new
                {
                    car = new ExportCar2ndDto()
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance,
                    },
                    parts = c.PartsCars.Select(pc => new ExportPartd2ndDto()
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price.ToString(".00")
                    })
                })
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return jsonResult;
        }
        #endregion
    }
}