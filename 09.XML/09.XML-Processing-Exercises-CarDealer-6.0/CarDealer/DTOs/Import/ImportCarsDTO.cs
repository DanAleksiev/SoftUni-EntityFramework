using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
    {
    [XmlType("Car")]
    public class ImportCarsDTO
        {
        [XmlElement("make")]
        public string? Make { get; set; }

        [XmlElement("model")]
        public string? Model { get; set; }

        [XmlElement("traveledDistance")]
        public int traveledDistance { get; set; }

        [XmlArray("parts")]
        public PartsCollection[] Parts { get; set; }
        }

    [XmlType("partsId")]
    public class PartsCollection
        {
        [XmlAttribute("Id")]
        public int Id { get; set; }
        }
    }
