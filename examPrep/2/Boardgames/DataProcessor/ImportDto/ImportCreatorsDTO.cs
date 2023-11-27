using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
    {
    [XmlType("Creator")]
    public class ImportCreatorsDTO
        {
        [Required]
        [MinLength(2)]
        [MaxLength(7)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(7)]
        public string LastName { get; set; }

        public ImportBoardGamesDTO[] Boardgames { get; set; }
        }

    [XmlType("Boardgame")]
    public class ImportBoardGamesDTO
        {
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [Range(1, 10.00)]
        public double Rating { get; set; }

        [Required]
        [Range(2018, 2023)]
        public int YearPublished { get; set; }

        [Required]
        //[EnumDataType(typeof(CategoryType))]

        public int CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; }
        }
    }
