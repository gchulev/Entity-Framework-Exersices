using AutoMapper;
using AutoMapper.QueryableExtensions;

using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

using Castle.Core.Resource;

using ProductShop.Utilities;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using (var context = new CarDealerContext())
            {
                string filePath = @"D:\Visual Studio\Projects\EntityFrameWork Exersices\Car DealerXml\CarDealer\Datasets\sales.xml";
                string inputXml = File.ReadAllText(filePath);

                //Console.WriteLine(ImportSuppliers(context, inputXml));
                //Console.WriteLine(ImportParts(context, inputXml));
                //Console.WriteLine(ImportCars(context, inputXml));
                //Console.WriteLine(ImportCustomers(context, inputXml));
                //Console.WriteLine(ImportSales(context, inputXml));

                //Console.WriteLine(GetCarsWithDistance(context));
                Console.WriteLine(GetCarsFromMakeBmw(context));

            }

        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

            IMapper mapper = config.CreateMapper();

            return mapper;
        }

        #region Imports
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
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            ImportCarDto[] importCarsDto = XmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");

            ICollection<Car> cars = new HashSet<Car>();

            foreach (var importCarDto in importCarsDto)
            {
                Car car = mapper.Map<Car>(importCarDto);

                foreach (var importPartDto in importCarDto.Parts.DistinctBy(p => p.PartId))
                {
                    if (importPartDto.PartId != null || context.Parts.Any(p => p.Id == importPartDto.PartId))
                    {
                        PartCar parCar = new PartCar()
                        {
                            PartId = importPartDto!.PartId,
                        };

                        car.PartsCars.Add(parCar);
                    }

                    
                }
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            ImportCustomerDto[] importCustomerDto = XmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

            Customer[] customers = importCustomerDto.Select(c => mapper.Map<Customer>(c)).ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            ImportSaleDto[] importSales = XmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

            Sale[] sales = importSales.Where(s => context.Cars.Any(c => c.Id == s.CarId)).Select(s => mapper.Map<Sale>(s)).ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }
        #endregion

        #region Exports
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            ExportCarDto[] cars = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarDto>(mapper.ConfigurationProvider)
                .ToArray();

            string xmlResult = XmlHelper.Serialize(cars, "cars");
            return xmlResult;
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            ExportBmwCarsDto[] cars = context.Cars
                .Where(c => c.Make.ToUpper() == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportBmwCarsDto>(mapper.ConfigurationProvider)
                .ToArray();

            string xmlResult = XmlHelper.Serialize(cars, "cars");

            return xmlResult;
        }
        #endregion
    }
}