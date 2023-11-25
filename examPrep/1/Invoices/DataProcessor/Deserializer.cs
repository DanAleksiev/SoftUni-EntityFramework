namespace Invoices.DataProcessor
    {
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using AutoMapper;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Extentions;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var serialiser = new XmlFormating();

            ImportClientsDTO[] dto = serialiser.Deserialize<ImportClientsDTO[]>(xmlString, "Clients");
            StringBuilder sb = new StringBuilder();

            List<Client> clients = new List<Client>();
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


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            List<ImportInvoicesDTO> dto = jsonString.DeserializeFromJson<List<ImportInvoicesDTO>>();
            StringBuilder sb = new StringBuilder();

            List<Invoice> invoices = new List<Invoice>();
            foreach (var invoice in dto)
                {
                if (!IsValid(invoice) || invoice.IssueDate > invoice.DueDate)
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }


                var newInvoice = new Invoice
                    {
                    Number = invoice.Number,
                    IssueDate = invoice.IssueDate,
                    DueDate = invoice.DueDate,
                    Amount  = invoice.Amount,
                    CurrencyType = invoice.CurrencyType,
                    ClientId = invoice.ClientId,
                    };

                    sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
                    invoices.Add(newInvoice);

                }
            context.Invoices.AddRange(invoices);
            context.SaveChanges();
            return sb.ToString().Trim();
            }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            List<ImportProductsDTO> dto = jsonString.DeserializeFromJson<List<ImportProductsDTO>>();
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

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
