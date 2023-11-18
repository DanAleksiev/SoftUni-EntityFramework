using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
            {
            ProductShopContext context = new ProductShopContext();

            string categoriesJson = File.ReadAllText("../../../Datasets/categories.xml");
            string categoriesProductsJson = File.ReadAllText("../../../Datasets/categories-products.xml");
            string productsJson = File.ReadAllText("../../../Datasets/products.xml");
            string userxml = File.ReadAllText("../../../Datasets/users.xml");

            // 01
            Console.WriteLine(ImportUsers(context, userxml));
            }
        public static IMapper CreateMapper()
            {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ProductShopProfile>();
            });



            IMapper mapper = configuration.CreateMapper();

            return mapper;
            }

        public static string ImportUsers(ProductShopContext context, string inputXml)
            {
            // 1. Create xml serializer
            var result = new XmlSerializer(typeof(ImportUsersDTO[]), new XmlRootAttribute("Users"));


            // 2.
            using var reader = new StringReader(inputXml);
            ImportUsersDTO[] importUsersDTO = (ImportUsersDTO[])result.Deserialize(reader);

            // 3.
            var map =  CreateMapper();
            User[] users = map.Map<User[]>(importUsersDTO);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
            }
        }
}