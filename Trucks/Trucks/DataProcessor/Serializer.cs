using AutoMapper;

using Trucks.Data;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Trucks.Utilities;

using Trucks.Data.Models;
using Trucks.Data.Models.Enums;
using Trucks.DataProcessor.ExportDto;

namespace Trucks.DataProcessor
{
    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            IMapper mapper = CreateMapper();

            ExportDispatcherDto[] dispachers = context.Despatchers
                .Where(d => d.Trucks.Count > 0)
                .Select(d => new ExportDispatcherDto()
                {
                    DespatcherName = d.Name,
                    TrucksCount = d.Trucks.Count,
                    Trucks = d.Trucks.Select(t => new ExportDispatcherTruckDto()
                    {
                        MakeType = t.MakeType,
                        RegistrationNumber = t.RegistrationNumber
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                })
                .OrderByDescending(d => d.Trucks.Length)
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            string xmlResult = XmlHelper.Serialize(dispachers, "Despatchers");
            //Console.WriteLine("###########");
            //Console.WriteLine();
            //Console.WriteLine(xmlResult);


            return xmlResult;
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            IMapper mapper = CreateMapper();

            //    var clients = context.Clients
            //        .AsNoTracking()
            //        .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
            //        .Select(c => new Client()
            //        {
            //            Name = c.Name,
            //            ClientsTrucks = c.ClientsTrucks
            //            .Where(t => t.Truck.TankCapacity >= capacity)
            //            .Select(t => new ClientTruck()
            //            {
            //                Truck = new Truck()
            //                {
            //                    RegistrationNumber = t.Truck.RegistrationNumber,
            //                    VinNumber = t.Truck.VinNumber,
            //                    TankCapacity = t.Truck.TankCapacity,
            //                    CargoCapacity = t.Truck.CargoCapacity,
            //                    CategoryType = t.Truck.CategoryType,
            //                    MakeType = t.Truck.MakeType
            //                },
            //                Client = t.Client,
            //                ClientId = t.ClientId,
            //                TruckId = t.TruckId
            //            })
            //            .OrderBy(ct => ct.Truck.MakeType)
            //            .ThenByDescending(ct => ct.Truck.CargoCapacity)
            //            .ToList()
            //        })
            //        .OrderByDescending(c => c.ClientsTrucks.Count(t => t.Truck.TankCapacity >= capacity))
            //        .ThenBy(c => c.Name)
            //        .Take(10)
            //        .ToArray();

            //ExportClientDto[] finalResult = mapper.Map<ExportClientDto[]>(clients);

            ExportClientDto[] clients = context.Clients
                .AsNoTracking()
                .Where(c => c.ClientsTrucks.Count > 0 && c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
                .Select(c => new ExportClientDto()
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                        .Where(ct => ct.Truck.TankCapacity >= capacity)
                        .Select(ct => new ExportTruckDto()
                        {
                            CargoCapacity = ct.Truck.CargoCapacity,
                            CategoryType = ct.Truck.CategoryType,
                            MakeType = ct.Truck.MakeType,
                            TankCapacity = ct.Truck.TankCapacity,
                            TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                            VinNumber = ct.Truck.VinNumber
                        })
                        .OrderBy(t => t.MakeType)
                        .ThenByDescending(t => t.CargoCapacity)
                        .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Length)
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(clients, Formatting.Indented);
            //Console.WriteLine(jsonResult);
            return jsonResult;
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TrucksProfile>());
            return config.CreateMapper();
        }
    }
}
