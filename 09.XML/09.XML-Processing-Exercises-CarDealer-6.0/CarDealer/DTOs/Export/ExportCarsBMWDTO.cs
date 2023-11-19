
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
    {
    [XmlType("car")]
    public class ExportCarsBMWDTO
        {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("model")]
        public string Model { get; set; }
        [XmlAttribute("traveled-distance")]
        public string TraveledDistance { get; set; }
    }
    }
