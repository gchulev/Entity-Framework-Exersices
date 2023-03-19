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
            // User
            CreateMap<ImportUserDto, User>();
            CreateMap<User, ExportUserDto>()
                .ForMember(d => d.SoldProducts, opt => opt.MapFrom(src => src.ProductsSold));

            // Product
            CreateMap<ImportProductDto, Product>();
            CreateMap<Product, ExportProductDto>()
                .ForMember(d => d.Buyer, opt => opt.MapFrom(src => src.Buyer != null ? $"{src.Buyer.FirstName} {src.Buyer.LastName}" : null))
                .ForMember(d => d.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price.ToString("0.##"))));
            

            // Category
            CreateMap<ImportCategoryDto, Category>();

            // CategoryProduct
            CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
