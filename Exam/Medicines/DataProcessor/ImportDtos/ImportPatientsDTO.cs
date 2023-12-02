using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ImportDtos
    {
    public class ImportPatientsDTO
        {
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string FullName { get; set; }

        [Required]
        [EnumDataType(typeof(AgeGroup))]
        public AgeGroup AgeGroup { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public int[] Medicines { get; set; }
        }
    }
