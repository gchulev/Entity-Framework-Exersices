using System.Security.Cryptography.X509Certificates;

using AutoMapper;

using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            // Users mapping
            CreateMap<User, ImportUserDto>();
            CreateMap<ImportUserDto, User>();

            // Products mapping
            CreateMap<ImportProductDto, Product>();
            CreateMap<Product, ExportProductDto>()
                .ForMember(d => d.Seller, opt => opt.MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}"));

            // Categories mapping
            CreateMap<ImportCategorieDto, Category>();

            // CategoryProduct mapping
            CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
