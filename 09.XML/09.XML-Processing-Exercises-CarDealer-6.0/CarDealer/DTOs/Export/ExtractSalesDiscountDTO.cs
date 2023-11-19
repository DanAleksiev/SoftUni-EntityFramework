using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
    {
    [XmlType("sale")]
    public class ExtractSalesDiscountDTO
        {
        [XmlElement("car")]
        public CarInfo CarInfo { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public double DiscountedPrice { get; set; }
        }
    

    [XmlType("car")]
    public class CarInfo
        {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }
        }
    }
