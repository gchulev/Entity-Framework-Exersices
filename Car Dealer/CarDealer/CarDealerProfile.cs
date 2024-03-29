﻿using System.Globalization;

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
            CreateMap<ImportSupplierDto, Supplier>();

            CreateMap<ImportPartDto, Part>();

            CreateMap<ImportCarDto, Car>()
                .ForMember(d => d.TraveledDistance, opt => opt.MapFrom(src => src.TraveledDistance));

            CreateMap<ImportCustomerDto, Customer>();

            CreateMap<ImportSaleDto, Sale>();

            CreateMap<Customer, ExportCustomerDto>()
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToString("dd/MM/yyyy")));

            CreateMap<Car, ExportCarDto>()
                .ForMember(d => d.TraveledDistance, opt => opt.MapFrom(src => src.TraveledDistance));

            CreateMap<Supplier, ExportSupplierDto>()
                 .ForMember(d => d.PartsCount, opt => opt.MapFrom(src => src.Parts.Count));

            CreateMap<Car, ExportCar2ndDto>();

            CreateMap<Part, ExportPartd2ndDto>();
        }
    }
}
