using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
    {
    [XmlType("Coach")]
    public class ImportCoachesDTO
        {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string Nationality { get; set; }

        public AllTeammates[] Footballers { get; set; }
        }

    [XmlType("Footballer")]
    public class AllTeammates
        {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string ContractStartDate { get; set; }
        [Required]
        public string ContractEndDate { get; set; }
        [Required]
        public int BestSkillType { get; set; }
        [Required]
        public int PositionType { get; set; }

        }
    }
