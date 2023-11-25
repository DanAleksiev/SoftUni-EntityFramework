using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ExportDto
    {
    public class ExportProductsWithMostClientsDTO
        {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public CategoryType Category { get; set; }
        public  ClientsForProduct[] Clients { get; set; }
    }

    public class ClientsForProduct
        {
        public string Name { get; set; }
        public string NumberVat { get; set; }
    }
    }
