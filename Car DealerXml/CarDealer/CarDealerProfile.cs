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

            //// Customer
            //CreateMap<ImportCustomerDto, Customer>();
            //CreateMap<Customer, ExportCustomerDto>()
            //    .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.Name))
            //    .ForMember(d => d.BoughtCars, opt => opt.MapFrom(src => src.Sales.Count))
            //    .ForMember(d => d.SpentMoney, opt => opt.MapFrom(src => CalculateSpentMoney(src)));


            // Customer
            //CreateMap<ImportCustomerDto, Customer>();
            //CreateMap<Customer, ExportCustomerDto>()
            //    .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.Name))
            //    .ForMember(d => d.BoughtCars, opt => opt.MapFrom(src => src.Sales.Count))
            //    .ForMember(d => d.SpentMoney, opt => opt.MapFrom(src => src.IsYoungDriver == true
            //        ? src.Sales.SelectMany(s => s.Car.PartsCars.Select(cp => cp.Part.Price)).Sum() - src.Sales.First(s => s.CustomerId == src.Id).Discount
            //        : src.Sales.SelectMany(s => s.Car.PartsCars.Select(cp => cp.Part.Price)).Sum()));


            // Sale
            CreateMap<ImportSaleDto, Sale>();
            CreateMap<Sale, ExportSaleDto>()
                .ForMember(d => d.CustomerName , opt => opt.MapFrom(s => s.Customer.Name))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Car.PartsCars.Sum(pc => pc.Part.Price)))
                .ForMember(d => d.PriceWithDiscount, opt => opt.MapFrom(s => s.Car.PartsCars.Sum(pc => pc.Part.Price) - s.Car.PartsCars.Sum(pc => pc.Part.Price) * s.Discount / 100));
        }


        // This function was created to replace the long lambda expression, in MapFrom method, just for demo and learning purposes (both work)
        private static decimal CalculateSpentMoney(Customer src)
        {
            string name = src.Name;
            bool isYoungDriver = src.IsYoungDriver;
            //decimal discount = src.Sales.First(s => s.CustomerId == src.Id).Discount;
            decimal discount = src.Sales.Sum(s => s.Discount);
            decimal spentMoney = src.Sales.SelectMany(s => s.Car.PartsCars.Select(cp => cp.Part.Price)).Sum();

            if (isYoungDriver)
            {
                spentMoney -= spentMoney * discount / 100;
                return spentMoney;
            }
            return spentMoney;
        }
    }
}
