
using Microsoft.EntityFrameworkCore;

namespace P01_StudentSystem.Data
    {
    public class StudentSystemContext :DbContext
        {
        private const string connectionString = @"Server=MSI\SQLEXPRESS;Database=SoftUni;Integrated Security=True;TrustServerCertificate=True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
            optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
