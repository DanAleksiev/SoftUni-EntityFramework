using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
    {
    public class Client
        {
        public Client()
            {
            Invoices = new List<Invoice>();
            Addresses = new List<Address>();
            ProductsClients = new List<ProductsClients>();
            }

        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(10), MaxLength(25)]
        public string? Name { get; set; }
        [Required]
        [MinLength(10), MaxLength(15)]
        public string? NumberVat { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
        public ICollection<Address>? Addresses { get; set; }
        public ICollection<ProductsClients>? ProductsClients { get; set; }
        }
    }