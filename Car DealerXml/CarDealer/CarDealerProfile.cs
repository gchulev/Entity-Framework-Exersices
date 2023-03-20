﻿using AutoMapper;

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

            // Part
            CreateMap<ImportPartDto, Part>();
            CreateMap<ImportPartIdDto, Part>();

            // Car
            CreateMap<ImportCarDto, Car>();

            // Customer
            CreateMap<ImportCustomerDto, Customer>();

            // Sale
            CreateMap<ImportSaleDto, Sale>();
        }
    }
}
