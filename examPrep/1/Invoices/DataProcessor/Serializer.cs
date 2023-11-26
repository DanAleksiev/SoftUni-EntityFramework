namespace Invoices.DataProcessor
    {
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Extentions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;

    public class Serializer
        {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
            {
            var result = context.Clients
                .Where(c => c.Invoices.Any(c => c.IssueDate > date))
                .ToArray()
                .Select(c => new ExportClientsWithTheirInvoicesDTO()
                    {
                    InvoicesCount = c.Invoices.Count(),
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                    .OrderBy(i => i.IssueDate)
                    .ThenByDescending(i => i.DueDate)
                    .Select(i => new AllInvoices()
                        {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        //DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = i.DueDate.ToString("MM/dd/yyyy"),
                        Currency = i.CurrencyType.ToString(),
                        })

                    .ToArray()
                    })
                .OrderByDescending(c => c.Invoices.Count())
                .ThenBy(c => c.ClientName)
                .ToArray();

            XmlFormating xmlFormater = new XmlFormating();
            return xmlFormater.Serialize<ExportClientsWithTheirInvoicesDTO[]>(result, "Clients");
            }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
            {
            var products = context.Products
                .Where(p => p.ProductsClients.Any(p => p.Client.Name.Length > nameLength))
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