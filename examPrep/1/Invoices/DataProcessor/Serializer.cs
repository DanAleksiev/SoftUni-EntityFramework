namespace Invoices.DataProcessor
    {
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Extentions;
    using Microsoft.EntityFrameworkCore;

    public class Serializer
        {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
            {
            throw new NotImplementedException();
            }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
            {
            var products = context.Products
                .Where(p => p.ProductsClients.Any() && p.Name.Length >= nameLength)
                .Include(p => p.ProductsClients)
                .ThenInclude(pc => pc.Client)
                .Select(p => new ExportProductsWithMostClientsDTO
                    {
                    Name = p.Name,
                    Price = decimal.Parse(p.Price.ToString("0.##")),
                    Category = p.CategoryType,
                    Clients = p.ProductsClients
                        .Where(p => p.Client.Name.Length >= nameLength)
                        .Select(pc => new ClientsForProduct()

                        {
                        Name = pc.Client.Name,
                        NumberVat = pc.Client.NumberVat
                        })
                    .OrderBy(pc => pc.Name)
                    .ToArray(),
                    })
                .OrderByDescending(p => p.Clients.Count())
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            return products.SerializeToJson();
            }
        }
    }