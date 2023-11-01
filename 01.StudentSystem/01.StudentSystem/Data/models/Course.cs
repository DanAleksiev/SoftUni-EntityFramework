
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
    {
    public class Course
        {
        [Key]
        public int CourseId { get; set; }

        [MaxLength(80)]
        [Unicode]
        [Required]
        public string Name { get; set; }

        [MaxLength(80)]
        [Unicode]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        }
    }
