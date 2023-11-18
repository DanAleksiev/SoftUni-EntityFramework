using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
    {
    [XmlType("Users")]
    public class ExportProductsSoldByUsersDTO
        {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UsersArr[] AllUsers { get; set; }
        }

    [XmlType("User")]
    public class UsersArr
        {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        public ProductsSoldDTO ProductsSoldSection { get; set; }
        }

    [XmlType("SoldProducts")]
    public class ProductsSoldDTO
        {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ProductsArray[] Products { get; set; }
        }

    [XmlType("Product")]
    public class ProductsArray
        {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
        }

    }
