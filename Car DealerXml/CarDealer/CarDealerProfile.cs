using AutoMapper;

using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            // Supplier
            CreateMap<ImportSupplierDto, Supplier>();
            CreateMap<Supplier, ExportSupplierDto>()
                .ForMember(d => d.PartsCount, opt => opt.MapFrom(src => src.Parts.Count));

            // Part
            CreateMap<ImportPartDto, Part>();
            CreateMap<ImportPartIdDto, Part>();
            CreateMap<Part, ExportPartDto>();

            // Car
            CreateMap<ImportCarDto, Car>();
            CreateMap<Car, ExportCarDto>();
            CreateMap<Car, ExportBmwCarsDto>();
            CreateMap<Car, ExportCarWithPartsDto>()
                .ForMember(d => d.Parts, opt => opt.MapFrom(src => src.PartsCars.Select(pc => pc.Part).ToArray()));

            // Customer
            CreateMap<ImportCustomerDto, Customer>();

            // Sale
            CreateMap<ImportSaleDto, Sale>();
        }
    }
}
