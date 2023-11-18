using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
    {
    [XmlType("User")]
    public class ExportSoldProductsDTO
        {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public ProductsDTO[] SoldProducts { get; set; }
        }

    [XmlType("Product")]
    public class ProductsDTO
        {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        }
    }
