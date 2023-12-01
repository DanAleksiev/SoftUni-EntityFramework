using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
    {
    [XmlType("Country")]
    public class ImportCountriesDTO
        {
        [XmlElement("CountryName")]
        [MinLength(4)]
        [MaxLength(60)]
        [Required]
        public string CountryName { get; set; }

        [XmlElement("ArmySize")]
        [Required]
        [Range(50000, 10000000)]
        public int ArmySize { get; set; }
    }
    }
