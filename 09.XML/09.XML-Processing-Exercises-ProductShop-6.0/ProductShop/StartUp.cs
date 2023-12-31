﻿using AutoMapper;
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
            //Console.WriteLine(GetProductsInRange(context));

            //6
            //Console.WriteLine(GetSoldProducts(context));

            //7
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //8
            Console.WriteLine(GetUsersWithProducts(context));

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
            var map = CreateMapper();
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
                .Take(10)
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new ExportProductsDTO()
                    {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = $"{x.Buyer.FirstName} {x.Buyer.LastName}"
                    })
                .ToArray();

            var xml = new XmlFormating();

            return xml.Serialize<ExportProductsDTO[]>(result, "Products");
            }
        public static string GetSoldProducts(ProductShopContext context)
            {
            var result = context.Users
                .Where(x => x.ProductsSold.Count > 1)
                .Select(x => new ExportSoldProductsDTO()
                    {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold
                        .Select(x => new ProductsDTO()
                            {
                            Name = x.Name,
                            Price = x.Price,
                            })
                        .ToArray()
                    })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToArray();

            var xml = new XmlFormating();

            return xml.Serialize<ExportSoldProductsDTO[]>(result, "Users");
            }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
            {
            var result = context.Categories
                .Select(x => new ExportCategoryProductDTO()
                    {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count,
                    AvaragePrice = x.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)
                    })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            var xml = new XmlFormating();

            return xml.Serialize<ExportCategoryProductDTO[]>(result, "Categories");

            }

        public static string GetUsersWithProducts(ProductShopContext context)
            {
            //give up and copy it :(
            var usersInfo = context
                    .Users
                    .Where(u => u.ProductsSold.Any())
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .Select(u => new UsersArr()
                        {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        ProductsSoldSection = new ProductsSoldDTO()
                            {
                            Count = u.ProductsSold.Count,
                            Products = u.ProductsSold.Select(p => new ProductsArray()
                                {
                                Name = p.Name,
                                Price = p.Price
                                })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                            }
                        })
                    .Take(10)
                    .ToArray();

            ExportProductsSoldByUsersDTO exportUserCountDto = new ExportProductsSoldByUsersDTO()
                {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                AllUsers = usersInfo
                };

            var xmlParser = new XmlFormating();
            return xmlParser.Serialize<ExportProductsSoldByUsersDTO>(exportUserCountDto, "Users");
            }
        }
    }