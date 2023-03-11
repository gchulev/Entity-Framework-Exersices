using AutoMapper;

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

            // Categories mapping
            CreateMap<ImportCategorieDto, Category>();

            // CategoryProduct mapping
            CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
