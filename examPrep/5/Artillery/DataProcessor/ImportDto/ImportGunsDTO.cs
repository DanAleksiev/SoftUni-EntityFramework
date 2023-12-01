using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Artillery.DataProcessor.ImportDto
    {
    public class ImportGunsDTO
        {
        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        [Range(100, 1350000)]
        public int GunWeight { get; set; }

        [Required]
        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }

        public int NumberBuild { get; set; }

        [Required]
        [Range(1.00, 100000)]
        public int Range { get; set; }

        [Required]
        public string GunType { get; set; }

        [Required]
        public int ShellId { get; set; }

        public AllCountries[] Countries { get; set; }
    }

    public class AllCountries
        {
        public int Id { get; set; }
    }
    }
