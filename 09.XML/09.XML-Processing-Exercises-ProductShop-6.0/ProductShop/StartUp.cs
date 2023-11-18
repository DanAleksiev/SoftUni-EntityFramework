using ProductShop.Data;
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
            string userJson = File.ReadAllText("../../../Datasets/users.xml");


            Console.WriteLine(ImportUsers(context, userJson));
            }

        public static string ImportUsers(ProductShopContext context, string inputXml)
            {
            //var doc = new XmlSerializer(typeof(User));
            //string des = doc.Deserialize(inputXml).ToString();

            var doc = new XDocument();
            doc = XDocument.Parse(inputXml);

            List<User> users = new List<User>();

            foreach (var d in doc.Root.Elements())
            {
                User user = new User();
                users.Add(user);
                
                }
            context.Users.AddRange();
            //XmlSerializer xmlSerializer = new XmlSerializer();
            return $"Successfully imported {users.Length}";
            }
        }
}