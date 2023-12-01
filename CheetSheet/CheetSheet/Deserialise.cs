using Invoices.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheetSheet
    {
    internal class Deserialise
        {
        public static string StructureDeserialiseJson(InvoicesContext context, string jsonString)
            {
            List<DTO> dto = jsonString.DeserializeFromJson<List<DTO>>();
            StringBuilder sb = new StringBuilder();

            List<Product> products = new List<Product>();

            var clients = context.Clients.Select(p => p.Id).ToList();

            foreach (var product in dto)
                {
                if (!IsValid(product))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }


                var newProduct = new Product
                    {
                    Name = product.Name,
                    Price = product.Price,
                    CategoryType = product.CategoryType,
                    };

                foreach (var c in product.Clients.Distinct())
                    {
                    if (clients.Contains(c))
                        {
                        newProduct.ProductsClients.Add(new ProductClient()
                            {
                            ClientId = c,
                            });
                        }
                    else
                        {
                        sb.AppendLine(ErrorMessage);
                        }
                    }

                sb.AppendLine(string.Format(SuccessfullyImportedProducts, newProduct.Name, newProduct.ProductsClients.Count()));
                products.Add(newProduct);

                }
            context.Products.AddRange(products);
            context.SaveChanges();
            return sb.ToString().Trim();
            }


        public static string StructureDeserialiseXML(InvoicesContext context, string xmlString)
            {
            var serialiser = new XmlFormating();

            DTO[] dto = serialiser.Deserialize<DTO[]>(xmlString, "Whatever");
            StringBuilder sb = new StringBuilder();

            List<Product> clients = new List<Product>();
            foreach (var client in dto)
                {
                if (!IsValid(client))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }


                var newClient = new Client
                    {
                    Name = client.Name,
                    NumberVat = client.NumberVat,
                    };


                foreach (var address in client.Addresses.Distinct())
                    {
                    if (IsValid(address))
                        {
                        newClient.Addresses.Add(new Address()
                            {
                            StreetNumber = address.StreetNumber,
                            StreetName = address.StreetName,
                            PostCode = address.PostCode,
                            Country = address.Country,
                            City = address.City,
                            });
                        }
                    else
                        {
                        sb.AppendLine(ErrorMessage);
                        }

                    sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
                    clients.Add(newClient);
                    }

                }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return sb.ToString().Trim();
            }
        }
    }
