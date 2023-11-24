using Invoices.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Data
    {
    public class InvoicesContext : DbContext
        {

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductsClients> ProductsClients { get; set; }
        public InvoicesContext()
            {
            }

        public InvoicesContext(DbContextOptions options)
            : base(options)
            {
            }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
            if (!optionsBuilder.IsConfigured)
                {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
                }
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            //modelBuilder.Entity<ProductsClients>(e =>
            //{
            //    e.HasKey(k => new { k.ProductId, k.ClientId });
            //});
            }
        }
    }
