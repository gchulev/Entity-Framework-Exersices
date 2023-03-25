namespace Trucks
{
    using Trucks.DataProcessor.ImportDto;
    //using Trucks.DataProcessor.ExportDto;
    using Trucks.Data.Models;

    using AutoMapper;
    using Trucks.DataProcessor.ExportDto;

    public class TrucksProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public TrucksProfile()
        {
            // Truck
            CreateMap<ImportTruckDto, Truck>();
            CreateMap<Truck, ExportTruckDto>()
                .ForMember(d => d.TruckRegistrationNumber, opt => opt.MapFrom(s => s.RegistrationNumber));
            CreateMap<Truck, ExportDispatcherTruckDto>();
                

            // Dispatcher
            CreateMap<ImportDispatcherDto, Despatcher>();
            CreateMap<Despatcher, ExportDispatcherDto>()
                .ForMember(d => d.DespatcherName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.TrucksCount, opt => opt.MapFrom(s => s.Trucks.Count))
                .ForMember(d => d.Trucks, opt => opt.MapFrom(s => s.Trucks));


            // Client
            CreateMap<ImportClientDto, Client>();
            CreateMap<Client, ExportClientDto>()
                .ForMember(d => d.Trucks, opt => opt.MapFrom(s => s.ClientsTrucks.Select(t => t.Truck)));
        }
    }
}
