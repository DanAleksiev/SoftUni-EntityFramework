using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Medicines.Data.Models.Enums;

namespace Medicines.Data.Models
    {
    public class Medicine
        {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000.00)]
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public DateTime ProductionDate { get; set; }


        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Producer { get; set; }

        [Required]
        public int PharmacyId { get; set; }

        [ForeignKey(nameof(PharmacyId))]
        public Pharmacy Pharmacy { get; set; }

        public ICollection<PatientMedicine> PatientsMedicines { get; set; } = new HashSet<PatientMedicine>();
        }
    }