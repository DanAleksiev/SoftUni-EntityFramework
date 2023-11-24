using Invoices.Data.Models;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
    {
    [XmlType("client")]
    public class ImportClientsDTO
        {
        [XmlElement("Name")]
        public string? Name { get; set; }
        [XmlElement("NumberVat")]
        public string? NumberVat { get; set; }

        [XmlArray("Addresses")]
        public AllAddresses[]? AllAddresses { get; set; }
        }
    [XmlType("Address")]
    public class AllAddresses
        {
        [XmlElement("StreetName")]
        public string? StreetName { get; set; }
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }
        [XmlElement("PostCode")]
        public string? PostCode { get; set; }
        [XmlElement("City")]
        public string? City { get; set; }
        [XmlElement("Country")]
        public string? Country { get; set; }

        }
    }
