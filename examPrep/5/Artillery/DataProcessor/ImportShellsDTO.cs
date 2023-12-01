using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor
    {
    [XmlType("Shell")]
    public class ImportShellsDTO
        {
        [Required]
        [Range(2, 1680)]
        public double ShellWeight { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Caliber { get; set; }
        }
    }
