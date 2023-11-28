using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
    {
    [XmlType("Despatcher")]
    public class ImportDespatcherDTO
        {
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Name { get; set; }

        public string Position { get; set; }

        public AllTrucks[] Trucks { get; set; }

        }

    [XmlType("Truck")]
    public class AllTrucks
        {
        [RegularExpression(@"^([A-Z]{2}\d{4}[A-Z]{2})$")]
        public string RegistrationNumber { get; set; }

        [Required]
        [MinLength(17)]
        [MaxLength(17)]
        public string VinNumber { get; set; }

        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [Required]
        [Range(0,3)]
        public int CategoryType { get; set; }

        [Required]
        //[EnumDataType(typeof(MakeType))]
        [Range(0, 4)]
        public int MakeType { get; set; }
        }
    }
