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
                Console.WriteLine(GetSoldProducts(context));
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

            context.AddRange(categoryProducts);
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
    }
}