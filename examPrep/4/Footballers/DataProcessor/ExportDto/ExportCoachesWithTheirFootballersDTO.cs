using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
    {
    [XmlType("Coach")]
    public class ExportCoachesWithTheirFootballersDTO
        {
        [XmlAttribute("FootballersCount")]
        public int Count { get; set; }

        [XmlElement("CoachName")]
        public string CoachName { get; set; }

        [XmlArray("Footballers")]
        public AllPlayers[] Footballers { get; set; }

        }

    [XmlType("Footballer")]
    public class AllPlayers
        {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }
        }
    }
