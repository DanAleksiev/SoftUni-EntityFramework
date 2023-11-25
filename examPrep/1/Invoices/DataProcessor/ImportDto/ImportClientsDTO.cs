using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
    {
    [XmlType("Client")]
    public class ImportClientsDTO
        {
        [MaxLength(25)]
        [MinLength(10)]
        public string Name { get; set; }
        [MaxLength(15)]
        [MinLength(10)]
        public string NumberVat { get; set; }

        public AllAddresses[] Addresses { get; set; }
        }
    [XmlType("Address")]
    public class AllAddresses
        {
        [MaxLength(20)]
        [MinLength(10)]
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string PostCode { get; set; }
        [MaxLength(15)]
        [MinLength(5)]
        public string City { get; set; }
        [MaxLength(15)]
        [MinLength(5)]
        public string Country { get; set; }

        }
    }
