using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto
    {
    public class ImportSellerDTO
        {
        [Required]
        [MaxLength(20)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string Address { get; set; }

        [Required]
        public string Country { get; set; }


        [Required]
        [RegularExpression(@"^([w]{3}.{1}(\w*[-]*\w*).com)$")]
        public string Website { get; set; }

        public int[] Boardgames { get; set; }
    }
    }
