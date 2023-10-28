using _01.Models;
using Microsoft.EntityFrameworkCore;

namespace _01
    {
    public class ApplicationDBContext : DbContext
        {
        private const string conectionString = "Server=MSI\\SQLEXPRESS;Database=MinionsDB2;Integrated Security=True;TrustServerCertificate=True";


        //db Set 
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
            //install nuget
            //Microsoft.EntityFrameworkCore.SqlServer

            optionsBuilder.UseSqlServer(conectionString);

            }
        }
    }
