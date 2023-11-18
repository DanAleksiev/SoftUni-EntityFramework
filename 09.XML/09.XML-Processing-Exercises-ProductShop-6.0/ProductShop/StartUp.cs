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

            //1
            //Console.WriteLine(ImportUsers(context, userxml));

            //2
            Console.WriteLine(ImportProducts(context, productsJson));

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

            // 4.
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
            }

        public static string ImportProducts(ProductShopContext context, string inputXml)
            {
            // 1. Create xml serializer
            var result = new XmlSerializer(typeof(ImportProductDTO[]), new XmlRootAttribute("Products"));


            // 2.
            using var reader = new StringReader(inputXml);
            ImportProductDTO[] importDTO = (ImportProductDTO[])result.Deserialize(reader);

            // 3.
            var map = CreateMapper();
            Product[] products = map.Map<Product[]>(importDTO);

            // 4.
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
            }
        }
}