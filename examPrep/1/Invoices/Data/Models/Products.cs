using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
    {
    public class Products
        
        {
        public Products()
            {
            ProductsClients = new List<ProductsClients>();
            }

        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(9), MaxLength(30)]
        public string? Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public CategoryTypesEnum CategoryItem { get; set; }

        public ICollection<ProductsClients> ProductsClients { get; set; }


    }
    }
