using System.Runtime.CompilerServices;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Castle.DynamicProxy;

using Microsoft.EntityFrameworkCore.Query.Internal;

using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using (var context = new ProductShopContext())
            {
                string inputXmlPath = @"D:\Visual Studio\Projects\EntityFrameWork Exersices\ProductShopXml\ProductShop\Datasets\categories-products.xml";
                string inputXml = File.ReadAllText(inputXmlPath);

                //Console.WriteLine(ImportUsers(context, inputXmlPath));
                //Console.WriteLine(ImportProducts(context, inputXmlPath));
                //Console.WriteLine(ImportCategories(context, inputXml));
                //Console.WriteLine(ImportCategoryProducts(context, inputXml));

                //Console.WriteLine(GetProductsInRange(context));
                //Console.WriteLine(GetSoldProducts(context));
                //Console.WriteLine(GetCategoriesByProductsCount(context));
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

            return config.CreateMapper();
        }
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            using (StreamReader stream = new StreamReader(inputXml))
            {

                ImportUserDto[] users = XmlHelper.Deserialize<ImportUserDto[]>(inputXml, "Users");

                User[] importUsers = users.Select(u => mapper.Map<User>(u)).ToArray();

                context.Users.AddRange(importUsers);
                context.SaveChanges();

                return $"Successfully imported {users.Length}";
            }

        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            ImportProductDto[] importProducts = XmlHelper.Deserialize<ImportProductDto[]>(inputXml, "Products");

            Product[] products = importProducts.Select(ip => mapper.Map<Product>(ip)).ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryDto[] importCategories = XmlHelper.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

            Category[] categories = importCategories.Select(ic => mapper.Map<Category>(ic)).ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryProductDto[] categoriesProductsDtoList = XmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            CategoryProduct[] categoryProducts = categoriesProductsDtoList.Where(cp => cp.ProductId != null && cp.CategoryId != null)
                .Select(cp => mapper.Map<CategoryProduct>(cp)).ToArray();

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            ExportProductDto[] products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ExportProductDto>(mapper.ConfigurationProvider)
                .ToArray();

            string serializedXml = XmlHelper.Serialize(products, "Products");

            return serializedXml;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            ExportUserDto[] usersArray = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<ExportUserDto>(mapper.ConfigurationProvider)
                .ToArray();

            string usersToXml = XmlHelper.Serialize(usersArray, "Users");

            return usersToXml;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            ExportCategoriesDto[] categoriesDto = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Select(cp => cp.Product).Count())
                .ThenBy(c => c.CategoryProducts.Select(cp => cp.Product.Price).Sum())
                .ProjectTo<ExportCategoriesDto>(mapper.ConfigurationProvider)
                .ToArray();

            string categoriesToXml = XmlHelper.Serialize(categoriesDto, "Categories");

            return categoriesToXml;
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            ExportUsersDto users = new ExportUsersDto() 
            {
                Count = context.Users.Count(u => u.ProductsSold.Count > 0),
                Users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Take(10)
                .Select(u => new ExportUserDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportSoldProductsDto()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.OrderByDescending(p => p.Price).Select(p => mapper.Map<ExportProductDto>(p)).ToArray()
                    }
                })
                .ToArray()
            };

            string exportUsersXml = XmlHelper.Serialize(users, "Users");

            return exportUsersXml;
        }
    }
}