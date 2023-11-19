using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
    {
    [XmlType("cunstomer")]
    public class ExportCustomersBySalesDTO
        {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }
        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }
        [XmlAttribute("spent-money")]
        public decimal SpendMoney { get; set; }
    }
    }
