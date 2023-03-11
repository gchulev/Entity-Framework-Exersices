using AutoMapper;

using Newtonsoft.Json;

using ProductShop.Data;
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
                string inputJson = File.ReadAllText("D:\\Visual Studio\\Projects\\EntityFrameWork Exersices\\Product Shop\\ProductShop\\Datasets\\users.json");
                ImportUsers(context, inputJson);
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
            var mapper = config.CreateMapper();

            ImportUsersDto[] usersToImport = JsonConvert.DeserializeObject<ImportUsersDto[]>(inputJson)!;

            User[] users = usersToImport.Select(u => mapper.Map<User>(u)).ToArray();

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }
    }
}