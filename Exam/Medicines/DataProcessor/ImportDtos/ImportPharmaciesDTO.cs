using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
    {
    [XmlType("Pharmacy")]
    public class ImportPharmaciesDTO
        {
        [XmlAttribute("non-stop")]
        public string IsNonStop { get; set; }

        [XmlElement("Name")]
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }

        [XmlElement("PhoneNumber")]
        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        [RegularExpression(@"^[(]\d{3}[)][ ]{1}\d{3}[-]\d{4}$")]
        public string PhoneNumber { get; set; }

        public AllMedicinesInPharmacy[] Medicines { get; set; }
        }

    [XmlType("Medicine")]
    public class AllMedicinesInPharmacy
        {
        [Required]
        [Range(0, 4)]
        [XmlAttribute("category")]
        public int Category { get; set; }

        [Required]
        [MaxLength(150)]
        [MinLength(3)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000.00)]
        [XmlElement("Price")]
        public decimal Price { get; set; }

        [Required]
        [XmlElement("ProductionDate")]
        public string ProductionDate { get; set; }


        [Required]
        [XmlElement("ExpiryDate")]
        public string ExpiryDate { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [XmlElement("Producer")]
        public string Producer { get; set; }
        }
    }
