using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
    {
    [XmlType("Creator")]
    public class ExportCreatorsDTO
        {
        [XmlAttribute("BoardgamesCount")]
        public int Count { get; set; }

        [XmlElement("CreatorName")]
        public string CreatorName { get; set; }

        [XmlArray("Boardgames")]
        public AllBoardgames[] Boardgames { get; set; }
    }

    [XmlType("Boardgame")]
    public class AllBoardgames
        {
        [XmlElement("BoardgameName")]
        public string Name { get; set; }

        [XmlElement("BoardgameYearPublished")]
        public int Year { get; set; }
    }
    }
