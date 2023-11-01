
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
    {
    public class StudentSystemContext :DbContext
        {
        private const string connectionString = @"Server=MSI\SQLEXPRESS;Database=SoftUni;Integrated Security=True;TrustServerCertificate=True";

        public StudentSystemContext(DbContextOptions options)
            : base(options)
            {

            }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> Homework { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<StudentCourse> StudentCourse { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
            optionsBuilder.UseSqlServer(connectionString);  
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });
            }
        }
    }
