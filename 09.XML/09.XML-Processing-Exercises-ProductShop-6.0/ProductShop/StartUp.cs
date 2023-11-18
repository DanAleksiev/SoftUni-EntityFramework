using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Xml;
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
            //Console.WriteLine(ImportProducts(context, productsJson));

            //3
            //Console.WriteLine(ImportCategories(context, categoriesJson));

            //4
            //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsJson));

            //5
            Console.WriteLine(GetProductsInRange(context));

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

            // 2.read the xml
            using var reader = new StringReader(inputXml);
            ImportUsersDTO[] importUsersDTO = (ImportUsersDTO[])result.Deserialize(reader);

            // 3.map the object
            var map =  CreateMapper();
            User[] users = map.Map<User[]>(importUsersDTO);

            // 4. commit to db
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
            }

        public static string ImportProducts(ProductShopContext context, string inputXml)
            {
            var result = new XmlSerializer(typeof(ImportProductDTO[]), new XmlRootAttribute("Products"));

            using var reader = new StringReader(inputXml);
            ImportProductDTO[] importDTO = (ImportProductDTO[])result.Deserialize(reader);

            var map = CreateMapper();
            Product[] products = map.Map<Product[]>(importDTO);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
            }

        public static string ImportCategories(ProductShopContext context, string inputXml)
            {
            var result = new XmlSerializer(typeof(ImportCategoriesDTO[]), new XmlRootAttribute("Categories"));

            using var reader = new StringReader(inputXml);
            ImportCategoriesDTO[] importDTO = (ImportCategoriesDTO[])result.Deserialize(reader);

            var map = CreateMapper();
            Category[] cat = map.Map<Category[]>(importDTO);

            context.Categories.AddRange(cat);
            context.SaveChanges();

            return $"Successfully imported {cat.Length}";
            }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
            {
            var result = new XmlSerializer(typeof(ImportCategoryProductDTO[]), new XmlRootAttribute("CategoryProducts"));

            using var reader = new StringReader(inputXml);
            ImportCategoryProductDTO[] importDTO = (ImportCategoryProductDTO[])result.Deserialize(reader);

            var map = CreateMapper();
            CategoryProduct[] output = map.Map<CategoryProduct[]>(importDTO);

            context.CategoryProducts.AddRange(output);
            context.SaveChanges();

            return $"Successfully imported {output.Length}";
            }

        public static string GetProductsInRange(ProductShopContext context)
            {
            var result = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new ExportProductsDTO()
                    {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = $"{x.Buyer.FirstName} {x.Buyer.LastName}"
                    })
                .Take(10)
                .ToArray();

            var xml = new XmlFormating();

            return xml.Serialize<ExportProductsDTO[]>(result, "Products");
            }
        }
}