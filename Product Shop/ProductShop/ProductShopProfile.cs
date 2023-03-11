using AutoMapper;

using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            // Users mapping
            CreateMap<User, ImportUserDto>();
            CreateMap<ImportUserDto, User>();
            CreateMap<User, ExportUserDto>()
                .ForMember(d => d.SoldProducts, opt => opt.MapFrom(src => src.ProductsSold));

            // Products mapping
            CreateMap<ImportProductDto, Product>();
            CreateMap<Product, ExportProductDto>()
                .ForMember(d => d.Seller, opt => opt.MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}"));

            CreateMap<Product, ExportBoughtProductDto>()
                .ForMember(d => d.BuyerFirstName, opt => opt.MapFrom(src => src.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, opt => opt.MapFrom(src => src.Buyer.LastName));

            // Categories mapping
            CreateMap<ImportCategorieDto, Category>();
            CreateMap<Category, ExportCateogryByProductDto>()
                .ForMember(d => d.Category, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.ProductsCount, opt => opt.MapFrom(src => src.CategoriesProducts.Count))
                .ForMember(d => d.AveragePrice, opt => opt.MapFrom(src => Math.Round(src.CategoriesProducts.Average(p => p.Product.Price), 2)))
                .ForMember(d => d.TotalRevenue, opt => opt.MapFrom(src => src.CategoriesProducts.Sum(p => p.Product.Price)));

            // CategoryProduct mapping
            CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
