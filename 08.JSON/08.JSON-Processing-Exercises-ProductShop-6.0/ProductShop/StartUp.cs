using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
    {
    public class StartUp
        {
        public static void Main()
            {
            ProductShopContext context = new ProductShopContext();


            string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            string categoriesProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            string productsJson = File.ReadAllText("../../../Datasets/products.json");
            string userJson = File.ReadAllText("../../../Datasets/users.json");


            // 01
            //Console.WriteLine(ImportUsers(context, userJson));

            // 02
            //Console.WriteLine(ImportProducts(context, productsJson));

            // 03
            //Console.WriteLine(ImportCategories(context, categoriesJson));

            // 04
            //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsJson));

            // 05
            //Console.WriteLine(GetProductsInRange(context));

            // 06
            //Console.WriteLine(GetSoldProducts(context));

            // 07
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //08
            Console.WriteLine(GetUsersWithProducts(context));

            }

        public static string ImportUsers(ProductShopContext context, string inputJson)
            {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}";
            }

        public static string ImportProducts(ProductShopContext context, string inputJson)
            {
            var product = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(product);
            context.SaveChanges();
            return $"Successfully imported {product.Length}";
            }

        public static string ImportCategories(ProductShopContext context, string inputJson)
            {
            var categories = JsonConvert
                .DeserializeObject<Category[]>(inputJson).Where(c => c.Name != null);

            var valid = categories.Where(c => c.Name is not null).ToArray();

            if (valid is not null)
                {
                context.Categories.AddRange(valid);
                context.SaveChanges();
                return $"Successfully imported {valid.Count()}";
                }
            return "none valid";
            }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
            {
            var pc = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoriesProducts.AddRange(pc);
            context.SaveChanges();
            return $"Successfully imported {pc.Length}";
            }

        public static string GetProductsInRange(ProductShopContext context)
            {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                    {
                    name = p.Name,
                    price = p.Price,
                    seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                    });

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);

            File.WriteAllText(@"../../../products-in-range.json", json);

            return json.Trim();
            }

        public static string GetSoldProducts(ProductShopContext context)
            {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                    {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Where(p => p.BuyerId != null)
                    .Select(p => new
                        {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer!.FirstName,
                        buyerLastName = p.Buyer.LastName
                        }).ToArray()
                    })
                .OrderBy(p => p.lastName)
                .ThenBy(p => p.firstName)
                .ToArray();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);

            File.WriteAllText(@"../../../users-sold-products.json", json);

            return json.Trim();
            }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
            {
            //var cat = context.Categories
            //    .Select(c => new
            //        {
            //        category = c.Name,
            //        productsCount = c.CategoriesProducts.Count(),
            //        avaragePrice = $"{c.CategoriesProducts.Average(cp => cp.Product.Price):f2}",
            //        totalRevenue = $"{c.CategoriesProducts.Sum(cp => cp.Product.Price):f2}",
            //        })
            //    .OrderByDescending(p => p.productsCount)
            //    .ToArray();

            //var json = JsonConvert.SerializeObject(cat, Formatting.Indented);

            //File.WriteAllText(@"../../../categories-by-products.json", json);
            //return json.Trim();


            var categoriesByProductCount = context.Categories
               .Select(c => new
                   {
                   category = c.Name,
                   productsCount = c.CategoriesProducts.Count,
                   averagePrice = c.CategoriesProducts
                       .Average(cp => cp.Product.Price).ToString("f2"),
                   totalRevenue = c.CategoriesProducts
                       .Sum(cp => cp.Product.Price).ToString("f2")
                   })
               .OrderByDescending(x => x.productsCount)
               .ToArray();

            return JsonConvert.SerializeObject(categoriesByProductCount, Formatting.Indented);

            }

        public static string GetUsersWithProducts(ProductShopContext context)
            {
            var usersWithProduct = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                    {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                            {
                            name = p.Name,
                            price = p.Price
                            })
                        .ToArray()
                    })
                .OrderByDescending(u => u.soldProducts.Count())
                .ToArray();


            var output = new
                {
                usersCount = usersWithProduct.Count(),
                users = usersWithProduct.Select(u => new
                    {
                    u.firstName,
                    u.lastName,
                    u.age,
                    soldProducts = new
                        {
                        count = u.soldProducts.Count(),
                        products = u.soldProducts
                        }
                    })
                };

            string jsonOutput = JsonConvert.SerializeObject(output, new JsonSerializerSettings
                {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
                });

            return jsonOutput;
            }
        }
    }