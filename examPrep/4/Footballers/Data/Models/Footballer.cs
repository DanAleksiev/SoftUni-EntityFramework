using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Footballers.Data.Models.Enums;

namespace Footballers.Data.Models
{
    public class Footballer
        {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public DateTime ContractStartDate { get; set; }

        [Required]
        public DateTime ContractEndDate { get; set; }

        [Required]
        public Position PositionType { get; set; }

        [Required]
        public BestSkill BestSkillType { get; set; }

        [Required]
        public int CoachId { get; set; }

        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; }

        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();

        }
    }
