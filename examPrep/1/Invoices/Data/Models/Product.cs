using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
    {
    public class Product

        {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(9), MaxLength(30)]
        public string? Name { get; set; }
        [Required]
        [Range(typeof(decimal), "5", "1000")]
        public decimal Price { get; set; }
        [Required]
        public CategoryTypesEnum CategoryType { get; set; }

        public ICollection<ProductClient> ProductsClients { get; set; } = new List<ProductClient>();

        }
    }
