using AutoMapper;
using AutoMapper.QueryableExtensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            
            using (var context = new ProductShopContext())
            {
                string inputJson = File.ReadAllText("D:\\Visual Studio\\Projects\\EntityFrameWork Exersices\\Product Shop\\ProductShop\\Datasets\\categories-products.json");
                //ImportUsers(context, inputJson);
                //ImportProducts(context, inputJson);
                //ImportCategories(context, inputJson);
                //ImportCategoryProducts(context, inputJson);

                Console.WriteLine(GetProductsInRange(context));
            }
        }
        private static IMapper ProvideMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
            var mapper = config.CreateMapper();

            return mapper;
        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            var mapper = ProvideMapper();

            ImportUserDto[] usersToImport = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson)!;

            User[] users = usersToImport.Select(u => mapper.Map<User>(u)).ToArray();

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var mapper = ProvideMapper();

            Product[] products = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson)!.Select(p => mapper.Map<Product>(p)).ToArray();

            context.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var mapper = ProvideMapper();

            ImportCategorieDto[] categoriesForImport = JsonConvert.DeserializeObject<ImportCategorieDto[]>(inputJson)!
                .Where(c => c.Name != null).ToArray();

            Category[] categories = categoriesForImport.Select(c => mapper.Map<Category>(c)).ToArray();

            context.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var mapper = ProvideMapper();

            ImportCategoryProductDto[] categoriesProductsList = JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson)!;

            CategoryProduct[] categoryProducts = categoriesProductsList.Select(cp => mapper.Map<CategoryProduct>(cp)).ToArray();

            context.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var mapper = ProvideMapper();

            ExportProductDto[] productsToExport = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ExportProductDto>(mapper.ConfigurationProvider)
                .ToArray();

            string jsonExport = JsonConvert.SerializeObject(productsToExport, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });

            return jsonExport;
        }
    }
}