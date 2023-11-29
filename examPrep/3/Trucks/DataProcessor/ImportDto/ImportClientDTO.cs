using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Trucks.DataProcessor.ImportDto
    {
    public class ImportClientDTO
        {
        [Required]
        [MaxLength(40)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Nationality { get; set; }

        [Required]
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Trucks")]

        public int[] Trucks { get; set; }
    }
    }
