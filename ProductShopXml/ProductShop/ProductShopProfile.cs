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
                //.ForMember(d => d.Buyer, opt => opt.MapFrom(src => src.Buyer != null ? $"{src.Buyer.FirstName} {src.Buyer.LastName}" : null))
                .ForMember(d => d.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price.ToString("0.##"))));
            CreateMap<Product[], ExportSoldProductsDto>()
                .ForMember(d => d.Products, opt => opt.MapFrom(src => src))
                .ForMember(d => d.Count, opt => opt.MapFrom(src => src.Length));


            // Category
            CreateMap<ImportCategoryDto, Category>();
            CreateMap<Category, ExportCategoriesDto > ()
                .ForMember(d => d.Count, opt => opt.MapFrom(src => src.CategoryProducts.Select(p => p.Product).Count()))
                .ForMember(d => d.AveragePrice, opt => opt.MapFrom(src => src.CategoryProducts.Select(p => p.Product.Price).Average()))
                .ForMember(d => d.TotalRevenue, opt => opt.MapFrom(src => src.CategoryProducts.Select(p => p.Product.Price).Sum()));

            // CategoryProduct
            CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
