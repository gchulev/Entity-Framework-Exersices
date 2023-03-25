using System.ComponentModel.DataAnnotations;

using AutoMapper;

using Trucks.Data;

using Trucks.Utilities;
using Trucks.Data.Models;
using Trucks.DataProcessor.ImportDto;
using System.Text;
using Newtonsoft.Json;

namespace Trucks.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            IMapper mapper = CreateMapper();

            ImportDispatcherDto[] dispatchersDto = XmlHelper.Deserialize<ImportDispatcherDto[]>(xmlString, "Despatchers");

            Despatcher[] dispatchers = mapper.Map<Despatcher[]>(dispatchersDto);

            var sb = new StringBuilder();
            var validDispatchers = new List<Despatcher>();
            var allValidTrucks = new List<Truck>();

            foreach (Despatcher dispatcher in dispatchers)
            {
                var validTrucks = new List<Truck>();

                if (IsValid(dispatcher) && !string.IsNullOrWhiteSpace(dispatcher.Name))
                {
                    if (!string.IsNullOrWhiteSpace(dispatcher.Position))
                    {
                        foreach (Truck truck in dispatcher.Trucks)
                        {
                            if (!IsValid(truck) || string.IsNullOrWhiteSpace(truck.VinNumber) || (int)truck.MakeType < 0 || (int)truck.MakeType > 4)
                            {
                                sb.AppendLine(ErrorMessage);
                                continue;
                            }
                            validTrucks.Add(truck);
                            allValidTrucks.Add(truck);
                        }
                        dispatcher.Trucks = validTrucks;
                        validDispatchers.Add(dispatcher);

                        sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, dispatcher.Name, dispatcher.Trucks.Count));
                        continue;
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                }
                sb.AppendLine(ErrorMessage);
            }

            context.Trucks.AddRange(allValidTrucks);
            context.Despatchers.AddRange(validDispatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            ImportClientDto[] clientsDto = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString)!;

            Client[] clients = clientsDto.Select(c => new Client()
            {
                Name = c.Name,
                Nationality = c.Nationality,
                Type = c.Type,
                ClientsTrucks = c.Trucks.Distinct().Select(id => new ClientTruck()
                {
                    TruckId = id
                }).ToArray()
            })
             .ToArray();

            var validClients = new List<Client>();
            var allValidClientTrucks = new List<ClientTruck>();
            var sb = new StringBuilder();

            foreach (var client in clients)
            {
                if (IsValid(client) && client.Type.ToLower() != "usual" && client.Nationality != null && client.Name != null)
                {
                    var validClientTrucks = new List<ClientTruck>();

                    foreach (var clientTruck in client.ClientsTrucks)
                    {
                        if (!context.Trucks.Distinct().Any(t => t.Id == clientTruck.TruckId))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                        validClientTrucks.Add(clientTruck);
                        allValidClientTrucks.Add(clientTruck);
                    }

                    client.ClientsTrucks = validClientTrucks;
                    validClients.Add(client);

                    sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
                    continue;
                }

                sb.AppendLine(ErrorMessage);
            }

            context.Clients.AddRange(validClients);
            context.ClientsTrucks.AddRange(allValidClientTrucks);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TrucksProfile>());
            return config.CreateMapper();
        }
    }
}