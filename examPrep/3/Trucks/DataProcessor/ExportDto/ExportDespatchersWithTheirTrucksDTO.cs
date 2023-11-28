using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto
    {
    [XmlType("Despatcher")]
    public class ExportDespatchersWithTheirTrucksDTO
        {
        [XmlAttribute("TrucksCount")]
        public int Count { get; set; }

        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; }

        [XmlArray("Trucks")]
        public AllDTrucks[] Trucks { get; set; }
    }

    [XmlType("Truck")]
    public class AllDTrucks
        {
        [XmlElement("RegistrationNumber")]
        public string RegistrationNumber { get; set; }

        [XmlElement("Make")]
        public MakeType Make { get; set; }

    }
    }
