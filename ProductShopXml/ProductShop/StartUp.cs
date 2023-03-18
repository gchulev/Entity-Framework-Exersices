using System.Xml;
using System.Xml.Serialization;

using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using ProductShop.Data;
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
                string inputXmlPath = @"D:\Visual Studio\Projects\EntityFrameWork Exersices\ProductShopXml\ProductShop\Datasets\users.xml";

                Console.WriteLine(ImportUsers(context, inputXmlPath));
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
    }
}