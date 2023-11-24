using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Data.Models
    {
    public class ProductsClients
        {
        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Products? Product { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        }
    }