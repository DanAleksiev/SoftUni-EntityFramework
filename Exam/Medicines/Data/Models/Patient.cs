using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Medicines.Data.Models.Enums;

namespace Medicines.Data.Models
    {
    public class Patient
        {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string FullName { get; set; }

        [Required]
        public AgeGroup AgeGroup { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public ICollection<PatientMedicine> PatientsMedicines { get; set; } = new HashSet<PatientMedicine>();
        }
    }
